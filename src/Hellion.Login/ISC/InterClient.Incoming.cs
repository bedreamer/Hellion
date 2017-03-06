using Ether.Network.Packets;
using Hellion.Core.Data.Headers;
using Hellion.Core.IO;
using Hellion.Core.ISC.Structures;

namespace Hellion.Login.ISC
{
    public partial class InterClient
    {
        private void OnAuthenticate(NetPacketBase packet)
        {
            var serverTypeNumber = packet.Read<int>();
            var interPassword = packet.Read<string>();
            var serverType = (InterServerType)serverTypeNumber;

            if (interPassword.ToLower() != this.Server.Configuration.Password.ToLower())
            {
                Log.Warning("A client tried to authenticate with an incorect password.");
                this.Server.RemoveClient(this);
                return;
            }

            if (serverType == InterServerType.Cluster)
            {
                int clusterId = packet.Read<int>();
                string clusterName = packet.Read<string>();
                string clusterIp = packet.Read<string>();

                if (this.Server.HasClusterWithId(clusterId))
                {
                    Log.Warning("A cluster server with same id is already connected to the ISC.");
                    this.SendAuthenticationResult(false);
                    this.Server.RemoveClient(this);
                    return;
                }

                this.ServerInfo = new ClusterServerInfo(clusterId, clusterIp, clusterName);
                Log.Info("New ClusterServer '{0}' authenticated from {1}", clusterName, this.Socket.RemoteEndPoint.ToString());
            }

            if (serverType == InterServerType.World)
            {
                int clusterId = packet.Read<int>();
                int worldId = packet.Read<int>();
                string worldName = packet.Read<string>();
                string worldIp = packet.Read<string>();
                int capacity = packet.Read<int>();
                int connectedPlayerCount = packet.Read<int>();

                if (this.Server.HasClusterWithId(clusterId) == false)
                {
                    Log.Warning("WorldServer '{0}' tried to connect to an unknown cluster.", worldName);
                    this.SendAuthenticationResult(false);
                    this.Server.RemoveClient(this);
                    return;
                }

                if (this.Server.HasWorldInCluster(clusterId, worldId))
                {
                    Log.Warning("A WorldServer with id {0} already exists in Cluster {1}", worldId, clusterId);
                    this.SendAuthenticationResult(false);
                    this.Server.RemoveClient(this);
                    return;
                }

                this.ServerInfo = new WorldServerInfo(worldId, clusterId, capacity, worldIp, worldName, connectedPlayerCount);
                Log.Info("New WorldServer '{0}' authenticated from {1}", worldName, this.Socket.RemoteEndPoint.ToString());
            }

            this.ServerType = serverType;
            this.SendAuthenticationResult(true);
            this.Server.RefreshServerList();

            foreach (var cluster in this.Server.GetClusters())
                this.SendWorldServerListToCluster(cluster.Id);
        }
    }
}
