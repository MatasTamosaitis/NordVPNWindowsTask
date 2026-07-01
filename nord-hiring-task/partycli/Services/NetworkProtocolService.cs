using NordVPNModels.Models;
using partycli.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace partycli.Services
{
    public class NetworkProtocolService : INetworkProtocolService
    {
        public int GetNetworkProtocolIdByName(string protocolName)
        {
            var protocol = _networkProtocol.Where(x => x.NetworkProtocolName == protocolName).First();

            return protocol.NetworkProtocolId;
        }

        #region
        private static readonly List<NetworkProtocol> _networkProtocol = new List<NetworkProtocol>
            {
                new NetworkProtocol
                {
                    NetworkProtocolId = 3,
                    NetworkProtocolName = "UDP"
                },
                new NetworkProtocol
                {
                    NetworkProtocolId = 5,
                    NetworkProtocolName = "TCP"
                },
                new NetworkProtocol
                {
                    NetworkProtocolId = 35,
                    NetworkProtocolName = "NordLynx"
                }
            };
        #endregion
    }
}
