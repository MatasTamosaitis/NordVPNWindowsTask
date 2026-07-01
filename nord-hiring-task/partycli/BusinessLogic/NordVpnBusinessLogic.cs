using System;
using System.Collections.Generic;
using System.Linq;
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

        public NordVpnBusinessLogic(INordApiService apiService, INetworkProtocolService networkProtocolService, ICountryService countryService, INordDataStoreService nordDataStoreService)
        {
            _apiService = apiService;
            _networkProtocolService = networkProtocolService;
            _countryService = countryService;
            _nordDataStoreService = nordDataStoreService;
        }

        public async Task HandleCommandAsync(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                ProcessNameAndDisplayHelper.ToolTipDisplay();
                return;
            }

            string command = args[0].ToLowerInvariant();

            switch (command)
            {
                case CommandConstants.SERVER_LIST:
                    await HandleServerListCommandAsync(args.Skip(1).ToArray());
                    break;

                case CommandConstants.CONFIG:
                    HandleConfigCommand(args.Skip(1).ToArray());
                    break;

                default:
                    ProcessNameAndDisplayHelper.ToolTipDisplay();
                    break;
            }

            Console.Read();
        }

        private async Task HandleServerListCommandAsync(string[] subArgs)
        {
            // if no sub command selected then get all servers
            if (subArgs.Length == 0)
            {
                var serverList = await _apiService.GetAllServersListAsync();
                ProcessAndSaveServerList(serverList);
                return;
            }

            string command = subArgs[0].ToLowerInvariant();

            switch (command)
            {
                case CommandConstants.LOCAL_OPTION:
                    string localList = Properties.Settings.Default.serverlist;
                    if (!string.IsNullOrEmpty(localList))
                    {
                        DisplayList(localList);
                    }
                    else
                    {
                        Console.WriteLine("Error: There are no server data in local storage");
                    }
                    break;
                //if command contains a country try to retrieve servers by country.
                case CommandConstants.COUNTRY_OPTION:
                    if (subArgs.Length > 1)
                    {
                        string countryInput = subArgs[1]; 

                        int countryId = _countryService.GetCountryIdByName(countryInput);

                        if (countryId != 0) 
                        {
                            var countryList = await _apiService.GetAllServerByCountryListAsync(countryId);
                            ProcessAndSaveServerList(countryList);
                        }
                        else
                        {
                            Console.WriteLine($"Error: Country '{countryInput}' is not supported.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Please specify a valid country. e.g. --country albania");
                    }
                    break;
                //if command contains a network protocol try to retrieve protocols.
                case CommandConstants.NETWORK_PROTOCOL_OPTION:
                    if (subArgs.Length > 1)
                    {
                        string protocolInput = subArgs[1].ToLowerInvariant(); 
                        int protocolId = _networkProtocolService.GetNetworkProtocolIdByName(protocolInput);

                        if (protocolId != 0)
                        {
                            var protocolList = await _apiService.GetAllServerByProtocolListAsync(protocolId);
                            ProcessAndSaveServerList(protocolList);
                        }
                        else
                        {
                            Console.WriteLine($"Error: Protocol '{protocolInput}' is not supported.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Please specify a valid protocol. e.g. --protocol udp");
                    }
                    break;

                //if there is no valid command then display message
                default:
                    Console.WriteLine($"Unknown command: {command}");
                    ProcessNameAndDisplayHelper.ToolTipDisplay();
                    break;
            }
        }

        private void HandleConfigCommand(string[] subArgs)
        {
            if (subArgs.Length < 2)
            {
                Console.WriteLine("Error: 'config' command requires a setting name and a value.");
                return;
            }

            for (int i = 0; i < subArgs.Length - 1; i += 2)
            {
                string rawName = subArgs[i];
                string value = subArgs[i + 1];

                string processedName = ProcessNameAndDisplayHelper.ProccessName(rawName);
                _nordDataStoreService.StoreValue(processedName, value);
                _nordDataStoreService.Log($"Changed {processedName} to {value}");
            }
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
