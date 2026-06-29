using System.Threading.Tasks;

namespace partycli.Interfaces
{
    public interface INordApiService
    {
        Task<string> GetAllServersListAsync();
        Task<string> GetAllServerByCountryListAsync(int countryId);
        Task<string> GetAllServerByProtocolListAsync(int vpnProtocol);

    }
}
