using Hellion.Core.Data.Headers;
using Hellion.Core.Network;
using Hellion.Core.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Systems
{
    public partial class Mover
    {
        internal void SendMoverMoving()
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.DESTPOS);
                packet.Write(this.DestinationPosition.X);
                packet.Write(this.DestinationPosition.Y);
                packet.Write(this.DestinationPosition.Z);
                packet.Write<byte>(1);

                this.SendToVisible(packet);
            }
        }

        internal void SendMoverPosition()
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.SETPOS);
                packet.Write(this.Position.X);
                packet.Write(this.Position.Y);
                packet.Write(this.Position.Z);

                this.SendToVisible(packet);
            }
        }

        public void SendMoverAction(int motionId)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.MOTION);
                packet.Write(motionId);

                this.SendToVisible(packet);
            }
        }

        internal void SendFollowTarget(float distance)
        {
            if (this.TargetMover == null)
                return;

            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.MOVERSETDESTOBJ);
                packet.Write(this.TargetMover.ObjectId);
                packet.Write(distance);

                base.SendToVisible(packet);
            }
        }

        private void SendNormalChat(string message, Player toPlayer = null)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.CHAT);
                packet.Write(message);

                if (toPlayer == null)
                    this.SendToVisible(packet);
                else
                    toPlayer.Send(packet);
            }
        }

        internal void SendNormalChat(string message)
        {
            this.SendNormalChat(message, null);
        }

        internal void SendNormalChatTo(string message, Player player)
        {
            this.SendNormalChat(message, player);
        }

        internal void SendMeleeAttack(int motion, int targetId)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.MELEE_ATTACK);
                packet.Write(motion);
                packet.Write(targetId);
                packet.Write(0);
                packet.Write(0x10000);

                this.SendToVisible(packet);
            }
        }

        internal void SendSpeed(float speedFactor)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.SET_SPEED_FACTOR);
                packet.Write(speedFactor);

                this.SendToVisible(packet);
            }
        }

        internal void SendDamagesTo(Mover defender, int damages, AttackFlags flags, Vector3 position = null, float angle = 0f)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(defender.ObjectId, SnapshotType.DAMAGE);
                packet.Write(this.ObjectId);
                packet.Write(damages);
                packet.Write((int)flags);

                if (flags.HasFlag(AttackFlags.AF_FLYING))
                {
                    packet.Write(position.X);
                    packet.Write(position.Y);
                    packet.Write(position.Z);
                    packet.Write(angle * 10f);
                }

                this.SendToVisible(packet);
            }
        }

        internal void SendDeath()
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.MOVERDEATH);

                packet.Write<long>(0);
                this.SendToVisible(packet);
            }
        }
    }
}
