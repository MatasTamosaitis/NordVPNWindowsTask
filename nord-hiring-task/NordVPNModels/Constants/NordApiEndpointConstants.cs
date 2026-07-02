
namespace NordVPNModels.Constants
{
    public static class NordApiEndpointConstants
    {
        //I made this simple for the test but theres a multitude of options to improve it
        //like storing it in app.config or even a singleton
        public const string BASE_URL = "https://api.nordvpn.com/v1/servers";
        public const string COUNTRIES_LIST_ENDPOINT = "?filters[servers_technologies][id]=35&filters[country_id]=";
        public const string PROTOCOL_LIST_ENDPOINT = "?filters[servers_technologies][id]=";
    }
}
