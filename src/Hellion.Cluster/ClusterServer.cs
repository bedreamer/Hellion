﻿using Ether.Network;
using Ether.Network.Packets;
using Hellion.Cluster.Client;
using Hellion.Cluster.ISC;
using Hellion.Core.Configuration;
using Hellion.Database;
using Hellion.Core.IO;
using Hellion.Core.ISC.Structures;
using Hellion.Core.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Hellion.Core.Helpers;

namespace Hellion.Cluster
{
    /// <summary>
    /// Cluster server class.
    /// </summary>
    public class ClusterServer : NetServer<ClusterClient>
    {
        private const string ClusterConfigurationFile = "config/cluster.json";
        private const string DatabaseConfigurationFile = "config/database.json";
        
        private InterConnector connector;
        private Thread iscThread;

        /// <summary>
        /// Gets the cluster server configuration.
        /// </summary>
        public ClusterConfiguration ClusterConfiguration { get; private set; }

        /// <summary>
        /// Gets the database configuration.
        /// </summary>
        public DatabaseConfiguration DatabaseConfiguration { get; private set; }

        /// <summary>
        /// Gets the list of the conencted world servers.
        /// </summary>
        public ICollection<WorldServerInfo> ConnectedWorldServers { get; private set; }

        /// <summary>
        /// Creates a new ClusterServer instance.
        /// </summary>
        public ClusterServer()
            : base()
        {
            Console.Title = "Hellion ClusterServer";
            Log.Info("Starting ClusterServer...");

            this.ConnectedWorldServers = new List<WorldServerInfo>();
        }

        /// <summary>
        /// Dispose the server's resources.
        /// </summary>
        public override void DisposeServer()
        {
        }

        /// <summary>
        /// ClusterServer idle.
        /// </summary>
        protected override void Idle()
        {
            Log.Info("Server listening on port {0}", this.ClusterConfiguration.Port);

            while (this.IsRunning)
            {
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Initialize the ClusterServer.
        /// </summary>
        protected override void Initialize()
        {
            FFPacketHandler.Initialize<ClusterClient>();
            this.LoadConfiguration();
            this.ConnectToDatabase();
            this.ConnectToISC();

            Console.WriteLine();
        }

        /// <summary>
        /// On client connected.
        /// </summary>
        /// <param name="client">Client</param>
        protected override void OnClientConnected(ClusterClient client)
        {
            Log.Info("New client connected from {0}", client.Socket.RemoteEndPoint.ToString());

            client.Server = this;
        }

        /// <summary>
        /// On client disconnected.
        /// </summary>
        /// <param name="client">Client</param>
        protected override void OnClientDisconnected(ClusterClient client)
        {
            Log.Info("Client with id {0} disconnected.", client.Id);
        }

        /// <summary>
        /// Split incoming buffer into several FFPacket.
        /// </summary>
        /// <param name="buffer">Incoming buffer</param>
        /// <returns></returns>
        protected override IReadOnlyCollection<NetPacketBase> SplitPackets(byte[] buffer)
        {
            return FFPacket.SplitPackets(buffer);
        }

        /// <summary>
        /// Load the ClusterServer configuration.
        /// </summary>
        private void LoadConfiguration()
        {
            Log.Loading("Loading configuration...");

            if (File.Exists(ClusterConfigurationFile) == false)
                JsonHelper.Save(new ClusterConfiguration(), ClusterConfigurationFile);

            this.ClusterConfiguration = JsonHelper.Load<ClusterConfiguration>(ClusterConfigurationFile);

            this.ServerConfiguration.Ip = this.ClusterConfiguration.Ip;
            this.ServerConfiguration.Port = this.ClusterConfiguration.Port;

            if (File.Exists(DatabaseConfigurationFile) == false)
                JsonHelper.Save(new DatabaseConfiguration(), DatabaseConfigurationFile);

            this.DatabaseConfiguration = JsonHelper.Load<DatabaseConfiguration>(DatabaseConfigurationFile);

            Log.Done("Configuration loaded!\t\t\t");
        }


        /// <summary>
        /// Connect to the database.
        /// </summary>
        private void ConnectToDatabase()
        {
            try
            {
                Log.Loading("Connecting to database...");

                DatabaseService.Initialize(this.DatabaseConfiguration.Ip,
                    this.DatabaseConfiguration.User,
                    this.DatabaseConfiguration.Password,
                    this.DatabaseConfiguration.DatabaseName);

                Log.Done("Connected to database!\t\t\t");
            }
            catch (Exception e)
            {
                Log.Error($"Cannot connect to database. {e.Message}");
            }
        }

        /// <summary>
        /// Connect to the Inter-Server.
        /// </summary>
        private void ConnectToISC()
        {
            Log.Loading("Connecting to Inter-Server...");

            this.connector = new InterConnector(this);

            try
            {
                var resolvedIp = HostResolver.ResolveToIp(this.ClusterConfiguration.ISC.Ip);
                this.connector.Connect(resolvedIp, this.ClusterConfiguration.ISC.Port);
                this.iscThread = new Thread(this.connector.Run);
                this.iscThread.Start();
            }
            catch (Exception e)
            {
                Log.Error("Cannot connect to ISC. {0}", e.Message);
                Environment.Exit(0);
            }

            Log.Done("Connected to Inter-Server!\t\t\t");
        }
    }
}
