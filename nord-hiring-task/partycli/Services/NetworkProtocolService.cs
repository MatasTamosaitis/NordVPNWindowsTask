using NordVPNModels.Models;
using partycli.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace partycli.Services
{
    public class NetworkProtocolService : INetworkProtocolService
    {
        public int GetNetworkProtocolIdByName(string protocolName)
        {
            if (string.IsNullOrWhiteSpace(protocolName))
            {
                return -1;
            }

            var protocol = _networkProtocol.FirstOrDefault(x =>
                x.NetworkProtocolName.Equals(protocolName, StringComparison.OrdinalIgnoreCase));

            return protocol != null ? protocol.NetworkProtocolId : -1;
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
