using Ether.Network;
using Ether.Network.Packets;
using Hellion.Core.Configuration;
using Hellion.Core.Helpers;
using Hellion.Core.IO;
using Hellion.Core.Network;
using Hellion.Database;
using Hellion.World.Client;
using Hellion.World.ISC;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Hellion.World
{
    public partial class WorldServer : NetServer<WorldClient>
    {
        private const string WorldConfigurationFile = "config/world.json";
        private const string DatabaseConfigurationFile = "config/database.json";

        private DatabaseContext dbContext;
        private InterConnector connector;
        private Thread iscThread;

        /// <summary>
        /// Gets the world server configuration.
        /// </summary>
        public WorldConfiguration WorldConfiguration { get; private set; }

        /// <summary>
        /// Gets the database configuration.
        /// </summary>
        public DatabaseConfiguration DatabaseConfiguration { get; private set; }

        /// <summary>
        /// Creates a new WorldServer instance.
        /// </summary>
        public WorldServer()
            : base()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
            Console.Title = "Hellion WorldServer";
            Log.Info("Starting WorldServer...");
        }

        /// <summary>
        /// Dispose the server's resources.
        /// </summary>
        public override void DisposeServer()
        {
        }

        /// <summary>
        /// WorldServer idle.
        /// </summary>
        protected override void Idle()
        {
            Log.Info("Server listening on port {0}", this.WorldConfiguration.Port);

            while (this.IsRunning)
            {
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Initialize the WorldServer.
        /// </summary>
        protected override void Initialize()
        {
            FFPacketHandler.Initialize<WorldClient>();
            this.LoadConfiguration();
            this.ConnectToDatabase();
            this.LoadData();
            this.ConnectToISC();

            Console.WriteLine();
        }

        /// <summary>
        /// On client connected.
        /// </summary>
        /// <param name="client">Client</param>
        protected override void OnClientConnected(WorldClient client)
        {
            Log.Info("New client connected from {0}", client.Socket.RemoteEndPoint.ToString());

            client.Server = this;
        }

        /// <summary>
        /// On client disconnected.
        /// </summary>
        /// <param name="client">Client</param>
        protected override void OnClientDisconnected(WorldClient client)
        {
            client.Disconnected();
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
        /// Load the WorldServer configuration.
        /// </summary>
        private void LoadConfiguration()
        {
            Log.Info("Loading configuration...");

            if (File.Exists(WorldConfigurationFile) == false)
                JsonHelper.Save(new WorldConfiguration(), WorldConfigurationFile);

            this.WorldConfiguration = JsonHelper.Load<WorldConfiguration>(WorldConfigurationFile);

            this.ServerConfiguration.Ip = this.WorldConfiguration.Ip;
            this.ServerConfiguration.Port = this.WorldConfiguration.Port;

            if (File.Exists(DatabaseConfigurationFile) == false)
                JsonHelper.Save(new DatabaseConfiguration(), DatabaseConfigurationFile);

            this.DatabaseConfiguration = JsonHelper.Load<DatabaseConfiguration>(DatabaseConfigurationFile);

            Log.Done("Configuration loaded!");
        }


        /// <summary>
        /// Connect to the database.
        /// </summary>
        private void ConnectToDatabase()
        {
            try
            {
                Log.Info("Connecting to database...");

                this.dbContext = new DatabaseContext(this.DatabaseConfiguration.Ip,
                    this.DatabaseConfiguration.User,
                    this.DatabaseConfiguration.Password,
                    this.DatabaseConfiguration.DatabaseName);
                this.dbContext.Database.EnsureCreated();

                DatabaseService.InitializeDatabase(dbContext);

                Log.Done("Connected to database!");
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
            Log.Info("Connecting to Inter-Server...");

            this.connector = new InterConnector(this);

            try
            {
                var resolvedIp = HostResolver.ResolveToIp(this.WorldConfiguration.ISC.Ip);
                this.connector.Connect(resolvedIp, this.WorldConfiguration.ISC.Port);
                this.iscThread = new Thread(this.connector.Run);
                this.iscThread.Start();
            }
            catch (Exception e)
            {
                Log.Error("Cannot connect to ISC. {0}", e.Message);
                Environment.Exit(0);
            }

            Log.Done("Connected to Inter-Server!");
        }
    }
}
