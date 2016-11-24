﻿using Ether.Network;
using Hellion.Core.Database;
using Hellion.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Ether.Network.Packets;
using Hellion.Core;

namespace Hellion.World
{
    public class WorldClient : NetConnection
    {
        private uint sessionId;

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
        /// Send hi to the client.
        /// </summary>
        public override void Greetings()
        {
            base.Greetings();
        }

        /// <summary>
        /// Handle incoming packets.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public override void HandleMessage(NetPacketBase packet)
        {
            base.HandleMessage(packet);
        }
    }
}