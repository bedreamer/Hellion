using Hellion.World.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Managers
{
    public static class FormulasManager
    {
        public static int GetHpRecovery(Mover mover)
        {
            float factor = 1f;

            if (mover is Player)
                factor = (mover as Player).Class.Data.FactorHpRecovery;

            int recoveryValue = (int)((mover.Level / 3.0f) + (mover.MaxHp / (500f * mover.Level)) + (mover.Stamina * factor));

            recoveryValue = (int)(recoveryValue - (recoveryValue * 0.1f));

            return recoveryValue;
        }

        public static int GetMpRecovery(Mover mover)
        {
            float factor = 1f;

            if (mover is Player)
                factor = (mover as Player).Class.Data.FactorMpRecovery;

            int recoveryValue = (int)(((mover.Level * 1.5f) + (mover.MaxMp / (500f * mover.Level)) + (mover.Intelligence * factor)) * 0.2f);

            recoveryValue = (int)(recoveryValue - (recoveryValue * 0.1f));

            return recoveryValue;
        }

        public static int GetFpRecovery(Mover mover)
        {
            float factor = 1f;

            if (mover is Player)
                factor = (mover as Player).Class.Data.FactorFpRecovery;

            int recoveryValue = (int)(((mover.Level * 2.0f) + (mover.MaxFp / (500f * mover.Level)) + (mover.Stamina * factor)) * 0.2f);
            recoveryValue = (int)(recoveryValue - (recoveryValue * 0.1f));

            return recoveryValue;
        }
    }
}
