using Microsoft.Extensions.DependencyInjection;
using partycli.BusinessLogic;
using partycli.Interfaces;
using partycli.Services;
using System;

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

            Console.Read();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<INordApiService, NordApiService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<INetworkProtocolService, NetworkProtocolService>();
            services.AddTransient<INordDataStoreService, NordDataStoreService>();

            services.AddTransient<NordVpnBusinessLogic>();

            return services.BuildServiceProvider();
        }
    }
}
