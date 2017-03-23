using Hellion.Core.Data.Resources;

namespace Hellion.Core.Structures
{
    public class JobData
    {
        public int Id { get; private set; }

        public float AttackSpeed { get; private set; }

        public float FactorMaxHp { get; private set; }

        public float FactorMaxMp { get; private set; }

        public float FactorMaxFp { get; private set; }

        public float FactorDef { get; private set; }

        public float FactorHpRecovery { get; private set; }

        public float FactorMpRecovery { get; private set; }

        public float FactorFpRecovery { get; private set; }

        public float MeleeSword { get; private set; }

        public float MeleeAxe { get; private set; }

        public float MeleeStaff { get; private set; }

        public float MeleeStick { get; private set; }

        public float MeleeKnuckle { get; private set; }

        public float MagicWand { get; private set; }

        public float Blocking { get; private set; }

        public float MeleeYoyo { get; private set; }

        public float Critical { get; private set; }

        public JobData(ResourceTable table)
        {
            this.Id = table.Get<int>("nJob");
            this.AttackSpeed = table.Get<float>("fAttackSpeed");
            this.FactorMaxHp = table.Get<float>("fFactorMaxHp");
            this.FactorMaxMp = table.Get<float>("fFactorMaxMp");
            this.FactorMaxFp = table.Get<float>("fFactorMaxFp");
            this.FactorDef = table.Get<float>("fFactorDef");
            this.FactorHpRecovery = table.Get<float>("fFactorHPRecovery");
            this.FactorMpRecovery = table.Get<float>("fFactorMPRecovery");
            this.FactorFpRecovery = table.Get<float>("fFactorFPRecovery");
            this.MeleeSword = table.Get<float>("fMeleeSWD");
            this.MeleeAxe = table.Get<float>("fMeleeAXE");
            this.MeleeStaff = table.Get<float>("fMeleeSTAFF");
            this.MeleeStick = table.Get<float>("fMeleeSTICK");
            this.MeleeKnuckle = table.Get<float>("fMeleeKNUCKLE");
            this.MagicWand = table.Get<float>("fMagicWand");
            this.Blocking = table.Get<float>("fBlocking");
            this.MeleeYoyo = table.Get<float>("fMeleeYOYO");
            this.Critical = table.Get<float>("fCritical");
        }
    }
}
