using Ether.Network;
using System.Net.Sockets;
using Ether.Network.Packets;
using Hellion.Core.ISC.Structures;
using Hellion.Core.Data.Headers;
using Hellion.Core.IO;

namespace Hellion.Login.ISC
{
    public partial class InterClient : NetConnection
    {
        /// <summary>
        /// Gets the server informations
        /// </summary>
        public BaseServer ServerInfo { get; private set; }

        /// <summary>
        /// Gets the server type.
        /// </summary>
        public InterServerType ServerType { get; private set; }

        /// <summary>
        /// Gets or sets the InterServer reference.
        /// </summary>
        public InterServer Server { get; internal set; }

        /// <summary>
        /// Creates a new InterClient instance.
        /// </summary>
        public InterClient()
            : base()
        {
        }

        /// <summary>
        /// Creates a new InterClient instance.
        /// </summary>
        /// <param name="acceptedSocket"></param>
        public InterClient(Socket acceptedSocket) 
            : base(acceptedSocket)
        {
            
        }

        /// <summary>
        /// Send a welcome message to the client.
        /// </summary>
        public override void Greetings()
        {
            using (var packet = new NetPacket())
            {
                packet.Write((int)InterHeaders.CanAuthticate);

                this.Send(packet);
            }
        }

        /// <summary>
        /// Handles the incoming messages.
        /// </summary>
        /// <param name="packet"></param>
        public override void HandleMessage(NetPacketBase packet)
        {
            var packetHeaderNumber = packet.Read<int>();
            var packetHeader = (InterHeaders)packetHeaderNumber;

            switch (packetHeader)
            {
                case InterHeaders.Authentication: this.OnAuthenticate(packet); break;

                default:
                    Log.Warning("Unknow packet: 0x{0}", packetHeaderNumber.ToString("X2"));
                    break;
            }
        }

        /// <summary>
        /// Print disconnected log.
        /// </summary>
        internal void Disconnected()
        {
            if (this.ServerInfo is ClusterServerInfo)
            {
                var clusterInfo = this.ServerInfo as ClusterServerInfo;

                Log.Info("ClusterServer '{0}' disconnected.", clusterInfo.Name);
            }
            else if (this.ServerInfo is WorldServerInfo)
            {
                var worldInfo = this.ServerInfo as WorldServerInfo;

                Log.Info("WorldServer '{0}' disconnected.", worldInfo.Name);
            }
            else
                Log.Info("Unknow client disconnected.");
        }
    }
}
