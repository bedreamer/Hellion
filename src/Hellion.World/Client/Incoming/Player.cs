using Ether.Network.Packets;
using Hellion.Core.Data.Headers;
using Hellion.Core.IO;
using Hellion.Core.Network;
using Hellion.Core.Structures;
using Hellion.Database;
using Hellion.Database.Structures;
using Hellion.World.Structures;
using Hellion.World.Systems;

/*
 * This file contains only the incoming packets realated with the Player.
 * Packets as the moves, duels, chat, etc...
 */

namespace Hellion.World.Client
{
    public partial class WorldClient
    {
        /// <summary>
        /// The client sended the Join request to join the world as a player.
        /// </summary>
        /// <param name="packet"></param>
        [FFIncomingPacket(PacketType.JOIN)]
        private void OnJoin(NetPacketBase packet)
        {
            var worldId = packet.Read<int>();
            var playerId = packet.Read<int>();
            var authKey = packet.Read<int>();
            var partyId = packet.Read<int>();
            var guildId = packet.Read<int>();
            var guildWarId = packet.Read<int>();
            var idOfMulti = packet.Read<int>(); // what is this?
            var slot = packet.Read<byte>();
            var playerName = packet.Read<string>();
            var username = packet.Read<string>();
            var password = packet.Read<string>();
            var messengerState = packet.Read<int>();
            var messengerCount = packet.Read<int>();
            // Not using messenger yet

            this.CurrentUser = DatabaseService.Users.Get(x =>
                x.Username.ToLower() == username.ToLower() &&
                x.Password.ToLower() == password.ToLower() &&
                x.Authority > 0);

            if (this.CurrentUser == null)
            {
                Log.Warning("Unknow account: '{0}'.", username);
                this.Server.RemoveClient(this);
                return;
            }

            DbCharacter character = DatabaseService.Characters.Get(x =>
                x.AccountId == this.CurrentUser.Id &&
                x.Name.ToLower() == playerName.ToLower() &&
                x.Id == playerId, includes => includes.Items); // TODO: include more

            if (character == null)
            {
                Log.Warning("Cannot find character '{0}' with id {1} for account '{2}'.", playerName, playerId, this.CurrentUser.Id);
                this.Server.RemoveClient(this);
                return;
            }

            this.Player = new Player(this, character);
            this.Player.SendPlayerSpawn();

            Map playerMap = WorldServer.MapManager[this.Player.MapId];

            if (playerMap == null)
            {
                Log.Error("Invalid MapId: {0}", this.Player.MapId);
                this.Server.RemoveClient(this);
                return;
            }

            playerMap.AddObject(this.Player);
        }

        /// <summary>
        /// The client has send a snapshot request.
        /// Which means that this is a packet containing more than one packet.
        /// </summary>
        /// <param name="packet"></param>
        [FFIncomingPacket(PacketType.SNAPSHOT)]
        private void OnSnapshot(NetPacketBase packet)
        {
            var snapshotCount = packet.Read<byte>();

            while (snapshotCount != 0)
            {
                var snapshotHeaderNumber = packet.Read<short>();
                var snapshotHeader = (SnapshotType)snapshotHeaderNumber;

                Log.Debug("Recieve snapshot: {0}", snapshotHeader);

                switch (snapshotHeader)
                {
                    case SnapshotType.DESTPOS: this.OnSnapshotDestPos(packet); break;
                    default: FFPacket.UnknowPacket<SnapshotType>((uint)snapshotHeaderNumber, 4); break;
                };

                snapshotCount--;
            }
        }

        /// <summary>
        /// Process the player moves.
        /// </summary>
        /// <param name="packet"></param>
        private void OnSnapshotDestPos(NetPacketBase packet)
        {
            var posX = packet.Read<float>();
            var posY = packet.Read<float>();
            var posZ = packet.Read<float>();
            var forward = packet.Read<byte>();

            this.Player.RemoveTarget();
            this.Player.IsFollowing = false;
            this.Player.IsMovingWithKeyboard = false;
            this.Player.MovingFlags = ObjectState.OBJSTA_NONE;
            this.Player.MovingFlags |= ObjectState.OBJSTA_FMOVE;
            this.Player.DestinationPosition = new Vector3(posX, posY, posZ);
            this.Player.SendMoverMoving();
        }

