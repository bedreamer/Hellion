using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hellion.Core.Data;
using Hellion.Core.Structures;
using Hellion.Core.IO;
using Hellion.Core.Network;
using Hellion.Core.Data.Headers;
using Hellion.World.Systems;
using Hellion.Core.Helpers;

namespace Hellion.World.Structures
{
    public abstract class Mover : WorldObject
    {
        private long nextMove;
        private long lastMoveTime;

        /// <summary>
        /// Get or sets the mover level.
        /// </summary>
        public virtual int Level { get; protected set; }

        /// <summary>
        /// Gets or sets the mover name.
        /// </summary>
        public virtual string Name { get; set; }

        public bool IsDead { get; set; }

        public bool IsFlying { get; set; }

        public bool IsFighting { get; set; }

        public bool IsFollowing { get; set; }

        public bool IsReseting { get; set; }

        public bool IsMovingWithKeyboard { get; set; }

        /// <summary>
        /// Gets the mover speed.
        /// </summary>
        public float Speed
        {
            get
            {
                float moverSpeed = WorldServer.MoversData[this.ModelId].Speed;

                return moverSpeed * this.SpeedFactor;
            }
        }
        
        /// <summary>
        /// Gets or sets the mover speed factor.
        /// </summary>
        public float SpeedFactor { get; set; }

        /// <summary>
        /// Gets the mover flight speed.
        /// </summary>
        public virtual float FlightSpeed { get; }

        public ObjectState MovingFlags { get; set; }

        public StateFlags MotionFlags { get; set; }

        public int ActionFlags { get; set; }

        public Mover TargetMover { get; private set; }

        public float FollowDistance { get; set; }

        public Vector3 DestinationPosition { get; set; }
        
        /// <summary>
        /// Gets the mover's attributes.
        /// </summary>
        public Attributes Attributes { get; private set; }

        public override WorldObjectType Type
        {
            get { return WorldObjectType.Mover; }
        }

        public Mover(int modelId)
            : base(modelId)
        {
            this.nextMove = Time.GetTick() + 10;
            this.lastMoveTime = Time.GetTick();
            this.Level = 1;
            this.DestinationPosition = new Vector3();
            this.TargetMover = null;
            this.FollowDistance = 1f;
            this.SpeedFactor = 1f;
            this.MovingFlags = ObjectState.OBJSTA_STAND;

            this.Attributes = new Attributes();
        }

        public void Target(Mover mover)
        {
            Log.Debug("{0} is targeting {1}", this.Name, mover.Name);
            this.TargetMover = mover;
        }

        public void RemoveTarget()
        {
            this.TargetMover = null;
        }

        public virtual void Update()
        {
            this.ProcessMoves();
        }


        private long timeDelta;
        private void ProcessMoves()
        {
            timeDelta = Time.GetTick() - this.lastMoveTime;
            this.lastMoveTime = Time.GetTick();

            if (this.DestinationPosition.IsZero())
                return;

            if (this.nextMove > Time.GetTick())
                return;

            this.nextMove = Time.GetTick() + 10;

            if (this.IsFollowing)
                this.Follow();

            if (this.IsFlying)
                this.Fly();
            else
                this.Walk();
        }

