using Ether.Network;
using Hellion.Core.Configuration;
using Hellion.Core.Data.Headers;
using Hellion.Core.IO;
using Hellion.Core.ISC.Structures;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Hellion.Login.ISC
{
    public sealed class InterServer : NetServer<InterClient>
    {
        /// <summary>
        /// Gets the LoginServer instance.
        /// </summary>
        internal LoginServer LoginServer { get; private set; }

        /// <summary>
        /// Gets the ISC Configuration.
        /// </summary>
        internal ISCConfiguration Configuration { get; private set; }

        /// <summary>
        /// Creates a new InterServer instance.
        /// </summary>
        /// <param name="loginServer"></param>
        public InterServer(LoginServer loginServer)
            : base()
        {
            this.LoginServer = loginServer;
        }

        /// <summary>
        /// Dispose the server resources.
        /// </summary>
        public override void DisposeServer()
        {
        }

        /// <summary>
        /// Server idle state.
        /// </summary>
        protected override void Idle()
        {
            while (this.IsRunning)
                Thread.Sleep(100);
        }

        /// <summary>
        /// Initialize the ISC Server.
        /// </summary>
        protected override void Initialize()
        {
            this.Configuration = this.LoginServer.LoginConfiguration.ISC;
            this.ServerConfiguration.Ip = this.Configuration.Ip;
            this.ServerConfiguration.Port = this.Configuration.Port;
        }
        
        protected override void OnClientConnected(InterClient client)
        {
            Log.Info("New inter client connected from {0}.", client.Socket.RemoteEndPoint.ToString());

            client.Server = this;
        }

        protected override void OnClientDisconnected(InterClient client)
        {
            client.Disconnected();

            this.RefreshServerList();
        }

        internal void RefreshServerList()
        {
            var clusters = this.GetClusters();

            LoginServer.Clusters.Clear();
            foreach (var cluster in clusters)
            {
                cluster.Worlds.Clear();
                var worldsInCluster = this.GetWorldsByClusterId(cluster.Id);

                foreach (var world in worldsInCluster)
                    cluster.Worlds.Add(world);
                LoginServer.Clusters.Add(cluster);  
            }
        }

        /// <summary>
        /// Gets the cluster list.
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<ClusterServerInfo> GetClusters()
        {
            return from x in this.Clients
                   where x.ServerType == InterServerType.Cluster
                   where x.Socket.Connected
                   select x.ServerInfo as ClusterServerInfo;
        }

        /// <summary>
        /// Get all worlds connected by cluster Id.
        /// </summary>
        /// <param name="clusterId">Parent cluster Id</param>
        /// <returns></returns>
        internal IEnumerable<WorldServerInfo> GetWorldsByClusterId(int clusterId)
        {
            return from x in this.Clients
                   where x.ServerType == InterServerType.World
                   where (x.ServerInfo as WorldServerInfo).ClusterId == clusterId
                   where x.Socket.Connected
                   select x.ServerInfo as WorldServerInfo;
        }

        /// <summary>
        /// Get cluster server by Id.
        /// </summary>
        /// <param name="clusterId">Cluster Server Id</param>
        /// <returns></returns>
        internal InterClient GetClusterById(int clusterId)
        {
            return (from x in this.Clients
                    where x.ServerType == InterServerType.Cluster
                    where (x.ServerInfo as ClusterServerInfo).Id == clusterId
                    select x).FirstOrDefault();
        }

        /// <summary>
        /// Get world server by Id.
        /// </summary>
        /// <param name="worldId">World server Id</param>
        /// <returns></returns>
        internal InterClient GetWorldById(int worldId)
        {
            return (from x in this.Clients
                    where x.ServerType == InterServerType.World
                    where (x.ServerInfo as WorldServerInfo).Id == worldId
                    select x).FirstOrDefault();
        }

        /// <summary>
        /// Check if there is a cluster server with the same Id.
        /// </summary>
        /// <param name="clusterId">Cluster Server Id</param>
        /// <returns></returns>
        internal bool HasClusterWithId(int clusterId)
        {
            return this.GetClusterById(clusterId) != null;
        }

        /// <summary>
        /// Check if the cluster id has worlds.
        /// </summary>
        /// <param name="clusterId"></param>
        /// <returns></returns>
        internal bool HasWorldWithClusterId(int clusterId)
        {
            return this.GetWorldsByClusterId(clusterId).Any();
        }

        /// <summary>
        /// Check if there is a world in a cluster.
        /// </summary>
        /// <param name="clusterId"></param>
        /// <param name="worldId"></param>
        /// <returns></returns>
        internal bool HasWorldInCluster(int clusterId, int worldId)
        {
            var worlds = this.GetWorldsByClusterId(clusterId);

            return (from x in worlds
                    where x.Id == worldId
                    select x).Any();
        }
    }
}
