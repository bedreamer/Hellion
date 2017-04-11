using Ether.Network;
using Ether.Network.Packets;
using Hellion.Core;
using Hellion.Core.Data.Headers;
using Hellion.Core.IO;
using Hellion.Core.Network;
using Hellion.Database.Structures;
using Hellion.World.Systems;
using System.Net.Sockets;

namespace Hellion.World.Client
{
    /// <summary>
    /// Represents a world client connected to the world server.
    /// </summary>
    public partial class WorldClient : NetConnection
    {
        private uint sessionId;

        /// <summary>
        /// Gets the player account informations.
        /// </summary>
        public DbUser CurrentUser { get; set; }

        /// <summary>
        /// Gets or sets the current player.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// Gets or sets the WorldServer reference.
        /// </summary>
        public WorldServer Server { get; set; }

        /// <summary>
        /// Creates a new WorldClient instance.
        /// </summary>
        public WorldClient()
            : base()
        {
            this.sessionId = (uint)Global.GenerateRandomNumber();
        }

        /// <summary>
        /// Creates a new WorldClient instance.
        /// </summary>
        /// <param name="socket">Client socket</param>
        public WorldClient(Socket socket)
            : base(socket)
        {
            this.sessionId = (uint)Global.GenerateRandomNumber();
        }

        /// <summary>
        /// Disconnectes the current client.
        /// </summary>
        public void Disconnected()
        {
            Log.Info("Client with id {0} disconnected.", this.Id);

            this.Player?.Disconnect();
        }

        /// <summary>
        /// Send hi to the client.
        /// </summary>
        public override void Greetings()
        {
            using (var packet = new FFPacket())
            {
                packet.Write(0);
                packet.Write((int)this.sessionId);

                this.Send(packet);
            }
        }

        /// <summary>
        /// Handle incoming packets.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public override void HandleMessage(NetPacketBase packet)
        {
            packet.Position = 17;

            var packetHeaderNumber = packet.Read<uint>();
            var packetHeader = (PacketType)packetHeaderNumber;

            if (!FFPacketHandler.Invoke(this, packetHeader, packet))
                FFPacket.UnknowPacket<PacketType>(packetHeaderNumber, 8);
        }
    }
}
