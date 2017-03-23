using Hellion.Core.Data.Headers;
using Hellion.Core.Helpers;
using Hellion.Core.IO;
using Hellion.Core.Structures;
using Hellion.World.Structures;
using Hellion.World.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Managers
{
    /// <summary>
    /// This <see cref="BattleManager"/> handles everything realated with the battles
    /// between players and monsters.
    /// It has some help methods used to calculate the damages and defense rate.
    /// </summary>
    public static class BattleManager
    {
        public static int CalculateMeleeDamages(Mover attacker, Mover defender)
        {
            int baseDamages = 0;

            if (attacker is Player)
            {
                var player = attacker as Player;
                Item rightWeapon = player.Inventory.GetRightWeapon(); // right weapon first.
                int weaponType = rightWeapon.Data.WeaponType;
                baseDamages = attacker.GetWeaponAttackDamages(weaponType);

                int weaponMinAbility = rightWeapon.Data.AbilityMin * 2;
                int weaponMaxAbility = rightWeapon.Data.AbilityMax * 2;

                int weaponDamage = RandomHelper.Random(weaponMinAbility, weaponMaxAbility);
                int refineDamage = (weaponDamage * (int)GetWeaponRefineMultiplier(rightWeapon.Refine)) + (int)Math.Pow(rightWeapon.Refine, 1.5);

                baseDamages += weaponDamage + refineDamage;

                // If player is assassin Then
                // calculate damage for left weapon if he has one
            }
            else
            {
                var monster = attacker as Monster;

                baseDamages = RandomHelper.Random(monster.Data.AtkMin, monster.Data.AtkMax);
            }

            if (baseDamages < 0)
                baseDamages = 0;

            return baseDamages;
        }

        public static AttackFlags GetAttackFlags(Mover attacker, Mover defender)
        {
            AttackFlags flags = 0;

            if (IsMissedHit(attacker, defender))
                return AttackFlags.AF_MISS;
            else
                flags |= AttackFlags.AF_GENERIC;

            if (IsCriticalHit(attacker, defender))
            {
                flags |= AttackFlags.AF_CRITICAL;

                float chance = RandomHelper.Random(0, 100);

                if (chance < 30)
                    flags |= AttackFlags.AF_FLYING;
            }
            if (IsBlockingHit(attacker, defender))
                flags |= AttackFlags.AF_BLOCKING;

            return flags;
        }

        public static void Process(Mover attacker, Mover defender)
        {
            AttackFlags flags = GetAttackFlags(attacker, defender);
            int damages = CalculateMeleeDamages(attacker, defender);

            if (flags.HasFlag(AttackFlags.AF_BLOCKING))
            {
                float blockFactor = GetBlockFactor(attacker, defender);

                if (blockFactor < 1f)
                    damages = (int)(damages * blockFactor);

                Log.Debug("=> {0} blocking {1}", defender.Name, attacker.Name);
            }
            if (flags.HasFlag(AttackFlags.AF_CRITICAL))
            {
                damages *= 2; // TODO: GetCriticalHitFactor

                Log.Debug("=> {0} inflicts a critical hit to {1}", attacker.Name, defender.Name);
            }
            
            damages -= GetDefense(attacker, defender, flags);

            if (flags == AttackFlags.AF_MISS)
            {
                Log.Debug("{0} misses {1}", attacker.Name, defender.Name);
                damages = 0;
            }

            if (damages < 0)
                damages = 0;

            Log.Debug("{0} inflicted {1} damages to {2}", attacker.Name, damages, defender.Name);

            // Send damages
            if (flags.HasFlag(AttackFlags.AF_FLYING)) // KnockBack
                SendDamagesFly(attacker, defender, damages, flags);
            else
                attacker.SendDamagesTo(defender, damages, flags);

            defender.Attributes[DefineAttributes.HP] -= damages;

            if (defender.Attributes[DefineAttributes.HP] <= 0)
                defender.Die();

            if (defender.IsDead)
            {
                if (defender is Monster && attacker is Player)
                {
                }

                if (defender is Player && attacker is Monster)
                {
                }
            }
        }

        private static double GetWeaponRefineMultiplier(byte weaponRefine)
        {
            try
            {
                return (Item.RefineTable[Math.Min(10, Math.Max(0, (int)weaponRefine))] + 100) * 0.01;
            }
            catch
            {
                return 1;
            }
        }

        private static bool IsMissedHit(Mover attacker, Mover defender)
        {
            float chance = RandomHelper.FloatRandom(0f, 100f);
            float hitRate = 0f;

            if (attacker is Player && defender is Monster)
            {
                var player = attacker as Player;
                var monster = defender as Monster;
                var playerDex = player.Attributes[DefineAttributes.DEX]; // TODO: add dex bonus

                hitRate = (int)(((playerDex * 1.6f) / (float)(playerDex + monster.Data.ER)) * 1.5f *
                   (player.Level * 1.2f / (float)(player.Level + monster.Level)) * 100.0f);
            }
            else if (attacker is Monster && defender is Player)
            {
                var monster = attacker as Monster;
                var player = defender as Player;
                var monsterDex = monster.Data.HR;

                hitRate = (int)(((monsterDex * 1.5f) / (float)(monsterDex + player.Level)) * 2.0f *
                    (monster.Level * 0.5f / (float)(monster.Level + player.Level * 0.3f)) * 100.0f);
            }

            if (hitRate > 96)
                hitRate = 96;
            else if (hitRate < 20)
                hitRate = 20;

            return hitRate < chance;
        }

        private static bool IsCriticalHit(Mover attacker, Mover defender)
        {
            float chance = RandomHelper.FloatRandom(0f, 100f);
            float jobFactor = 1f;

            if (attacker is Player)
                jobFactor = (attacker as Player).Class.Data.Critical;

            float critProb = attacker.Attributes[DefineAttributes.DEX] / 10;
            critProb *= jobFactor;

            if (critProb < 1f)
                critProb = 1f;

            return chance < critProb;
        }

        private static bool IsBlockingHit(Mover attacker, Mover defender)
        {
            float chance = RandomHelper.FloatRandom(0f, 100f);
            float blockRate = 0f;

            if (chance <= 5f)
                return false;
            else if (chance >= 95f)
                return true;

            if (defender is Monster)
            {
                var monster = defender as Monster;

                blockRate = (monster.Data.ER - monster.Level) * 0.5f;

                if (blockRate < 0)
                    blockRate = 0;
            }

            if (defender is Player)
            {
                var player = defender as Player;
                Item shield = player.Inventory.GetEquipedItemAt(InventoryParts.PARTS_SHIELD);

                if (shield == null)
                    blockRate = 0f;
                else
                {
                    blockRate = (player.Attributes[DefineAttributes.DEX] - 30) / (player.Level * 2);
                    blockRate += shield.Data.BlockRating;

                    if (blockRate < 2f)
                        blockRate = 2f;
                    if (blockRate > 65f)
                        blockRate = 65f;
                }
            }

            return blockRate > chance;
        }

        private static float GetCriticalHitFactor(Mover attacker, Mover defender)
        {
            float minCrit = 1.1f;
            float maxCrit = 1.4f;
            float criticalBonus = 1f;

            if (attacker.Level > defender.Level)
            {
                if (defender is Monster)
                {
                    minCrit = 1.2f;
                    maxCrit = 2.0f;
                }

                if (defender is Player)
                {
                    minCrit = 1.4f;
                    maxCrit = 1.8f;
                }
            }

            // TODO: critical bonus
            //criticalBonus += attacker.Attributes[DefineAttributes.CRITICAL_BONUS] / 100f;

            //if (criticalBonus < 0.1f)
            //    criticalBonus = 0.1f;

            return RandomHelper.FloatRandom(minCrit, maxCrit);
        }

        private static float GetBlockFactor(Mover attacker, Mover defender)
        {
            if (defender is Player)
            {
                var player = defender as Player;
                int defenderDex = defender.Attributes[DefineAttributes.DEX];
                int attackerDex = attacker.Attributes[DefineAttributes.DEX];
                float chance = RandomHelper.FloatRandom(0f, 80f);

                if (chance <= 5)
                    return 1.0f;
                if (chance >= 75)
                    return 0.1f;

                float blockFactor1 = defender.Level / ((defender.Level + attacker.Level) * 15.0f);
                float blockFactor2 = (defenderDex + attackerDex + 2) * ((defenderDex - attackerDex) / 800.0f);
                if (blockFactor2 > 10.0f)
                    blockFactor2 = 10.0f;
                float blockFactor = blockFactor1 + blockFactor2;
                if (blockFactor < 0.0f)
                    blockFactor = 0.0f;

                int blockRate = (int)((defenderDex / 8.0f) * player.Class.Data.Blocking + blockFactor);
                if (blockRate < 0)
                    blockRate = 0;

                if (blockRate > chance)
                    return 0.0f;
            }
            else if (defender is Monster)
            {
                var monster = defender as Monster;
                float chance = RandomHelper.FloatRandom(0f, 100f);

                if (chance <= 5)
                    return 1.0f;
                if (chance >= 95)
                    return 0.1f;

                int blockRate = (int)((monster.Data.ER - monster.Level) * 0.5f);
                if (blockRate < 0)
                    blockRate = 0;

                if (blockRate > chance)
                    return 0.2f;
            }

            return 1f;
        }

        private static int GetDefense(Mover attacker, Mover defender, AttackFlags flags)
        {
            if (flags.HasFlag(AttackFlags.AF_MAGICSKILL))
            {
                // return Magic resist
                return 0;
            }

            if ((attacker is Player && defender is Player) || flags.HasFlag(AttackFlags.AF_GENERIC))
            {
            }

            return defender.GetDefense(attacker, flags);
        }

        private static void SendDamagesFly(Mover attacker, Mover defender, int damages, AttackFlags flags)
        {
            var delta = new Vector3();
            float angle = MathHelper.ToRadian(defender.Angle);
            float angleY = MathHelper.ToRadian(145f);

            delta.Y = (float)(-Math.Cos(angleY) * 0.18f);
            float dist = (float)(Math.Sin(angleY) * 0.18f);
            delta.X = (float)(Math.Sin(angle) * dist);
            delta.Z = (float)(-Math.Cos(angle) * dist);

            defender.DestinationPosition.X += delta.X;
            defender.DestinationPosition.Z += delta.Z;
            attacker.SendDamagesTo(defender, damages, flags, defender.Position, angle);
        }
    }
}