        /// <summary>
        /// The client is moving with the keyboard.
        /// </summary>
        /// <param name="packet"></param>
        [FFIncomingPacket(PacketType.PLAYERMOVED)]
        private void OnMoveByKeyboard(NetPacketBase packet)
        {
            this.Player.IsFollowing = false;

            var startPositionX = packet.Read<float>();
            var startPositionY = packet.Read<float>();
            var startPositionZ = packet.Read<float>();
            var startPosition = new Vector3(startPositionX, startPositionY, startPositionZ);

            var directionX = packet.Read<float>();
            var directionY = packet.Read<float>();
            var directionZ = packet.Read<float>();
            var directionVector = new Vector3(directionX, directionY, directionZ);
            
            this.Player.Position = startPosition.Clone();
            this.Player.Position += directionVector;
            this.Player.Angle = packet.Read<float>();
            this.Player.MovingFlags = (ObjectState)packet.Read<uint>();
            this.Player.MotionFlags = (StateFlags)packet.Read<int>();
            this.Player.ActionFlags = packet.Read<int>();
            var motionEx = packet.Read<int>();
            var loop = packet.Read<int>();
            var motionOption = packet.Read<int>();
            var tick = packet.Read<long>();
            
            this.Player.DestinationPosition.Reset();
            
            if (this.Player.MovingFlags.HasFlag(ObjectState.OBJSTA_FMOVE) || 
                this.Player.MovingFlags.HasFlag(ObjectState.OBJSTA_BMOVE))
                this.Player.IsMovingWithKeyboard = true;
            else
                this.Player.IsMovingWithKeyboard = false;

            this.Player.DestinationPosition = this.Player.Position.Clone();
            
            this.Player.SendMoveByKeyboard(directionVector, motionEx, loop, motionOption, tick);
        }
        
        [FFIncomingPacket(PacketType.PLAYERMOVED2)]
        private void OnPlayerMoved2(NetPacketBase packet)
        {
            var startPositionX = packet.Read<float>();
            var startPositionY = packet.Read<float>();
            var startPositionZ = packet.Read<float>();
            var startPosition = new Vector3(startPositionX, startPositionY, startPositionZ);

            var directionX = packet.Read<float>();
            var directionY = packet.Read<float>();
            var directionZ = packet.Read<float>();
            var directionVector = new Vector3(directionX, directionY, directionZ);

            if (directionVector.IsZero())
                this.Player.DestinationPosition = startPosition.Clone();

            this.Player.Angle = packet.Read<float>();
            this.Player.AngleFly = packet.Read<float>();
            var flySpeed = packet.Read<float>();
            var turnAngle = packet.Read<float>();
            this.Player.MovingFlags = (ObjectState)packet.Read<uint>();
            this.Player.MotionFlags = (StateFlags)packet.Read<int>();
            this.Player.ActionFlags = packet.Read<int>();
            var motionEx = packet.Read<int>();
            var loop = packet.Read<int>();
            var motionOption = packet.Read<int>();
            var tick = packet.Read<long>();
            var frame = packet.Read<byte>();

            this.Player.IsFlying = this.Player.MovingFlags.HasFlag(ObjectState.OBJSTA_FMOVE);

            this.Player.SendMoverMoved(directionVector, motionEx, loop, motionOption, tick, frame, turnAngle);
        }

        /// <summary>
        /// This client has send a behavior request.
        /// </summary>
        /// <param name="packet"></param>
        [FFIncomingPacket(PacketType.PLAYERBEHAVIOR)]
        private void OnPlayerBehavior(NetPacketBase packet)
        {
            if (this.Player.IsFlying)
                return;

            var startPositionX = packet.Read<float>();
            var startPositionY = packet.Read<float>();
            var startPositionZ = packet.Read<float>();
            var startPosition = new Vector3(startPositionX, startPositionY, startPositionZ);

            var directionX = packet.Read<float>();
            var directionY = packet.Read<float>();
            var directionZ = packet.Read<float>();
            var directionVector = new Vector3(directionX, directionY, directionZ);

            var angle = packet.Read<float>();
            this.Player.MovingFlags = (ObjectState)packet.Read<uint>();
            this.Player.MotionFlags = (StateFlags)packet.Read<int>();
            this.Player.ActionFlags = packet.Read<int>();
            var motionEx = packet.Read<int>();
            var loop = packet.Read<int>();
            var motionOption = packet.Read<int>();
            var tick = packet.Read<long>();

            this.Player.Position = startPosition.Clone();
            this.Player.Position += directionVector;
            this.Player.DestinationPosition.Reset();

            this.Player.IsMovingWithKeyboard = this.Player.MovingFlags.HasFlag(ObjectState.OBJSTA_FMOVE);

            this.Player.SendMoverBehavior(directionVector, motionEx, loop, motionOption, tick);
        }

