using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NordVPNModels.Constants;
using NordVPNModels.Models;
using partycli.Helpers;
using partycli.Interfaces;

namespace partycli.BusinessLogic
{
    public class NordVpnBusinessLogic
    {
        private readonly INordApiService _apiService;
        private readonly INetworkProtocolService _networkProtocolService;
        private readonly ICountryService _countryService;
        private readonly INordDataStoreService _nordDataStoreService;

        public NordVpnBusinessLogic(
            INordApiService apiService,
            INetworkProtocolService networkProtocolService,
            ICountryService countryService,
            INordDataStoreService nordDataStoreService)
        {
            _apiService = apiService;
            _networkProtocolService = networkProtocolService;
            _countryService = countryService;
            _nordDataStoreService = nordDataStoreService;
        }

        public async Task HandleCommandAsync(string[] args)
        {
            var rootCommand = new RootCommand("PartyCLI - Nord Server Management Tool");

            var serverListCommand = new Command(CommandConstants.SERVER_LIST, "Retrieves and displays available servers.");

            var localOption = new Option<bool>(CommandConstants.LOCAL_OPTION)
            {
                Description = "View server data from local storage."
            };

            var countryOption = new Option<string>(CommandConstants.COUNTRY_OPTION)
            {
                Description = "Filter servers by country name."
            };

            var protocolOption = new Option<string>(CommandConstants.NETWORK_PROTOCOL_OPTION)
            {
                Description = "Filter servers by network protocol UDP/TCP/NordLynx."
            };

            serverListCommand.Add(localOption);
            serverListCommand.Add(countryOption);
            serverListCommand.Add(protocolOption);

            serverListCommand.SetAction((parseResult) =>
            {
                bool localValue = parseResult.GetValue(localOption);
                string countryValue = parseResult.GetValue(countryOption);
                string protocolValue = parseResult.GetValue(protocolOption);

                HandleServerListExecutionAsync(localValue, countryValue, protocolValue).GetAwaiter().GetResult();
            });

            rootCommand.Add(serverListCommand);

            if (args == null || args.Length == 0)
            {
                ProcessNameAndDisplayHelper.ToolTipDisplay();
                return;
            }

            var parseResultOutput = rootCommand.Parse(args);
            await parseResultOutput.InvokeAsync();
        }

        private async Task HandleServerListExecutionAsync(bool local, string country, string protocol)
        {
            if (local)
            {
                string localList = Properties.Settings.Default.serverlist;
                if (!string.IsNullOrEmpty(localList))
                {
                    DisplayList(localList);
                }
                else
                {
                    Console.WriteLine("There is server data in local storage");
                }
                return;
            }

            if (!string.IsNullOrWhiteSpace(country))
            {
                var countryId = _countryService.GetCountryIdByName(country);
                if (countryId != -1)
                {
                    var countryList = await _apiService.GetAllServerByCountryListAsync(countryId);
                    ProcessAndSaveServerList(countryList);
                }
                else
                {
                    Console.WriteLine($"Error: Country '{country}' is not found.");
                }
                return;
            }

            if (!string.IsNullOrWhiteSpace(protocol))
            {
                int protocolId = _networkProtocolService.GetNetworkProtocolIdByName(protocol);
                if (protocolId != -1)
                {
                    var protocolList = await _apiService.GetAllServerByProtocolListAsync(protocolId);
                    ProcessAndSaveServerList(protocolList);
                }
                else
                {
                    Console.WriteLine($"Error: Protocol '{protocol}' is not found.");
                }
                return;
            }

            var serverList = await _apiService.GetAllServersListAsync();
            ProcessAndSaveServerList(serverList);
        }

        private void ProcessAndSaveServerList(string serverList)
        {
            _nordDataStoreService.StoreValue("serverlist", serverList, writeToConsole: false);
            _nordDataStoreService.Log("Saved new server list: " + serverList);

            DisplayList(serverList);
        }

        private void DisplayList(string serverListString)
        {
            try
            {
                var serverList = JsonConvert.DeserializeObject<List<ServerModel>>(serverListString);
                if (serverList == null) return;

                Console.WriteLine("Server list: ");
                foreach (var server in serverList)
                {
                    Console.WriteLine($"Name: {server.Name}");
                }
                Console.WriteLine($"Total servers: {serverList.Count}");
            }
            catch (JsonException)
            {
                Console.WriteLine("Local server list is busted.");
            }
        }
    }
}