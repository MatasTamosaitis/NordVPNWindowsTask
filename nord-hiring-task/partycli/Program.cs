using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NordVPNModels.Models;
using partycli.BusinnessLogic;
using partycli.Interfaces;
using partycli.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace partycli
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();

            var app = serviceProvider.GetRequiredService<NordVpnBusinessLogic>();

            try
            { 
                app.HandleCommandAsync(args).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<INordApiService, NordApiService>();

            services.AddTransient<NordVpnBusinessLogic>();

            return services.BuildServiceProvider();
        }
    }
}