        //[FFIncomingPacket(PacketType.PLAYERBEHAVIOR2)]
        private void OnPlayerBehavior2(NetPacketBase packet)
        {
        }

        [FFIncomingPacket(PacketType.PLAYERANGLE)]
        private void OnPlayerAngle(NetPacketBase packet)
        {
            var startPositionX = packet.Read<float>();
            var startPositionY = packet.Read<float>();
            var startPositionZ = packet.Read<float>();
            var startPosition = new Vector3(startPositionX, startPositionY, startPositionZ);

            var directionX = packet.Read<float>();
            var directionY = packet.Read<float>();
            var directionZ = packet.Read<float>();
            var directionVector = new Vector3(directionX, directionY, directionZ);

            var angle = packet.Read<float>();
            var angleY = packet.Read<float>();
            float flySpeed = packet.Read<float>();
            this.Player.TurnAngle = packet.Read<float>();
            var tick = packet.Read<long>();

            this.Player.Angle = angle;
            this.Player.AngleFly = angle;
            this.Player.Position = startPosition.Clone();

            if (directionVector.IsZero())
                this.Player.DestinationPosition = this.Player.Position.Clone();

            this.Player.SendMoverAngle(directionVector, tick, this.Player.TurnAngle);
        }

        [FFIncomingPacket(PacketType.PLAYERSETDESTOBJ)]
        public void OnPlayerSetFollowTarget(NetPacketBase packet)
        {
            var moverId = packet.Read<uint>();
            this.Player.FollowDistance = packet.Read<float>();

            if (moverId == this.Player.ObjectId)
            {
                this.Player.Target(this.Player);
                return;
            }

            var targetMover = this.Player.GetSpawnedObjectById<Mover>((int)moverId);

            if (targetMover == null)
            {
                Log.Error("[PLAYERSETDESTOBJ]: Cannot target mover ID: {0}", moverId);
                return;
            }

            this.Player.IsFollowing = true;
            this.Player.MovingFlags = ObjectState.OBJSTA_FMOVE;
            this.Player.Target(targetMover);
            this.Player.DestinationPosition = targetMover.Position.Clone();
            this.Player.SendFollowTarget(this.Player.FollowDistance);
        }

        [FFIncomingPacket(PacketType.QUERYGETDESTOBJ)]
        public void OnQueryGetDestObj(NetPacketBase packet)
        {
            var objectId = packet.Read<int>();
            var mover = this.Player.GetSpawnedObjectById<Mover>(objectId);

            if (mover != null)
            {
                Log.Debug("Mover {0} arrived to destination", mover.Name);
                mover.Position = mover.DestinationPosition.Clone();
            }
        }

        [FFIncomingPacket(PacketType.MELEE_ATTACK)]
        public void OnMeleeAttack(NetPacketBase packet)
        {
            var motion = packet.Read<int>();
            var targetId = packet.Read<int>();
            var param2 = packet.Read<int>(); // ??
            var param3 = packet.Read<int>(); // ??
            var value = packet.Read<float>(); // ??
            var target = this.Player.GetSpawnedObjectById<Mover>(targetId);

            if (target == null || this.Player.TargetMover == null || this.Player.TargetMover.ObjectId != targetId)
                return;
            
            if (target.IsDead)
                return;

            this.Player.Fight(target);
            this.Player.SendMeleeAttack(motion, targetId);
        }

        [FFIncomingPacket(PacketType.SETTARGET)]
        public void OnSetTarget(NetPacketBase packet)
        {
            var objectId = packet.Read<int>();
            var state = packet.Read<byte>();
            
            if (state == 2)
            {
                var targetMover = this.Player.GetSpawnedObjectById<Mover>(objectId);

                if (targetMover != null)
                    this.Player.Target(targetMover);
                else
                    Log.Warning("Target is not a mover");
            }
            else
                this.Player.RemoveTarget();
        }
    }
}
