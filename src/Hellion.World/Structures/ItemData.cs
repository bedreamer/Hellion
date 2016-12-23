﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hellion.Core.Data.Resources;

namespace Hellion.World.Structures
{
    public class ItemData
    {
        #region FIELDS

        public int Version;
        public int ID;
        public string Name;
        public int Num;
        public int PackMax;
        public int ItemKind1;
        public int ItemKind2;
        public int ItemKind3;
        public int ItemJob;
        public bool Permanence;
        public int Useable;
        public int ItemSex;
        public int Cost;
        public int Endurance;
        public int nAbrasion;
        public int nMaxRepair;
        public int Handed;
        public int Flag;
        public int Parts;
        public int Partsub;
        public bool PartFile;
        public int Exclusive;
        public int BasePartsIgnore;
        public int ItemLV;
        public int ItemRare;
        public int ShopAble;
        public bool Log;
        public bool Charged;
        public int LinkKindBullet;
        public int LinkKind;
        public int AbilityMin;
        public int AbilityMax;
        public int eItemType;
        public int wItemEAtk;
        public int Parry;
        public int BlockRating;
        public int AddSkillMin;
        public int AddSkillMax;
        public int AtkStyle;
        public int WeaponType;
        public int ItemAtkOrder1;
        public int ItemAtkOrder2;
        public int ItemAtkOrder3;
        public int ItemAtkOrder4;
        public bool ContinnuousPain;
        public int ShellQuantity;
        public int Recoil;
        public int LoadingTime;
        public int AdjHitRate;
        public float AttackSpeed;
        public int DmgShift;
        public int AttackRange;
        public int Probability;
        public int DestParam1;
        public int DestParam2;
        public int DestParam3;
        public int nAdjParamVal1;
        public int nAdjParamVal2;
        public int nAdjParamVal3;
        public int ChgParamVal1;
        public int ChgParamVal2;
        public int ChgParamVal3;
        public int DestData1;
        public int DestData2;
        public int DestData3;
        public int ActiveSkill;
        public int ActiveSkillLv;
        public int ActiveSkillPer;
        public int ReqMp;
        public int ReqFp;
        public int ReqDisLV;
        public int ReSkill1;
        public int ReSkillLevel1;
        public int ReSkill2;
        public int ReSkillLevel2;
        public int SkillReadyType;
        public int SkillReady;
        public int SkillRange;
        public int SfxElemental;
        public int SfxObj;
        public int SfxObj2;
        public int SfxObj3;
        public int SfxObj4;
        public int SfxObj5;
        public int UseMotion;
        public int CircleTime;
        public uint SkillTime;
        public int ExeTarget;
        public int UseChance;
        public int SpellRegion;
        public int SpellType;
        public int ReferStat1;
        public int ReferStat2;
        public int ReferTarget1;
        public int ReferTarget2;
        public int ReferValue1;
        public int ReferValue2;
        public int SkillType;
        public float ItemResistElectricity;
        public float ItemResistFire;
        public float ItemResistWind;
        public float ItemResistWater;
        public float ItemResistEarth;
        public int nEvildoing;
        public int ExpertLV;
        public int ExpertMax;
        public int SubDefine;
        public int Exp;
        public int ComboStyle;
        public float FlightSpeed;
        public float FlightLRAngle;
        public float FlightTBAngle;
        public int FlightLimit;
        public int FFuelReMax;
        public int AFuelReMax;
        public int FuelRe;
        public int LimitLevel1;
        public int Reflect;
        public int SndAttack1;
        public int SndAttack2;
        public string Icon;
        public int QuestID;
        public string TextFile;
        public string Comment;

        #endregion

        #region CONSTRUCTORS

        public ItemData() { }

        public ItemData(ResourceTable table)
        {
            this.Initialize(table);
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Initialize the ItemData
        /// </summary>
        private void Initialize(ResourceTable table)
        {
            this.Version = table.Get<int>("dwVersion");
            this.ID = table.Get<int>("dwID");
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

        #endregion
    }
}
