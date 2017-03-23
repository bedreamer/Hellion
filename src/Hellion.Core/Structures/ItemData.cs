using Hellion.Core.Data.Resources;

namespace Hellion.Core.Structures
{
    /// <summary>
    /// Represents an Item data from the propItem.txt resource file located in dataSub2.res.
    /// </summary>
    public class ItemData
    {
        public int Version { get; private set; }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Num { get; private set; }
        public int PackMax { get; private set; }
        public int ItemKind1 { get; private set; }
        public int ItemKind2 { get; private set; }
        public int ItemKind3 { get; private set; }
        public int ItemJob { get; private set; }
        public bool Permanence { get; private set; }
        public int Useable { get; private set; }
        public int ItemSex { get; private set; }
        public int Cost { get; private set; }
        public int Endurance { get; private set; }
        public int nAbrasion { get; private set; }
        public int nMaxRepair { get; private set; }
        public int Handed { get; private set; }
        public int Flag { get; private set; }
        public int Parts { get; private set; }
        public int Partsub { get; private set; }
        public bool PartFile { get; private set; }
        public int Exclusive { get; private set; }
        public int BasePartsIgnore { get; private set; }
        public int ItemLV { get; private set; }
        public int ItemRare { get; private set; }
        public int ShopAble { get; private set; }
        public bool Log { get; private set; }
        public bool Charged { get; private set; }
        public int LinkKindBullet { get; private set; }
        public int LinkKind { get; private set; }
        public int AbilityMin { get; private set; }
        public int AbilityMax { get; private set; }
        public int eItemType { get; private set; }
        public int wItemEAtk { get; private set; }
        public int Parry { get; private set; }
        public int BlockRating { get; private set; }
        public int AddSkillMin { get; private set; }
        public int AddSkillMax { get; private set; }
        public int AtkStyle { get; private set; }
        public int WeaponType { get; private set; }
        public int ItemAtkOrder1 { get; private set; }
        public int ItemAtkOrder2 { get; private set; }
        public int ItemAtkOrder3 { get; private set; }
        public int ItemAtkOrder4 { get; private set; }
        public bool ContinnuousPain { get; private set; }
        public int ShellQuantity { get; private set; }
        public int Recoil { get; private set; }
        public int LoadingTime { get; private set; }
        public int AdjHitRate { get; private set; }
        public float AttackSpeed { get; private set; }
        public int DmgShift { get; private set; }
        public int AttackRange { get; private set; }
        public int Probability { get; private set; }
        public int DestParam1 { get; private set; }
        public int DestParam2 { get; private set; }
        public int DestParam3 { get; private set; }
        public int nAdjParamVal1 { get; private set; }
        public int nAdjParamVal2 { get; private set; }
        public int nAdjParamVal3 { get; private set; }
        public int ChgParamVal1 { get; private set; }
        public int ChgParamVal2 { get; private set; }
        public int ChgParamVal3 { get; private set; }
        public int DestData1 { get; private set; }
        public int DestData2 { get; private set; }
        public int DestData3 { get; private set; }
        public int ActiveSkill { get; private set; }
        public int ActiveSkillLv { get; private set; }
        public int ActiveSkillPer { get; private set; }
        public int ReqMp { get; private set; }
        public int ReqFp { get; private set; }
        public int ReqDisLV { get; private set; }
        public int ReSkill1 { get; private set; }
        public int ReSkillLevel1 { get; private set; }
        public int ReSkill2 { get; private set; }
        public int ReSkillLevel2 { get; private set; }
        public int SkillReadyType { get; private set; }
        public int SkillReady { get; private set; }
        public int SkillRange { get; private set; }
        public int SfxElemental { get; private set; }
        public int SfxObj { get; private set; }
        public int SfxObj2 { get; private set; }
        public int SfxObj3 { get; private set; }
        public int SfxObj4 { get; private set; }
        public int SfxObj5 { get; private set; }
        public int UseMotion { get; private set; }
        public int CircleTime { get; private set; }
        public uint SkillTime { get; private set; }
        public int ExeTarget { get; private set; }
        public int UseChance { get; private set; }
        public int SpellRegion { get; private set; }
        public int SpellType { get; private set; }
        public int ReferStat1 { get; private set; }
        public int ReferStat2 { get; private set; }
        public int ReferTarget1 { get; private set; }
        public int ReferTarget2 { get; private set; }
        public int ReferValue1 { get; private set; }
        public int ReferValue2 { get; private set; }
        public int SkillType { get; private set; }
        public float ItemResistElectricity { get; private set; }
        public float ItemResistFire { get; private set; }
        public float ItemResistWind { get; private set; }
        public float ItemResistWater { get; private set; }
        public float ItemResistEarth { get; private set; }
        public int nEvildoing { get; private set; }
        public int ExpertLV { get; private set; }
        public int ExpertMax { get; private set; }
        public int SubDefine { get; private set; }
        public int Exp { get; private set; }
        public int ComboStyle { get; private set; }
        public float FlightSpeed { get; private set; }
        public float FlightLRAngle { get; private set; }
        public float FlightTBAngle { get; private set; }
        public int FlightLimit { get; private set; }
        public int FFuelReMax { get; private set; }
        public int AFuelReMax { get; private set; }
        public int FuelRe { get; private set; }
        public int LimitLevel1 { get; private set; }
        public int Reflect { get; private set; }
        public int SndAttack1 { get; private set; }
        public int SndAttack2 { get; private set; }
        public string Icon { get; private set; }
        public int QuestID { get; private set; }
        public string TextFile { get; private set; }
        public string Comment { get; private set; }

