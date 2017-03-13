using Hellion.Core.Data.Headers;
using Hellion.Core.Helpers;
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

                baseDamages += weaponDamage;

                // If player is assassin Then
                // calculate damage for left weapon if he has one
            }
            else
            {
                // Monster
            }

            if (baseDamages < 0)
                baseDamages = 0;

            return baseDamages;
        }
    }
}
