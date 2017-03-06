using Ether.Network.Packets;
using Hellion.Core.Data.Headers;
using Hellion.Core.ISC.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.Login.ISC
{
    public partial class InterClient
    {
        /// <summary>
        /// Send the authentication result.
        /// </summary>
        /// <param name="result"></param>
        private void SendAuthenticationResult(bool result)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((int)InterHeaders.AuthenticationResult);
                packet.Write(result);

                this.Send(packet);
            }
        }

        public void SendWorldServerListToCluster(int clusterId)
        {
            InterClient clusterClient = this.Server.GetClusterById(clusterId);
            IEnumerable<WorldServerInfo> worlds = this.Server.GetWorldsByClusterId(clusterId);

            if (clusterClient != null && worlds.Any())
            {
                using (var packet = new NetPacket())
                {
                    packet.Write((int)InterHeaders.UpdateWorldServerList);
                    packet.Write(worlds.Count());

                    foreach (var worldServer in worlds)
                    {
                        packet.Write(worldServer.Id);
                        packet.Write(worldServer.Ip);
                        packet.Write(worldServer.Name);
                        packet.Write(worldServer.Capacity);
                        packet.Write(worldServer.ConnectedPlayerCount);
                    }

                    clusterClient.Send(packet);
                }
            }
        }
    }
}
