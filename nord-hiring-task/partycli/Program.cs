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
           
            services.AddHttpClient<INordApiService, NordApiService>();

            //since this won't be used by that many people using AddTransient should be fine for this use case but if there was
            //more in the future then using AddScoped is much more efficient.
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<INetworkProtocolService, NetworkProtocolService>();
            services.AddTransient<INordDataStoreService, NordDataStoreService>();

            services.AddTransient<NordVpnBusinessLogic>();

            return services.BuildServiceProvider();
        }
    }
}
