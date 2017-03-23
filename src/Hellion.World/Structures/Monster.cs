using Hellion.Core;
using Hellion.Core.Data.Headers;
using Hellion.Core.Helpers;
using Hellion.Core.IO;
using Hellion.Core.Structures;
using Hellion.World.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Structures
{
    public class Monster : Mover
    {
        private long moveTimer;
        private long attackTimer;
        private Region region;

        /// <summary>
        /// Gets the monster name.
        /// </summary>
        public override string Name
        {
            get { return this.Data.Name; }
            set { }
        }

        /// <summary>
        /// Gets the monster's data.
        /// </summary>
        public MoverData Data
        {
            get { return WorldServer.MoversData.ContainsKey(this.ModelId) ? WorldServer.MoversData[this.ModelId] : new MoverData(); }
        }

        /// <summary>
        /// Gets monster's level.
        /// </summary>
        public override int Level
        {
            get { return this.Data.Level; }
            protected set { base.Level = value; }
        }

        /// <summary>
        /// Gets the monster flight speed.
        /// </summary>
        public override float FlightSpeed
        {
            get { return this.Speed; }
        }

        /// <summary>
        /// Creates a new monster instance.
        /// </summary>
        /// <param name="modelId">Monster model id</param>
        /// <param name="mapId">Monster parent map id</param>
        public Monster(int modelId, int mapId)
            : this(modelId, mapId, null)
        {
        }

        /// <summary>
        /// Create a new monster instance.
        /// </summary>
        /// <param name="modelId">Monster model id</param>
        /// <param name="mapId">Monster map id</param>
        /// <param name="parentRegion">Monster parent region</param>
        public Monster(int modelId, int mapId, Region parentRegion)
            : base(modelId)
        {
            this.MapId = mapId;
            this.region = parentRegion;

            this.Attributes[DefineAttributes.HP] = this.Data.AddHp;
            this.Attributes[DefineAttributes.MP] = this.Data.AddMp;
            this.Attributes[DefineAttributes.STR] = this.Data.Str;
            this.Attributes[DefineAttributes.STA] = this.Data.Sta;
            this.Attributes[DefineAttributes.INT] = this.Data.Int;
            this.Attributes[DefineAttributes.DEX] = this.Data.Dex;
            this.Size = (short)(this.Data.Size + 100);

            this.Position = this.region.GetRandomPosition();
            this.DestinationPosition = this.Position.Clone();
            this.Angle = RandomHelper.Random(0, 360);
            this.moveTimer = Time.TimeInSeconds();
        }

        /// <summary>
        /// Update the monster.
        /// </summary>
        public override void Update()
        {
            if (this.IsFighting)
                this.ProcessFight();
            else
                this.ProcessMoves();

            base.Update();
        }

        /// <summary>
        /// Process the monster's moves
        /// </summary>
        private void ProcessMoves()
        {
            if (this.moveTimer <= Time.TimeInSeconds())
            {
                this.moveTimer = Time.TimeInSeconds() + RandomHelper.Random(15, 30);
                this.DestinationPosition = this.region.GetRandomPosition();
                this.Angle = Vector3.AngleBetween(this.Position, this.DestinationPosition);

                this.MovingFlags = ObjectState.OBJSTA_NONE;
                this.MovingFlags |= ObjectState.OBJSTA_FMOVE;
                this.SendMoverMoving();
            }
        }

        private void ProcessFight()
        {
            if (this.IsFighting && this.TargetMover != null)
            {
                if (this.SpeedFactor != 2)
                {
                    this.SpeedFactor = 2;
                    this.SendSpeed(this.SpeedFactor);
                }

                if (this.Position.IsInCircle(this.TargetMover.Position, 1f))
                {
                    if (this.attackTimer < Time.GetTick())
                        this.Fight(this.TargetMover);
                }
                else
                    this.SendFollowTarget(1f);
            }
            else if (this.TargetMover == null)
            {
                this.SpeedFactor = 1;
                this.SendSpeed(this.SpeedFactor);
                this.IsFighting = false;
                this.IsFollowing = false;
                this.DestinationPosition = this.region.GetRandomPosition();
                this.MovingFlags = ObjectState.OBJSTA_NONE;
                this.MovingFlags |= ObjectState.OBJSTA_FMOVE;
                this.SendMoverMoving();
            }
        }

        public override void Fight(Mover defender)
        {
            if (this.Position.IsInCircle(this.TargetMover.Position, 2)) // DEBUG arrived to target
            {
                Log.Debug("{0} is fighting {1}", this.Name, this.TargetMover.Name);
                // Reset attack delay
                this.attackTimer = Time.GetTick() + this.Data.ReAttackDelay;

                int motion = 29; // TODO: 28+attackType (IA)
                int damages = BattleManager.CalculateMeleeDamages(this, defender);

                this.SendMeleeAttack(motion, this.TargetMover.ObjectId);
            }
            else
            {
                Log.Debug("{0} following {1}", this.Name, this.TargetMover.Name);
                this.IsFollowing = true;
                this.SendFollowTarget(1);
            }

            base.Fight(defender);
        }

        public override int GetWeaponAttackDamages(int weaponType)
        {
            return 0;
        }

        public override int GetDefense(Mover attacker, AttackFlags flags)
        {
            float armor = this.Data.NaturalArmor;

            if (flags.HasFlag(AttackFlags.AF_MAGIC))
                armor = this.Data.ResistMagic;

            return (int)(armor / 7f + 1f);
        }
    }
}
