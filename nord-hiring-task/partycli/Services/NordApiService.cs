using Newtonsoft.Json;
using NordVPNModels.Models;
using partycli.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace partycli.Services
{
    public class NordApiService : INordApiService
    {
        private readonly HttpClient _client;

        public NordApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetAllServersListAsync()
        {
            var url = "https://api.nordvpn.com/v1/servers";
            var response = await _client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAllServerByCountryListAsync(int countryId)
        {
            var url = "https://api.nordvpn.com/v1/servers?filters[servers_technologies][id]=35&filters[country_id]=";
            var response = await _client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAllServerByProtocolListAsync(int vpnProtocol)
        {
            var url = "https://api.nordvpn.com/v1/servers?filters[servers_technologies][id]=";
            var response = await _client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
