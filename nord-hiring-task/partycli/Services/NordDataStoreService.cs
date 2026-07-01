using Newtonsoft.Json;
using NordVPNModels.Models;
using partycli.Interfaces;
using System;
using System.Collections.Generic;

namespace partycli.Services
{
    public class NordDataStoreService : INordDataStoreService
    {
        public void StoreValue(string name, string value, bool writeToConsole = true)
        {
            try
            {
                var settings = Properties.Settings.Default;
                settings[name] = value;
                settings.Save();

                if (writeToConsole)
                {
                    Console.WriteLine($"Changed {name} to {value}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Couldn't save {name}. Details: {ex.Message}");
            }
        }

        public void Log(string action)
        {
            var newLog = new LogModel
            {
                Action = action,
                Time = DateTime.Now
            };

            List<LogModel> currentLog = new List<LogModel>();
            string savedLog = Properties.Settings.Default.log;

            if (!string.IsNullOrEmpty(savedLog))
            {
                currentLog = JsonConvert.DeserializeObject<List<LogModel>>(savedLog) ?? new List<LogModel>();
            }

            currentLog.Add(newLog);
            StoreValue("log", JsonConvert.SerializeObject(currentLog), writeToConsole: false);
        }
    }
}
