using Newtonsoft.Json;
using NordVPNModels.Models;
using partycli.Helpers;
using partycli.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace partycli.BusinnessLogic
{
    public class NordVpnBusinessLogic
    {
        private readonly INordApiService _apiService; 

        public NordVpnBusinessLogic(INordApiService apiService)
        {  
            _apiService = apiService; 
        }

        public async Task HandleCommandAsync(string[] args)
        {
            var currentState = States.none;
            string name = null;
            int argIndex = 1;

            foreach (string arg in args)
            {
                if (currentState == States.none)
                {
                    if (arg == "server_list")
                    {
                        currentState = States.server_list;
                        if (argIndex >= args.Length)
                        {
                            var serverList = await _apiService.GetAllServersListAsync();
                            StoreValue("serverlist", serverList, false);
                            Log("Saved new server list: " + serverList);
                            DisplayList(serverList);
                        }
                    }
                    if (arg == "config")
                    {
                        currentState = States.config;
                    }
                }
                else if (currentState == States.config)
                {
                    if (name == null)
                    {
                        name = arg;
                    }
                    else
                    {
                        StoreValue(NordVpnHelper.ProccessName(name), arg);
                        Log("Changed " + NordVpnHelper.ProccessName(name) + " to " + arg);
                        name = null;
                    }
                }
                else if (currentState == States.server_list)
                {
                    if (arg == "--local")
                    {
                        if (!String.IsNullOrEmpty(Properties.Settings.Default.serverlist))
                        {
                            DisplayList(Properties.Settings.Default.serverlist);
                        }
                        else
                        {
                            Console.WriteLine("Error: There are no server data in local storage");
                        }
                    }
                    else if (arg == "--france")
                    {
                        //france == 74
                        //albania == 2
                        //Argentina == 10
                        var query = new VpnServerQuery(null, 74, null, null, null, null);
                        var serverList = await _apiService.GetAllServerByCountryListAsync(query.CountryId.Value); //France id == 74
                        StoreValue("serverlist", serverList, false);
                        Log("Saved new server list: " + serverList);
                        DisplayList(serverList);
                    }
                    else if (arg == "--TCP")
                    {
                        //UDP = 3
                        //Tcp = 5
                        //Nordlynx = 35
                        var query = new VpnServerQuery(5, null, null, null, null, null);
                        var serverList = await _apiService.GetAllServerByProtocolListAsync((int)query.Protocol.Value);
                        StoreValue("serverlist", serverList, false);
                        Log("Saved new server list: " + serverList);
                        DisplayList(serverList);
                    }
                }
                argIndex = argIndex + 1;
            }

            if (currentState == States.none)
            {
                Console.WriteLine("To get and save all servers, use command: partycli.exe server_list");
                Console.WriteLine("To get and save France servers, use command: partycli.exe server_list --france");
                Console.WriteLine("To get and save servers that support TCP protocol, use command: partycli.exe server_list --TCP");
                Console.WriteLine("To see saved list of servers, use command: partycli.exe server_list --local ");
            }
            Console.Read();
        }

        static void StoreValue(string name, string value, bool writeToConsole = true)
        {
            try
            {
                var settings = Properties.Settings.Default;
                settings[name] = value;
                settings.Save();
                if (writeToConsole)
                {
                    Console.WriteLine("Changed " + name + " to " + value);
                }
            }
            catch
            {
                Console.WriteLine("Error: Couldn't save " + name + ". Check if command was input correctly.");
            }

        }

        static void DisplayList(string serverListString)
        {
            var serverlist = JsonConvert.DeserializeObject<List<ServerModel>>(serverListString);
            Console.WriteLine("Server list: ");
            for (var index = 0; index < serverlist.Count; index++)
            {
                Console.WriteLine("Name: " + serverlist[index].Name);
            }
            Console.WriteLine("Total servers: " + serverlist.Count);
        }

        static void Log(string action)
        {
            var newLog = new LogModel
            {
                Action = action,
                Time = DateTime.Now
            };
            List<LogModel> currentLog;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.log))
            {
                currentLog = JsonConvert.DeserializeObject<List<LogModel>>(Properties.Settings.Default.log);
                currentLog.Add(newLog);
            }
            else
            {
                currentLog = new List<LogModel> { newLog };
            }

            StoreValue("log", JsonConvert.SerializeObject(currentLog), false);
        }

    }
}
