using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hellion.Core.Data;
using Hellion.Core.Structures;
using Hellion.Core.IO;
using Hellion.Core.Network;
using Hellion.Core.Data.Headers;
using Hellion.Core.Helpers;
using Hellion.World.Structures;

namespace Hellion.World.Systems
{
    public abstract partial class Mover : WorldObject
    {
        private long nextMove;
        private long lastMoveTime;
        private long timeDelta;

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

        public int Strength
        {
            get { return this.GetAttribute(DefineAttributes.STR); }
        }

        public int Stamina
        {
            get { return this.GetAttribute(DefineAttributes.STA); }
        }

        public int Dexterity
        {
            get { return this.GetAttribute(DefineAttributes.DEX); }
        }

        public int Intelligence
        {
            get { return this.GetAttribute(DefineAttributes.INT); }
        }

        public virtual int MaxHp
        {
            get { return 0; }
        }

        public virtual int MaxMp
        {
            get { return 0; }
        }

        public virtual int MaxFp
        {
            get { return 0; }
        }

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

        /// <summary>
        /// Gets the mover's bonus attributes.
        /// </summary>
        public Attributes BonusAttributes { get; private set; }

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
            this.BonusAttributes = new Attributes();
        }

        public void Target(Mover mover)
        {
            Log.Debug("{0} is targeting {1}", this.Name, mover.Name);
            this.TargetMover = mover;
        }

        public void RemoveTarget()
        {
            this.IsFighting = false;
            this.TargetMover = null;
        }

        public virtual void Update()
        {
            this.ProcessMoves();
        }


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
            if (this.Position.IsInCircle(this.DestinationPosition, 0.1f))
            {
                this.Position = this.DestinationPosition.Clone();
                this.OnArrival();
            }
            else
            {
                float speed = (this.Speed * 100f) * (timeDelta / 1000f);
                float distanceX = this.DestinationPosition.X - this.Position.X;
                float distanceZ = this.DestinationPosition.Z - this.Position.Z;
                float distance = (float)Math.Sqrt(distanceX * distanceX + distanceZ * distanceZ);

                // Normalize
                float deltaX = distanceX / distance;
                float deltaZ = distanceZ / distance;
                float offsetX = deltaX * speed;
                float offsetZ = deltaZ * speed;

                if (Math.Abs(offsetX) > Math.Abs(distanceX))
                    offsetX = distanceX;
                if (Math.Abs(offsetZ) > Math.Abs(distanceZ))
                    offsetZ = distanceZ;

                if (this.IsMovingWithKeyboard)
                {
                    float offset = (float)Math.Sqrt(offsetX * offsetX + offsetZ * offsetZ);

                    if (this.MovingFlags.HasFlag(ObjectState.OBJSTA_BMOVE))
                    {
                        this.Position.X -= (float)(Math.Sin(this.Angle * (Math.PI / 180)) * offset);
                        this.Position.Z += (float)(Math.Cos(this.Angle * (Math.PI / 180)) * offset);
                    }
                    else if (this.MovingFlags.HasFlag(ObjectState.OBJSTA_FMOVE))
                    {
                        this.Position.X += (float)(Math.Sin(this.Angle * (Math.PI / 180)) * offset);
                        this.Position.Z -= (float)(Math.Cos(this.Angle * (Math.PI / 180)) * offset);
                    }
                }
                else
                {
                    this.Position.X += offsetX;
                    this.Position.Z += offsetZ;
                }
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

        private int GetAttribute(DefineAttributes attribute, bool includeBonus = true)
        {
            int value = this.Attributes[attribute];

            if (includeBonus)
                value += this.BonusAttributes[attribute];

            if (value < 1)
                value = 1;

            return value;
        }


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

        public abstract void Fight(Mover defender);
        public abstract int GetWeaponAttackDamages(int weaponType);
    }
}
