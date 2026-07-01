using NordVPNModels.Constants;
using partycli.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

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
            var response = await _client.GetAsync(NordApiEndpointConstants.BASE_URL);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAllServerByCountryListAsync(int countryId)
        {
            var url = NordApiEndpointConstants.BASE_URL + NordApiEndpointConstants.COUNTRIES_LIST_ENDPOINT + countryId;
            var response = await _client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
         
        public async Task<string> GetAllServerByProtocolListAsync(int vpnProtocol)
        {
            var url = NordApiEndpointConstants.BASE_URL + NordApiEndpointConstants.PROTOCOL_LIST_ENDPOINT + vpnProtocol;
            var response = await _client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