        /// <summary>
        /// Creates an empty ItemData instance.
        /// </summary>
        public ItemData() { }

        /// <summary>
        /// Creates an ItemData from a <see cref="ResourceTable"/>.
        /// </summary>
        /// <param name="table"></param>
        public ItemData(ResourceTable table)
        {
            this.Initialize(table);
        }

        /// <summary>
        /// Initialize the ItemData from a resource table.
        /// </summary>
        private void Initialize(ResourceTable table)
        {
            this.Version = table.Get<int>("dwVersion");
            this.Id = table.Get<int>("dwID");
            this.Name = table.Get<string>("szName");
            this.Num = table.Get<int>("dwNum");
            this.PackMax = table.Get<int>("dwPackMax");
            this.ItemKind1 = table.Get<int>("dwItemKind1");
            this.ItemKind2 = table.Get<int>("dwItemKind2");
            this.ItemKind3 = table.Get<int>("dwItemKind3");
            this.ItemJob = table.Get<int>("dwItemJob");
            this.Permanence = table.Get<int>("bPermanence") != 0;
            this.Useable = table.Get<int>("dwUseable");
            this.ItemSex = table.Get<int>("dwItemSex");
            this.Cost = (int)table.Get<double>("dwCost");
            this.Endurance = table.Get<int>("dwEndurance");
            this.nAbrasion = table.Get<int>("nAbrasion");
            this.nMaxRepair = table.Get<int>("nMaxRepair");
            this.Handed = table.Get<int>("dwHanded");
            this.Flag = table.Get<int>("dwFlag");
            this.Parts = table.Get<int>("dwParts");
            this.Partsub = table.Get<int>("dwPartsub");
            this.PartFile = table.Get<int>("bPartFile") != 0;
            this.Exclusive = table.Get<int>("dwExclusive");
            this.BasePartsIgnore = table.Get<int>("dwBasePartsIgnore");
            this.ItemLV = table.Get<int>("dwItemLV");
            this.ItemRare = table.Get<int>("dwItemRare");
            this.ShopAble = table.Get<int>("dwShopAble");
            this.Log = table.Get<int>("bLog") != 0;
            this.Charged = table.Get<int>("bCharged") != 0;
            this.LinkKindBullet = table.Get<int>("dwLinkKindBullet");
            this.LinkKind = table.Get<int>("dwLinkKind");
            this.AbilityMin = table.Get<int>("dwAbilityMin");
            this.AbilityMax = table.Get<int>("dwAbilityMax");
            this.eItemType = table.Get<int>("eItemType");
            this.wItemEAtk = table.Get<int>("wItemEAtk");
            this.Parry = table.Get<int>("dwParry");
            this.BlockRating = table.Get<int>("dwBlockRating");
            this.AddSkillMin = table.Get<int>("dwAddSkillMin");
            this.AddSkillMax = table.Get<int>("dwAddSkillMax");
            this.AtkStyle = table.Get<int>("dwAtkStyle");
            this.WeaponType = table.Get<int>("dwWeaponType");
            this.ItemAtkOrder1 = table.Get<int>("dwItemAtkOrder1");
            this.ItemAtkOrder2 = table.Get<int>("dwItemAtkOrder2");
            this.ItemAtkOrder3 = table.Get<int>("dwItemAtkOrder3");
            this.ItemAtkOrder4 = table.Get<int>("dwItemAtkOrder4");
            this.ContinnuousPain = table.Get<int>("bContinnuousPain") != 0;
            this.ShellQuantity = table.Get<int>("dwShellQuantity");
            this.Recoil = table.Get<int>("dwRecoil");
            this.LoadingTime = table.Get<int>("dwLoadingTime");
            this.AdjHitRate = table.Get<int>("nAdjHitRate");
            this.AttackSpeed = table.Get<float>("fAttackSpeed");
            this.DmgShift = table.Get<int>("dwDmgShift");
            this.AttackRange = table.Get<int>("dwAttackRange");
            this.Probability = table.Get<int>("dwProbability");
            this.DestParam1 = table.Get<int>("dwDestParam1");
            this.DestParam2 = table.Get<int>("dwDestParam2");
            this.DestParam3 = table.Get<int>("dwDestParam3");
            this.nAdjParamVal1 = table.Get<int>("nAdjParamVal1");
            this.nAdjParamVal2 = table.Get<int>("nAdjParamVal2");
            this.nAdjParamVal3 = table.Get<int>("nAdjParamVal3");
            this.ChgParamVal1 = table.Get<int>("dwChgParamVal1");
            this.ChgParamVal2 = table.Get<int>("dwChgParamVal2");
            this.ChgParamVal3 = table.Get<int>("dwChgParamVal3");
            this.DestData1 = table.Get<int>("dwDestData1");
            this.DestData2 = table.Get<int>("dwDestData2");
            this.DestData3 = table.Get<int>("dwDestData3");
            this.ActiveSkill = table.Get<int>("dwActiveSkill");
            this.ActiveSkillLv = table.Get<int>("dwActiveSkillLv");
            this.ActiveSkillPer = table.Get<int>("dwActiveSkillPer");
            this.ReqMp = table.Get<int>("dwReqMp");
            this.ReqFp = table.Get<int>("dwReqFp");
            this.ReqDisLV = table.Get<int>("dwReqDisLV");
            this.ReSkill1 = table.Get<int>("dwReSkill1");
            this.ReSkillLevel1 = table.Get<int>("dwReSkillLevel1");
            this.ReSkill2 = table.Get<int>("dwReSkill2");
            this.ReSkillLevel2 = table.Get<int>("dwReSkillLevel2");
            this.SkillReadyType = table.Get<int>("dwSkillReadyType");
            this.SkillReady = table.Get<int>("dwSkillReady");
            this.SkillRange = table.Get<int>("dwSkillRange");
            this.SfxElemental = table.Get<int>("dwSfxElemental");
            this.SfxObj = table.Get<int>("dwSfxObj");
            this.SfxObj2 = table.Get<int>("dwSfxObj2");
            this.SfxObj3 = table.Get<int>("dwSfxObj3");
            this.SfxObj4 = table.Get<int>("dwSfxObj4");
            this.SfxObj5 = table.Get<int>("dwSfxObj5");
            this.UseMotion = table.Get<int>("dwUseMotion");
            this.CircleTime = table.Get<int>("dwCircleTime");
            this.SkillTime = table.Get<uint>("dwSkillTime");
            this.ExeTarget = table.Get<int>("dwExeTarget");
            this.UseChance = table.Get<int>("dwUseChance");
            this.SpellRegion = table.Get<int>("dwSpellRegion");
            this.SpellType = table.Get<int>("dwSpellType");
            this.ReferStat1 = table.Get<int>("dwReferStat1");
            this.ReferStat2 = table.Get<int>("dwReferStat2");
            this.ReferTarget1 = table.Get<int>("dwReferTarget1");
            this.ReferTarget2 = table.Get<int>("dwReferTarget2");
            this.ReferValue1 = table.Get<int>("dwReferValue1");
            this.ReferValue2 = table.Get<int>("dwReferValue2");
            this.SkillType = table.Get<int>("dwSkillType");
            this.ItemResistElectricity = table.Get<float>("fItemResistElecricity");
            this.ItemResistFire = table.Get<float>("fItemResistFire");
            this.ItemResistWind = table.Get<float>("fItemResistWind");
            this.ItemResistWater = table.Get<float>("fItemResistWater");
            this.ItemResistEarth = table.Get<float>("fItemResistEarth");
            this.nEvildoing = table.Get<int>("nEvildoing");
            this.ExpertLV = table.Get<int>("dwExpertLV");
            this.ExpertMax = table.Get<int>("ExpertMax");
            this.SubDefine = table.Get<int>("dwSubDefine");
            this.Exp = table.Get<int>("dwExp");
            this.ComboStyle = table.Get<int>("dwComboStyle");
            this.FlightSpeed = table.Get<float>("fFlightSpeed");
            this.FlightLRAngle = table.Get<float>("fFlightLRAngle");
            this.FlightTBAngle = table.Get<float>("fFlightTBAngle");
            this.FlightLimit = table.Get<int>("dwFlightLimit");
            this.FFuelReMax = table.Get<int>("dwFFuelReMax");
            this.AFuelReMax = table.Get<int>("dwAFuelReMax");
            this.FuelRe = table.Get<int>("dwFuelRe");
            this.LimitLevel1 = table.Get<int>("dwLimitLevel1");
            this.Reflect = table.Get<int>("dwReflect");
            this.SndAttack1 = table.Get<int>("dwSndAttack1");
            this.SndAttack2 = table.Get<int>("dwSndAttack2");
            this.Icon = table.Get<string>("szIcon");
            this.QuestID = table.Get<int>("dwQuestID");
            this.TextFile = table.Get<string>("szTextFile");
            this.Comment = table.Get<string>("szComment");
        }
    }
}
