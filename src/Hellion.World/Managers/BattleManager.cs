using Hellion.Core.Data.Headers;
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
            int damages = 0;

            if (attacker is Player)
            {
                var player = attacker as Player;
                int weaponType = player.Inventory.GetRightWeapon().Data.eItemType; // right weapon first.
                damages = attacker.GetWeaponAttackDamages(weaponType);

                // If player is assassin Then
                // calculate damage for left weapon if he has one
            }
            else
            {
                // Monster
            }

            if (damages < 0)
                damages = 0;

            return damages;
        }
    }
}
