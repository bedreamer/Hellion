using Hellion.Core.Data.Resources;

namespace Hellion.World.Structures
{
    public struct SkillLevelData
    {
        public int LevelID;
        public int SkillID;
        public int SLevel;
        public int AbilityMin;
        public int AbilityMax;
        public int AbilityMinPVP;
        public int AbilityMaxPVP;
        public int AttackSpeed;
        public int DmgShift;
        public int nProbability;
        public int nProbabilityPVP;
        public int Taunt;
        public int DestParam1;
        public int DestParam2;
        public int nAdjParamVal1;
        public int nAdjParamVal2;
        public int ChgParamVal1;
        public int ChgParamVal2;
        public int DestData1;
        public int DestData2;
        public int ActiveSkill;
        public int ActiveSkillRate;
        public int ActiveSkillRatePVP;
        public int ReqMp;
        public int ReqFp;
        public int Cooldown;
        public int CastingTime;
        public int SkillRange;
        public int CircleTime;
        public int PainTime;
        public uint SkillTime;
        public int SkillCount;
        public int SkillExp;
        public int Exp;
        public int ComboSkillTime;

        /// <summary>
        /// Initialize a new SkillLevel
        /// </summary>
        /// <param name="table">Data table</param>
        public SkillLevelData(ResourceTable table)
        {
            this.LevelID = table.Get<int>("dwLevelID");
            this.SkillID = table.Get<int>("dwSkillID");
            this.SLevel = table.Get<int>("dwSkillLevel");
            this.AbilityMin = table.Get<int>("dwAbilityMin");
            this.AbilityMax = table.Get<int>("dwAbilityMax");
            this.AbilityMinPVP = table.Get<int>("dwAbilityMinPVP");
            this.AbilityMaxPVP = table.Get<int>("dwAbilityMaxPVP");
            this.AttackSpeed = table.Get<int>("dwAttackSpeed");
            this.DmgShift = table.Get<int>("dwDmgShift");
            this.nProbability = table.Get<int>("nProbability");
            this.nProbabilityPVP = table.Get<int>("nProbabilityPVP");
            this.Taunt = table.Get<int>("dwTaunt");
            this.DestParam1 = table.Get<int>("dwDestParam1");
            this.DestParam2 = table.Get<int>("dwDestParam2");
            this.nAdjParamVal1 = table.Get<int>("nAdjParamVal1");
            this.nAdjParamVal2 = table.Get<int>("nAdjParamVal2");
            this.ChgParamVal1 = table.Get<int>("dwChgParamVal1");
            this.ChgParamVal2 = table.Get<int>("dwChgParamVal2");
            this.DestData1 = table.Get<int>("dwDestData1");
            this.DestData2 = table.Get<int>("dwDestData2");
            this.ActiveSkill = table.Get<int>("dwActiveSkill");
            this.ActiveSkillRate = table.Get<int>("dwActiveSkillRate");
            this.ActiveSkillRatePVP = table.Get<int>("dwActiveSkillRatePVP");
            this.ReqMp = table.Get<int>("dwReqMp");
            this.ReqFp = table.Get<int>("dwReqFp");
            this.Cooldown = table.Get<int>("dwCooldown");
            this.CastingTime = table.Get<int>("dwCastingTime");
            this.SkillRange = table.Get<int>("dwSkillRange");
            this.CircleTime = table.Get<int>("dwCircleTime");
            this.PainTime = table.Get<int>("dwPainTime");
            this.SkillTime = table.Get<uint>("dwSkillTime");
            this.SkillCount = table.Get<int>("dwSkillCount");
            this.SkillExp = table.Get<int>("dwSkillExp");
            this.Exp = table.Get<int>("dwExp");
            this.ComboSkillTime = table.Get<int>("dwComboSkillTime");
        }
    }
}