        private void Fly()
        {
            if (this.FlightSpeed > 0 && this.MovingFlags.HasFlag(ObjectState.OBJSTA_FMOVE))
            {
                Vector3 distance = this.DestinationPosition - this.Position;
                Vector3 moveVector = this.Position.Clone();
                float angle = Vector3.AngleBetween(this.Position, this.DestinationPosition);
                float angleFly = this.AngleFly;
                float angleTheta = MathHelper.ToRadian(angle);
                float angleFlyTheta = MathHelper.ToRadian(angleFly);
                float turnAngle = 0f;
                float accelPower = 0f;


                switch (this.MovingFlags & ObjectState.OBJSTA_MOVE_ALL)
                {
                    case ObjectState.OBJSTA_STAND:
                        accelPower = 0f;
                        break;
                    case ObjectState.OBJSTA_FMOVE:
                        accelPower = this.FlightSpeed;
                        break;
                }

                switch (this.MovingFlags & ObjectState.OBJSTA_TURN_ALL)
                {
                    case ObjectState.OBJSTA_RTURN:
                        turnAngle = this.TurnAngle;
                        if (this.MotionFlags.HasFlag(StateFlags.OBJSTAF_ACC))
                            turnAngle *= 2.5f;
                        angle += turnAngle;
                        if (angle < 0.0f)
                            angle += 360.0f;
                        break;
                    case ObjectState.OBJSTA_LTURN:
                        turnAngle = this.TurnAngle;
                        if (this.MotionFlags.HasFlag(StateFlags.OBJSTAF_ACC))
                            turnAngle *= 2.5f;
                        angle += turnAngle;
                        if (angle > 360.0f)
                            angle -= 360.0f;
                        break;
                }

                switch (this.MovingFlags & ObjectState.OBJSTA_LOOK_ALL)
                {
                    case ObjectState.OBJSTA_LOOKUP:
                        if (angleFly > 45f)
                            angleFly -= 1f;
                        break;
                    case ObjectState.OBJSTA_LOOKDOWN:
                        if (angleFly < 45f)
                            angleFly += 1f;
                        break;
                }

                if (this.MotionFlags.HasFlag(StateFlags.OBJSTAF_TURBO))
                    accelPower *= 1.5f;

                float d = (float)Math.Cos(angleFlyTheta) * accelPower;

                var deltaVector = new Vector3();
                var accelVector = new Vector3()
                {
                    X = (float)Math.Sin(angleTheta) * d,
                    Y = (float)-Math.Sin(angleFlyTheta) * accelPower,
                    Z = (float)-Math.Cos(angleTheta) * d
                };
                var accelVectorNorm = accelVector.Normalize();
                var deltaVectorNorm = deltaVector.Normalize();
                float deltaVectorLength = deltaVector.SquaredLength;
                float maxSpeed = 0.3f;

                if (this.MotionFlags.HasFlag(StateFlags.OBJSTAF_TURBO))
                    maxSpeed *= 1.1f;

                if (deltaVectorLength < (maxSpeed * maxSpeed))
                    deltaVector += accelVector;

                deltaVector *= (1.0f - 0.011f);

                if (this is Player)
                    moveVector += deltaVector;
                else
                {
                    // other ?
                }

                if (moveVector.Y > Map.MaxHeight)
                    moveVector.Y = Map.MaxHeight;

                this.Position = moveVector.Clone();
                this.AngleFly = angleFly;
                this.Angle = angle;
            }
        }
        
        private void Walk()
        {
            float speed = (this.Speed * 100f) * (timeDelta / 1000f);
            float distanceX = this.DestinationPosition.X - this.Position.X;
            float distanceZ = this.DestinationPosition.Z - this.Position.Z;
            float distance = (float)Math.Sqrt(distanceX * distanceX + distanceZ * distanceZ);

            if (this.Position.IsInCircle(this.DestinationPosition, 0.1f))
            {
                this.Position = this.DestinationPosition.Clone();
                this.OnArrival();
            }
            else
            {
                // Normalize
                float deltaX = distanceX / distance;
                float deltaZ = distanceZ / distance;

                float offsetX = deltaX * speed;
                float offsetZ = deltaZ * speed;

                if (Math.Abs(offsetX) > Math.Abs(distanceX))
                    offsetX = distanceX;
                if (Math.Abs(offsetZ) > Math.Abs(distanceZ))
                    offsetZ = distanceZ;

                this.Position.X += offsetX;
                this.Position.Z += offsetZ;
            }
        }

        private void Follow()
        {
            if (this.TargetMover != null)
            {
                this.DestinationPosition = this.TargetMover.Position.Clone();
                this.MovingFlags &= ~ObjectState.OBJSTA_STAND;
                this.MovingFlags |= ObjectState.OBJSTA_FMOVE;
            }
            else
            {
                //this.IsFollowing = false;
                //this.IsFighting = false;
                //this.RemoveTarget();
                //this.DestinationPosition.Reset();
                //this.MovingFlags = ObjectState.OBJSTA_STAND;
                //this.SendMoverAction((int)ObjectState.OBJSTA_STAND);
            }
        }


        // TODO: clean this mess up! :p

        private void move(float x, float z)
        {
            if (this.IsMovingWithKeyboard)
            {
                if (this.MovingFlags.HasFlag(ObjectState.OBJSTA_BMOVE))
                {
                    this.Position.X -= (float)(Math.Sin(this.Angle * (Math.PI / 180)) * Math.Sqrt(x * x + z * z));
                    this.Position.Z += (float)(Math.Cos(this.Angle * (Math.PI / 180)) * Math.Sqrt(x * x + z * z));
                }
                else if (this.MovingFlags.HasFlag(ObjectState.OBJSTA_FMOVE))
                {
                    this.Position.X += (float)(Math.Sin(this.Angle * (Math.PI / 180)) * Math.Sqrt(x * x + z * z));
                    this.Position.Z -= (float)(Math.Cos(this.Angle * (Math.PI / 180)) * Math.Sqrt(x * x + z * z));
                }
            }
            else
            {
                this.Position.X += x;
                this.Position.Z += z;
            }
        }

        public virtual void Fight(Mover defender) { }

        public virtual void OnArrival() { }

        public virtual void Die()
        {
            this.IsDead = true;

            this.RemoveTarget();
            this.SendDeath();
        }

        public virtual int GetDefense(Mover attacker, AttackFlags flags)
        {
            return 0;
        }

        public abstract int GetWeaponAttackDamages(int weaponType);

        // TODO: Move this packets to an other file.

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
