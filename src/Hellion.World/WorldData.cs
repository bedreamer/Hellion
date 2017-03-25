using Hellion.Core;
using Hellion.Core.Configuration;
using Hellion.Core.Data.Resources;
using Hellion.Core.IO;
using Hellion.Core.Structures;
using Hellion.Core.Structures.Dialogs;
using Hellion.Data;
using Hellion.World.Managers;
using Hellion.World.Structures;
using Hellion.World.Systems;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Hellion.World
{
    public partial class WorldServer
    {
        private Dictionary<string, int> defines = new Dictionary<string, int>();
        private Dictionary<string, string> texts = new Dictionary<string, string>();

        private static ExpTableData[] expTable;
        private static Dictionary<int, ItemData> itemsData = new Dictionary<int, ItemData>();
        private static Dictionary<int, MoverData> moversData = new Dictionary<int, MoverData>();
        private static Dictionary<string, DialogData> dialogData = new Dictionary<string, DialogData>();
        private static Dictionary<string, NPCData> npcData = new Dictionary<string, NPCData>();
        private static Dictionary<int, SkillData> skillData = new Dictionary<int, SkillData>();
        private static Dictionary<int, JobData> jobData = new Dictionary<int, JobData>();
        private static MapManager mapManager;

        /// <summary>
        /// Gets the Map manager.
        /// </summary>
        public static MapManager MapManager
        {
            get { return mapManager = mapManager ?? new MapManager(); }
        }

        /// <summary>
        /// Gets the items data.
        /// </summary>
        public static Dictionary<int, ItemData> ItemsData
        {
            get { return itemsData; }
        }

        /// <summary>
        /// Gets the movers data.
        /// </summary>
        public static Dictionary<int, MoverData> MoversData
        {
            get { return moversData; }
        }

        /// <summary>
        /// Gets the dialogs data.
        /// </summary>
        public static Dictionary<string, DialogData> DialogsData
        {
            get { return dialogData; }
        }

        /// <summary>
        /// Gets the NPC data.
        /// </summary>
        public static Dictionary<string, NPCData> NPCData
        {
            get { return npcData; }
        }

        /// <summary>
        /// Gets the skills data.
        /// </summary>
        public static Dictionary<int, SkillData> SkillData
        {
            get { return skillData; }
        }

        /// <summary>
        /// Gets the jobs data.
        /// </summary>
        public static Dictionary<int, JobData> JobData
        {
            get { return jobData; }
        }

        /// <summary>
        /// Gets the player experience table.
        /// </summary>
        public static ExpTableData[] ExpTable
        {
            get { return expTable; }
        }

        /// <summary>
        /// Loads the world server data like resources, maps, quests, dialogs, etc...
        /// </summary>
        private void LoadData()
        {
            var startTime = DateTime.Now;
            Log.Info("Loading world data...");

            this.LoadDefines();
            this.LoadTexts();
            this.LoadItems();
            this.LoadNpc();
            this.LoadMovers();
            this.LoadExpTable();
            this.LoadSkills();
            this.LoadJobs();
            this.LoadMaps();
            this.Clear();

            Log.Done("World data loaded in {0}s", (DateTime.Now - startTime).TotalSeconds);
        }

        /// <summary>
        /// Clear all unused resources.
        /// </summary>
        private void Clear()
        {
            this.defines.Clear();
            this.texts.Clear();

            npcData.Clear();
            dialogData.Clear();

            GC.Collect();
        }

        /// <summary>
        /// Load the flyff defines files.
        /// </summary>
        private void LoadDefines()
        {
            string[] defines = {
                                    "define.h",
                                    "defineAttribute.h",
                                    "defineEvent.h",
                                    "defineHonor.h",
                                    "defineItem.h",
                                    "defineItemkind.h",
                                    "defineJob.h",
                                    "defineNeuz.h",
                                    "defineObj.h",
                                    "defineSkill.h",
                                    "defineSound.h",
                                    "defineText.h",
                                    "defineWorld.h"
                                };

            foreach (var defineFile in defines)
            {
                var defineFileContent = new DefineFile(Path.Combine(Global.DataPath, "res", "data", defineFile));
                defineFileContent.Parse();

                foreach (var define in defineFileContent.Defines)
                {
                    if (!this.defines.ContainsKey(define.Key) && define.Value is int)
                        this.defines.Add(define.Key, int.Parse(define.Value.ToString()));
                }
            }
        }

        /// <summary>
        /// Load all FlyFF texts files.
        /// </summary>
        private void LoadTexts()
        {
            string[] textPaths =
            {
                "data/propCtrl.txt.txt",
                "data/propItemEtc.txt.txt",
                "data/propKarma.txt.txt",
                "data/propMotion.txt.txt",
                "data/propMover.txt.txt",
                "data/propSkill.txt.txt",
                "data/propTroupeSkill.txt.txt",
                "data/textEmotion.txt.txt",
                "data/world.txt.txt",
                "dataSub1/lordskill.txt.txt",
                "dataSub1/propQuest.txt.txt",
                "dataSub1/propQuest-Scenario.txt.txt",
                "dataSub1/propQuest-DungeonandPK.txt.txt",
                "dataSub1/etc.txt.txt",
                "dataSub1/character.txt.txt",
                "dataSub1/character-etc.txt.txt",
                "dataSub1/character-school.txt.txt",
                "dataSub2/propItem.txt.txt",
            };

            foreach (var textFilePath in textPaths)
            {
                var textFile = new TextFile(Path.Combine(Global.DataPath, "res", textFilePath));

                textFile.Parse();

                foreach (var text in textFile.Texts)
                {
                    if (this.texts.ContainsKey(text.Key))
                        Log.Warning("The text key '{0}' already exists.", text.Key);
                    else
                        this.texts.Add(text.Key, text.Value);
                }
            }
        }

        /// <summary>
        /// Load all flyff npcs.
        /// </summary>
        private void LoadNpc()
        {
            this.LoadNpcDialogs();

            Log.Loading("Loading NPC data...");

            string[] files = {
                                  "data//res//dataSub1//character.inc",
                                  "data//res//dataSub1//character-etc.inc",
                                  "data//res//dataSub1//character-school.inc"
                              };

            foreach (var npcFile in files)
            {
                string path = Path.Combine(Global.DataPath, npcFile);
                var npcGroupFile = new ResourceGroup(npcFile);

                npcGroupFile.Parse();

                foreach (var npc in npcGroupFile.Groups)
                {
                    var newNpc = new NPCData();

                    newNpc.Name = npc.Name;

                    if (!npcData.ContainsKey(newNpc.Name))
                        npcData.Add(newNpc.Name, newNpc);
                    else
                        npcData[newNpc.Name] = newNpc;
                }
            }

            Log.Done("{0} npcs data loaded!\t\t\t", npcData.Count);
        }

        /// <summary>
        /// Load all NPC dialogs.
        /// </summary>
        private void LoadNpcDialogs()
        {
            Log.Loading("Loading NPC dialogs for '{0}' language...", this.WorldConfiguration.Language);

            string dialogLanguagePath = $"dialogs/{this.WorldConfiguration.Language}";
            string dialogPath = Path.Combine(Global.DataPath, dialogLanguagePath);

            if (!Directory.Exists(dialogPath))
            {
                Log.Error("Cannot find '{0}' dialog path. No dialogs will be loaded.", dialogLanguagePath);
                return;
            }

            string[] dialogFilesPath = Directory.GetFiles(dialogPath);

            foreach (var dialogFile in dialogFilesPath)
            {
                string dialogFileContent = File.ReadAllText(dialogFile);
                var dialogsParsed = JToken.Parse(dialogFileContent, new JsonLoadSettings()
                {
                    CommentHandling = CommentHandling.Ignore,
                });

                if (dialogsParsed.Type == JTokenType.Array)
                {
                    var newDialogs = dialogsParsed.ToObject<DialogData[]>();

                    foreach (var newDialog in newDialogs)
                    {
                        if (!dialogData.ContainsKey(newDialog.Name))
                            dialogData.Add(newDialog.Name, newDialog);
                    }
                }
                else if (dialogsParsed.Type == JTokenType.Object)
                {
                    var newDialog = dialogsParsed.ToObject<DialogData>();

                    if (!dialogData.ContainsKey(newDialog.Name))
                        dialogData.Add(newDialog.Name, newDialog);
                }
            }

            Log.Done("{0} npc dialogs loaded for language '{1}'!\t\t\t", dialogData.Count, this.WorldConfiguration.Language);
        }

        /// <summary>
        /// Load all flyff movers.
        /// </summary>
        private void LoadMovers()
        {
            Log.Loading("Loading movers data...");

            try
            {
                string propMoverPath = Path.Combine(Global.DataPath, "res", "data", "propMover.txt");
                var propMover = new ResourceTable(propMoverPath);

                propMover.AddDefines(defines);
                propMover.AddTexts(texts);
                propMover.SetTableHeaders("dwID", "szName", "dwAI", "dwStr", "dwSta", "dwDex", "dwInt", "dwHR", "dwER", "dwRace", "dwBelligerence", "dwGender", "dwLevel", "dwFlightLevel", "dwSize", "dwClass", "bIfPart", "dwKarma", "dwUseable", "dwActionRadius", "dwAtkMin", "dwAtkMax", "dwAtk1", "dwAtk2", "dwAtk3", "dwHorizontalRate", "dwVerticalRate", "dwDiagonalRate", "dwThrustRate", "dwChestRate", "dwHeadRate", "dwArmRate", "dwLegRate", "dwAttackSpeed", "dwReAttackDelay", "dwAddHp", "dwAddMp", "dwNaturealArmor", "nAbrasion", "nHardness", "dwAdjAtkDelay", "eElementType", "wElementAtk", "dwHideLevel", "fSpeed", "dwShelter", "bFlying", "dwJumping", "dwAirJump", "bTaming", "dwResistMagic", "fResistElectricity", "fResistFire", "fResistWind", "fResistWater", "fResistEarth", "dwCash", "dwSourceMaterial", "dwMaterialAmount", "dwCohesion", "dwHoldingTime", "dwCorrectionValue", "dwExpValue", "nFxpValue", "nBodyState", "dwAddAbility", "bKillable", "dwVirtItem1", "dwVirtType1", "dwVirtItem2", "dwVirtType2", "dwVirtItem3", "dwVirtType3", "dwSndAtk1", "dwSndAtk2", "dwSndDie1", "dwSndDie2", "dwSndDmg1", "dwSndDmg2", "dwSndDmg3", "dwSndIdle1", "dwSndIdle2", "szComment");
                propMover.Parse();

                while (propMover.Read())
                {
                    var monsterData = new MoverData(propMover);

                    if (moversData.ContainsKey(monsterData.Id))
                        moversData[monsterData.Id] = monsterData;
                    else
                        moversData.Add(monsterData.Id, monsterData);

                    Log.Loading("Loading {0}/{1} movers...", propMover.ReadingIndex, propMover.Count);
                }

                Log.Done("{0} movers loaded!\t\t\t", moversData.Count);
            }
            catch (Exception e)
            {
                Log.Error("Cannot load movers: {0}", e.Message);
            }
        }

        /// <summary>
        /// Load the flyff maps specified in the configuration file.
        /// </summary>
        private void LoadMaps()
        {
            Log.Loading("Loading maps...");

            IEnumerable<MapConfiguration> maps = this.WorldConfiguration.Maps;

            foreach (var map in maps)
            {
                Log.Loading("Loading map '{0}'...", map.Name);
                var newMap = new Map(map.Id, map.Name);
                newMap.Load();
                newMap.StartThread();

                MapManager.AddMap(newMap);
            }

            Log.Done("{0} maps loaded!\t\t\t", MapManager.Count);
        }

        /// <summary>
        /// Load all FlyFF items.
        /// </summary>
        private void LoadItems()
        {
            try
            {
                Log.Loading("Loading items...");
                string propItemPath = Path.Combine(Global.DataPath, "res", "dataSub2", "propItem.txt");
                var propItemTable = new ResourceTable(propItemPath);

                propItemTable.AddTexts(texts);
                propItemTable.AddDefines(defines);
                propItemTable.SetTableHeaders("dwVersion", "dwID", "szName", "dwNum", "dwPackMax", "dwItemKind1", "dwItemKind2", "dwItemKind3", "dwItemJob", "bPermanence", "dwUseable", "dwItemSex", "dwCost", "dwEndurance", "nAbrasion", "nMaxRepair", "dwHanded", "dwFlag", "dwParts", "dwPartsub", "bPartFile", "dwExclusive", "dwBasePartsIgnore", "dwItemLV", "dwItemRare", "dwShopAble", "bLog", "bCharged", "dwLinkKindBullet", "dwLinkKind", "dwAbilityMin", "dwAbilityMax", "eItemType", "wItemEAtk", "dwParry", "dwBlockRating", "dwAddSkillMin", "dwAddSkillMax", "dwAtkStyle", "dwWeaponType", "dwItemAtkOrder1", "dwItemAtkOrder2", "dwItemAtkOrder3", "dwItemAtkOrder4", "bContinnuousPain", "dwShellQuantity", "dwRecoil", "dwLoadingTime", "nAdjHitRate", "fAttackSpeed", "dwDmgShift", "dwAttackRange", "dwProbability", "dwDestParam1", "dwDestParam2", "dwDestParam3", "nAdjParamVal1", "nAdjParamVal2", "nAdjParamVal3", "dwChgParamVal1", "dwChgParamVal2", "dwChgParamVal3", "dwDestData1", "dwDestData2", "dwDestData3", "dwActiveSkill", "dwActiveSkillLv", "dwActiveSkillPer", "dwReqMp", "dwReqFp", "dwReqDisLV", "dwReSkill1", "dwReSkillLevel1", "dwReSkill2", "dwReSkillLevel2", "dwSkillReadyType", "dwSkillReady", "dwSkillRange", "dwSfxElemental", "dwSfxObj", "dwSfxObj2", "dwSfxObj3", "dwSfxObj4", "dwSfxObj5", "dwUseMotion", "dwCircleTime", "dwSkillTime", "dwExeTarget", "dwUseChance", "dwSpellRegion", "dwSpellType", "dwReferStat1", "dwReferStat2", "dwReferTarget1", "dwReferTarget2", "dwReferValue1", "dwReferValue2", "dwSkillType", "fItemResistElecricity", "fItemResistFire", "fItemResistWind", "fItemResistWater", "fItemResistEarth", "nEvildoing", "dwExpertLV", "ExpertMax", "dwSubDefine", "dwExp", "dwComboStyle", "fFlightSpeed", "fFlightLRAngle", "fFlightTBAngle", "dwFlightLimit", "dwFFuelReMax", "dwAFuelReMax", "dwFuelRe", "dwLimitLevel1", "dwReflect", "dwSndAttack1", "dwSndAttack2", "szIcon", "dwQuestID", "szTextFile", "szComment");
                propItemTable.Parse();

                while (propItemTable.Read())
                {
                    var itemData = new ItemData(propItemTable);

                    if (itemsData.ContainsKey(itemData.Id))
                        itemsData[itemData.Id] = itemData;
                    else
                        itemsData.Add(itemData.Id, itemData);

                    Log.Loading("Loading {0}/{1} items...", propItemTable.ReadingIndex, propItemTable.Count);
                }

                Log.Done("{0} items loaded!\t\t\t", itemsData.Count);
            }
            catch (Exception e)
            {
                Log.Error("Cannot load items: {0}", e.Message);
            }
        }

        /// <summary>
        /// Load flyff skills and levels.
        /// </summary>
        private void LoadSkills()
        {
            try
            {
                Log.Loading("Loading skills...");
                string propSkillPath = Path.Combine(Global.DataPath, "res", "data", "propSkill.txt");
                var propSkillTable = new ResourceTable(propSkillPath);

                propSkillTable.AddTexts(texts);
                propSkillTable.AddDefines(defines);
                propSkillTable.SetTableHeaders("ver", "dwID", "szName", "dwNum", "dwPackMax", "dwItemKind1", "dwItemKind2", "dwItemKind3", "dwItemJob", "bPermanence", "dwUseable", "dwItemSex", "dwCost", "dwEndurance", "nAbrasion", "nHardness", "dwHanded", "dwHeelH", "dwParts", "dwPartsub", "bPartFile", "dwExclusive", "dwBasePartsIgnore", "dwItemLV", "dwItemRare", "dwShopAble", "bLog", "bCharged", "dwLinkKindBullet", "dwLinkKind", "dwAbilityMin", "dwAbilityMax", "eItemType", "wItemEAtk", "dwparry", "dwblockRating", "dwAddSkillMin", "dwAddSkillMax", "dwAtkStyle", "dwWeaponType", "dwItemAtkOrder1", "dwItemAtkOrder2", "dwItemAtkOrder3", "dwItemAtkOrder4", "tmContinuousPain", "dwShellQuantity", "dwRecoil", "dwLoadingTime", "nAdjHitRate", "dwAttackSpeed", "dwDmgShift", "dwAttackRange", "dwProbability", "dwDestParam1", "dwDestParam2", "dwDestParam3", "nAdjParamVal1", "nAdjParamVal2", "nAdjParamVal3", "dwChgParamVal1", "dwChgParamVal2", "dwChgParamVal3", "dwdestData1", "dwdestData2", "dwdestData3", "dwactiveskill", "dwactiveskillLv", "dwactiveskillper", "dwReqMp", "dwRepFp", "dwReqDisLV", "dwReSkill1", "dwReSkillLevel1", "dwReSkill2", "dwReSkillLevel2", "dwSkillReadyType", "dwSkillReady", "dwSkillRange", "dwSfxElemental", "dwSfxObj", "dwSfxObj2", "dwSfxObj3", "dwSfxObj4", "dwSfxObj5", "dwUseMotion", "dwCircleTime", "dwSkillTime", "dwExeTarget", "dwUseChance", "dwSpellRegion", "dwSpellType", "dwReferStat1", "dwReferStat2", "dwReferTarget1", "dwReferTarget2", "dwReferValue1", "dwReferValue2", "dwSkillType", "fItemResistElecricity", "fItemResistFire", "fItemResistWind", "fItemResistWater", "fItemResistEarth", "nEvildoing", "dwExpertLV", "ExpertMax", "dwSubDefine", "dwExp", "dwComboStyle", "fFlightSpeed", "fFlightLRAngle", "fFlightTBAngle", "dwFlightLimit", "dwFFuelReMax", "dwAFuelReMax", "dwFuelRe", "dwLimitLevel1", "dwReflect", "dwSndAttack1", "dwSndAttack2", "szIcon", "dwQuestID", "szTextFile", "szComment");
                propSkillTable.Parse();

                while (propSkillTable.Read())
                {
                    var skill = new SkillData(propSkillTable);

                    if (skillData.ContainsKey(skill.ID))
                        skillData[skill.ID] = skill;
                    else
                        skillData.Add(skill.ID, skill);
                }

                string propSkillLevelPath = Path.Combine(Global.DataPath, "res", "data", "propSkillAdd.csv");
                var propSkillLevelTable = new ResourceTable(propSkillLevelPath);
                propSkillLevelTable.AddTexts(texts);
                propSkillLevelTable.AddDefines(defines);
                propSkillLevelTable.SetTableHeaders("dwLevelID", "dwSkillID", "dwSkillLevel", "dwAbilityMin", "dwAbilityMax", "dwAbilityMinPVP", "dwAbilityMaxPVP", "dwAttackSpeed", "dwDmgShift", "nProbability", "nProbabilityPVP", "dwTaunt", "dwDestParam1", "dwDestParam2", "nAdjParamVal1", "nAdjParamVal2", "dwChgParamVal1", "dwChgParamVal2", "dwDestData1", "dwDestData2", "dwDestData3", "dwActiveSkill", "dwActiveSkillRate", "dwActiveSkillRatePVP", "dwReqMp", "dwReqFp", "dwCooldown", "dwCastingTime", "dwSkillRange", "dwCircleTime", "dwPainTime", "dwSkillTime", "dwSkillCount", "dwSkillExp", "dwExp", "dwComboSkillTime");
                propSkillLevelTable.Parse();

                while (propSkillLevelTable.Read())
                {
                    var skillLevel = new SkillLevelData(propSkillLevelTable);

                    if (skillData.ContainsKey(skillLevel.SkillID))
                        skillData[skillLevel.SkillID].Levels.Add(skillLevel);
                }

                Log.Done("{0} skills loaded!\t\t\t", skillData.Count);
            }
            catch (Exception e)
            {
                Log.Error("Cannot load skills: {0}", e.Message);
            }
        }

        private void LoadJobs()
        {
            try
            {
                string propJobPath = Path.Combine(Global.DataPath, "res", "dataSub1", "propJob.inc");
                var propJobTable = new ResourceTable(propJobPath);

                propJobTable.AddDefines(defines);
                propJobTable.SetTableHeaders("nJob", "fAttackSpeed", "fFactorMaxHp", "fFactorMaxMp", "fFactorMaxFp", "fFactorDef", "fFactorHPRecovery", "fFactorMPRecovery", "fFactorFPRecovery", "fMeleeSWD", "fMeleeAXE", "fMeleeSTAFF", "fMeleeSTICK", "fMeleeKNUCKLE", "fMagicWang", "fBlocking", "fMeleeYOYO", "fCritical");
                propJobTable.Parse();

                while (propJobTable.Read())
                {
                    var job = new JobData(propJobTable);

                    jobData.Add(job.Id, job);
                }
            }
            catch (Exception e)
            {
                Log.Error("Cannot load job properties: {0}", e.Message);
            }
        }

        private void LoadExpTable()
        {
            try
            {
                string expTablePath = Path.Combine(Global.DataPath, "res", "data", "expTable.inc");
                var expTableData = new ResourceGroup(expTablePath);

                expTableData.Parse();

                foreach (var table in expTableData.Groups)
                {
                    if (table.Name == "expCharacter")
                    {
                        expTable = new ExpTableData[table.Content.Count - 1];

                        for (int i = 1; i < table.Content.Count; ++i)
                        {
                            int level = i - 1;
                            string[] expData = table.Content[i].Split('\t');

                            if (expData.Length < 4)
                                continue;

                            expTable[level] = new ExpTableData(level, expData);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Cannot load exp table: {0}", e.Message);
            }
        }
    }
}
