using Hellion.Core.Data.Headers;
using Hellion.Core.IO;
using Hellion.Core.Network;
using Hellion.Core.Structures;
using Hellion.World.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hellion.World.Systems
{
    public partial class Player
    {
        internal void SendPlayerSpawn()
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.ENVIRONMENTALL, 0x0000FF00);
                packet.Write(0); // Get weather by season


                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.WORLD_READINFO);
                packet.Write(this.MapId);
                packet.Write(this.Position.X);
                packet.Write(this.Position.Y);
                packet.Write(this.Position.Z);

                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.ADD_OBJ);

                // Object properties
                packet.Write((byte)this.Type);
                packet.Write(this.ModelId);
                packet.Write((byte)this.Type);
                packet.Write(this.ModelId);
                packet.Write(this.Size);
                packet.Write(this.Position.X);
                packet.Write(this.Position.Y);
                packet.Write(this.Position.Z);
                packet.Write((short)(this.Angle * 10));
                packet.Write(this.ObjectId);

                packet.Write<short>(0);
                packet.Write<byte>(1); // is player ?
                packet.Write(this.Attributes[DefineAttributes.HP]);
                packet.Write((int)this.MovingFlags);
                packet.Write((int)this.MotionFlags);
                packet.Write<byte>(1);

                // baby buffer
                packet.Write(-1);

                packet.Write(this.Name);
                packet.Write(this.Gender);
                packet.Write((byte)this.SkinSetId);
                packet.Write((byte)this.HairId);
                packet.Write((int)this.HairColor);
                packet.Write((byte)this.FaceId);
                packet.Write(this.Id);
                packet.Write((byte)this.ClassId);
                packet.Write((short)this.Attributes[DefineAttributes.STR]);
                packet.Write((short)this.Attributes[DefineAttributes.STA]);
                packet.Write((short)this.Attributes[DefineAttributes.DEX]);
                packet.Write((short)this.Attributes[DefineAttributes.INT]);
                packet.Write((short)this.Level);
                packet.Write(-1); // Fuel
                packet.Write(0); // Actuel fuel

                // Guilds

                packet.Write<byte>(0); // have guild or not
                packet.Write(0); // guild cloak

                // Party

                packet.Write<byte>(0); // have party or not

                packet.Write((byte)this.Authority);
                packet.Write(0); // mode
                packet.Write(0); // state mode
                packet.Write(0x000001F6); // item used ??
                packet.Write(0); // last pk time.
                packet.Write(0); // karma
                packet.Write(0); // pk propensity
                packet.Write(0); // pk exp
                packet.Write(0); // fame
                packet.Write<byte>(0); // duel
                packet.Write(-1); // titles

                foreach (var item in this.Inventory.GetEquipedItems())
                {
                    if (item == null || item.Id < 0)
                        packet.Write(0);
                    else
                    {
                        packet.Write(item.Refine); // Refine
                        packet.Write<byte>(0);
                        packet.Write(item.Element); // element (fire, water, elec...)
                        packet.Write(item.ElementRefine); // Refine element
                    }
                }

                packet.Write(0); // guild war state

                for (int i = 0; i < 26; ++i)
                    packet.Write(0);

                packet.Write((short)this.Attributes[DefineAttributes.MP]);
                packet.Write((short)this.Attributes[DefineAttributes.FP]);
                packet.Write(0); // tutorial state
                packet.Write(0); // fly experience
                packet.Write(this.Gold);
                packet.Write(this.Experience);
                packet.Write(0); // skill level
                packet.Write(0); // skill points
                packet.Write<long>(0); // death exp
                packet.Write(0); // death level

                for (int i = 0; i < 32; ++i)
                    packet.Write(0); // job in each level

                packet.Write(0); // marking world id
                packet.Write(this.Position.X);
                packet.Write(this.Position.Y);
                packet.Write(this.Position.Z);

                // Quests
                packet.Write<byte>(0);
                packet.Write<byte>(0);
                packet.Write<byte>(0);

                packet.Write(42); // murderer id
                packet.Write<short>(0); // stat points
                packet.Write<short>(0); // always 0

                // item mask
                for (int i = 0; i < 31; i++)
                    packet.Write(0);

                // skills
                for (int i = 0; i < 45; ++i)
                {
                    packet.Write(0); // skill id
                    packet.Write(0); // skill level
                }

                packet.Write<byte>(0); // cheer point
                packet.Write(0); // next cheer point ?

                // Bank
                packet.Write((byte)this.Slot);
                for (int i = 0; i < 3; ++i)
                    packet.Write(0); // gold
                for (int i = 0; i < 3; ++i)
                    packet.Write(0); // player bank ?

                packet.Write(1); //ar << m_nPlusMaxHitPoint;
                packet.Write<byte>(0);  //ar << m_nAttackResistLeft;				
                packet.Write<byte>(0);  //ar << m_nAttackResistRight;				
                packet.Write<byte>(0);  //ar << m_nDefenseResist;
                packet.Write<long>(0); //ar << m_nAngelExp;
                packet.Write(0); //ar << m_nAngelLevel;

                // Inventory
                this.Inventory.Serialize(packet);

                // Bank

                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 0x2A; ++j)
                        packet.Write(j);
                    packet.Write<byte>(0); // count
                    for (int j = 0; j < 0x2A; ++j)
                        packet.Write(j);
                }

                packet.Write(int.MaxValue); // pet id

                // Bag

                packet.Write<byte>(1);
                for (int i = 0; i < 6; i++)
                {
                    packet.Write(i);
                }
                packet.Write<byte>(0);                 // Base bag item count
                for (int i = 0; i < 0; i++)
                {
                    packet.Write((byte)i);             // Slot
                    packet.Write(i);            // Slot
                }
                for (int i = 0; i < 6; i++)
                {
                    packet.Write(i);
                }
                packet.Write(0);
                packet.Write(0);

                // premium bag
                packet.Write<byte>(0);

                packet.Write(0); // muted

                // Honor titles
                for (Int32 i = 0; i < 150; ++i)
                    packet.Write(0);

                packet.Write(0); // id campus
                packet.Write(0); // campus points

                // buffs
                packet.Write(0); // count

                // Game time packet
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.GameTime);
                //packet.Write<short>(0);
                //packet.Write<byte>(0);
                //packet.Write<byte>(0);
                //packet.Write<float>(0);


                //// Wheater
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.WeatherClear);
                //packet.Write(1);
                //packet.Write(0);

                //// party
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.PartyDefaultName);
                //packet.Write(0);

                //// total play time
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.WriteGameJoin);
                //packet.Write(0);  //total play time

                //// server settings
                //packet.StartNewMergedPacket(-1, WorldHeaders.Outgoing.ServerSettings);
                //packet.Write<byte>(0x00);
                //packet.Write<byte>(0x01);  // duel enabled ?
                //packet.Write<byte>(0x01);  // Enable guild warehouse
                //packet.Write<byte>(0x01);  // Enable guild war
                //packet.Write<byte>(0); //Configuration.ClientSettings.EnabledFriendsList ? 0 : 1);  // school
                //packet.Write<byte>(0x00);  // school_battle
                //packet.Write<byte>(0x00); //no flymonster
                //packet.Write<byte>(0x00);  //no darkon
                //packet.Write<byte>(0x00);  //no guild
                //packet.Write<byte>(0x01);  // no wormon
                //packet.Write<byte>(0x00);  // despawn
                //packet.Write<byte>(0x01);  // PK
                //packet.Write<byte>(0x01);  // PKcost
                //packet.Write<byte>(0x00); //steal
                //packet.Write<byte>(0x00); //event0913
                //packet.Write<byte>(0x01);  // guildcombat
                //packet.Write<byte>(0x00); //dropitemremove
                //packet.Write<byte>(0x00); //event1206
                //packet.Write<byte>(0x00); //event1219
                //packet.Write<byte>(0x00); //event0127
                //packet.Write<byte>(0x00); //event0214
                //packet.Write<byte>(0x01);
                //packet.Write<byte>(0x01);  // combat war 1vs 1
                //packet.Write<byte>(0x01);  //arene
                //packet.Write<byte>(0x01);  //secret room
                //packet.Write<byte>(0x01);  //rainbow race

                //for (int i = 0; i < 998; ++i)
                //    packet.Write<byte>(0);

                //#region unknow end packet
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.Unknow3E);
                //packet.Write<Int16>(0);
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.GameRates);
                //packet.Write<Single>(0f);//Configuration.Rates.ShopCostRate);
                //packet.Write<byte>(0);//(byte)RateType.ShopCost);
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.GameRates);
                //packet.Write<Single>(0f);//Configuration.Rates.ExpRate);
                //packet.Write<byte>(0);//(byte)RateType.MonsterExpRate);
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.GameRates);
                //packet.Write<Single>(0f);//Configuration.Rates.DropRate);
                //packet.Write<byte>(0);//(byte)RateType.ItemDropRate);
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.GameRates);
                //packet.Write<Single>(0f);//Configuration.Rates.GoldRate);
                //packet.Write<byte>(0);//(byte)RateType.GoldDropRate);
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.GameRates);
                //packet.Write<Single>(0f);//Configuration.Rates.MonsterHitRate);
                //packet.Write<byte>(0);//(byte)RateType.MonsterHitRate);
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.GameRates);
                //packet.Write<Single>(0f);//Configuration.Rates.MonsterHPRate);
                //packet.Write<byte>(0);//(byte)RateType.MonsterHitpoint);
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.MonsterProp);
                //packet.Write<Int64>(0);
                //#region new unknow from v14
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.AllAction);
                //packet.Write<Int64>(1);
                //#region GUILD COMBAT INFOS
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.GuildCombat);
                //packet.Write<byte>(0x08);
                //packet.Write(0);
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.GuildCombat);
                //packet.Write<byte>(0x30);
                //packet.Write(1);
                //packet.Write(0x69);
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.GuildCombat);
                //packet.Write<byte>(0x31);
                //packet.Write(Int32.MaxValue);//0xEDDE880F);
                //packet.Write(0x00);
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.GuildCombat);
                //packet.Write<byte>(0);
                //packet.Write(1);
                //packet.Write(0);
                //packet.Write(0);
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.GuildCombat);
                //packet.Write<byte>(7);
                //packet.Write(1);
                //#endregion
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.SECRETROOM_MNG_STATE);
                //packet.Write<Int64>(1);
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.TaxAllInformations);
                //packet.Write(3);
                //packet.Write<byte>(0x0F);
                //packet.Write(-1);
                //packet.Write(3);
                //packet.Write<byte>(0);
                //packet.Write(0);
                //packet.Write<byte>(1);
                //packet.Write(0);
                //packet.Write<byte>(2);
                //packet.Write(0);
                //packet.Write<byte>(0x0F);
                //packet.Write(-1);
                //packet.Write(2);
                //packet.Write<byte>(0);
                //packet.Write(0);
                //packet.Write<byte>(1);
                //packet.Write(0);
                //packet.Write<byte>(0xFF);
                //packet.Write(-1);
                //packet.Write(3);
                //packet.Write<byte>(0);
                //packet.Write(0);
                //packet.Write<byte>(1);
                //packet.Write(0);
                //packet.Write<byte>(2);
                //packet.Write(0);
                //#region Maisons de guildes
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.GuildHouseAllInformations);
                //packet.Write(1); //bSetFurnitureChannel
                //packet.Write(0); //bHaveGuildHouse
                //                        /*
                //                        {
                //                            packet.Write(0); //m_dwGuildId
                //                            packet.Write(0); //worldID
                //                            packet.Write(0); //m_tUpkeepTime
                //                            packet.Write(0); //nSize
                //                        }*/
                //#endregion
                //#region Maisons
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.HousingAllInformations);
                //packet.Write(0); //size infos housing
                //packet.Write(0); //size info ami autorisé
                //#endregion

                //#endregion
                //#endregion

                // Message hud
                //packet.StartNewMergedPacket(this.ObjectId, WorldHeaders.Outgoing.MessageHud);
                //packet.Write("Welcome to Hellion Emulator!");

                this.Send(packet);
            }
        }

        internal void SendPlayerSpawn(Player worldObject)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(worldObject.ObjectId, SnapshotType.ADD_OBJ);

                packet.Write((byte)worldObject.Type);
                packet.Write(worldObject.ModelId);
                packet.Write((byte)worldObject.Type);
                packet.Write(worldObject.ModelId);
                packet.Write(worldObject.Size);
                packet.Write(worldObject.Position.X);
                packet.Write(worldObject.Position.Y);
                packet.Write(worldObject.Position.Z);
                packet.Write((short)(worldObject.Angle * 10f));
                packet.Write(worldObject.ObjectId);

                packet.Write<short>(0);
                packet.Write<byte>(1); // is player?
                packet.Write(worldObject.Attributes[DefineAttributes.HP]);
                packet.Write((int)worldObject.MovingFlags); // moving flags
                packet.Write((int)worldObject.MotionFlags); // motion flags
                packet.Write<byte>(0);
                packet.Write(-1); // baby buffer

                packet.Write(worldObject.Name);
                packet.Write(worldObject.Gender);
                packet.Write((byte)worldObject.SkinSetId);
                packet.Write((byte)worldObject.HairId);
                packet.Write((int)worldObject.HairColor);
                packet.Write((byte)worldObject.FaceId);
                packet.Write(worldObject.Id);
                packet.Write((byte)worldObject.ClassId);
                packet.Write((short)worldObject.Attributes[DefineAttributes.STR]);
                packet.Write((short)worldObject.Attributes[DefineAttributes.STA]);
                packet.Write((short)worldObject.Attributes[DefineAttributes.DEX]);
                packet.Write((short)worldObject.Attributes[DefineAttributes.INT]);
                packet.Write((short)worldObject.Level);

                packet.Write(-1);
                packet.Write(0);

                packet.Write<byte>(0); // has guild
                packet.Write(0); // guild cloak

                packet.Write<byte>(0); // has party

                packet.Write((byte)worldObject.Authority);
                packet.Write(0); // mode
                packet.Write(0); // state mode
                packet.Write(0x000001F6); // item used ??
                packet.Write(0); // last pk time.
                packet.Write(0); // karma
                packet.Write(0); // pk propensity
                packet.Write(0); // pk exp
                packet.Write(0); // fame
                packet.Write<byte>(0); // duel
                packet.Write(-1); // titles

                for (int i = Inventory.EquipOffset; i < Inventory.MaxItems; ++i)
                {
                    var item = worldObject.Inventory.GetItemBySlot(i);

                    if (item == null || item.Id < 0)
                        packet.Write(0);
                    else
                    {
                        packet.Write(item.Refine); // Refine
                        packet.Write<byte>(0);
                        packet.Write(item.Element); // Element (fire, water, elec, ect...)
                        packet.Write(item.ElementRefine); // Element refine
                    }
                }

                for (int i = 0; i < 28; i++)
                    packet.Write(0);

                IEnumerable<Item> equipedItems = worldObject.Inventory.GetEquipedItems();

                packet.Write((byte)equipedItems.Count(x => x.Id != -1));

                foreach (var item in equipedItems)
                {
                    if (item != null && item.Id > 0)
                    {
                        packet.Write((byte)(item.Slot - Inventory.EquipOffset));
                        packet.Write((short)item.Id);
                        packet.Write<byte>(0);
                    }
                }

                packet.Write(-1); // pet ?
                packet.Write(0); // buffs ?

                this.Send(packet);
            }
        }

        internal void SendMonsterSpawn(Monster monster)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(monster.ObjectId, SnapshotType.ADD_OBJ);

                packet.Write((byte)monster.Type);
                packet.Write(monster.ModelId);
                packet.Write((byte)monster.Type);
                packet.Write(monster.ModelId);
                packet.Write(monster.Size);
                packet.Write(monster.Position.X);
                packet.Write(monster.Position.Y);
                packet.Write(monster.Position.Z);
                packet.Write((short)(monster.Angle * 10f));
                packet.Write(monster.ObjectId);

                packet.Write<short>(5);
                packet.Write((byte)0);
                packet.Write(monster.Attributes[DefineAttributes.HP]);
                packet.Write(1);
                packet.Write(0);
                packet.Write((byte)monster.Data.Belligerence);
                packet.Write(-1);
                packet.Write((byte)0);
                packet.Write(-1);
                packet.Write((byte)0);
                packet.Write(0);
                packet.Write((byte)0);
                if (this.ModelId == 1021)
                {
                    packet.Write((byte)0);
                }
                else
                {
                    packet.Write(false ? (byte)1 : (byte)0);
                }
                packet.Write((byte)0);
                packet.Write((byte)0);
                packet.Write(0);
                packet.Write(this.SpeedFactor);
                packet.Write(0);

                this.Send(packet);
            }
        }

        internal void SendDespawnObject(WorldObject worldObject)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(worldObject.ObjectId, SnapshotType.DEL_OBJ);

                this.Send(packet);
            }
        }

        internal void SendMoveByKeyboard(Vector3 direction, int motionEx, int loop, int motionOption, long tick)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.MOVERMOVED);

                packet.Write(this.Position.X);
                packet.Write(this.Position.Y);
                packet.Write(this.Position.Z);
                packet.Write(direction.X);
                packet.Write(direction.Y);
                packet.Write(direction.Z);
                packet.Write(this.Angle);
                packet.Write((uint)this.MovingFlags);
                packet.Write((int)this.MotionFlags);
                packet.Write(this.ActionFlags);
                packet.Write(motionEx);
                packet.Write(loop);
                packet.Write(motionOption);
                packet.Write(tick);

                base.SendToVisible(packet);
            }
        }

        internal void SendMoverBehavior(Vector3 direction, int motionEx, int loop, int motionOption, long tick)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.MOVERBEHAVIOR);

                packet.Write(this.Position.X);
                packet.Write(this.Position.Y);
                packet.Write(this.Position.Z);
                packet.Write(direction.X);
                packet.Write(direction.Y);
                packet.Write(direction.Z);
                packet.Write(this.Angle);
                packet.Write((uint)this.MovingFlags);
                packet.Write((int)this.MotionFlags);
                packet.Write(this.ActionFlags);
                packet.Write(motionEx);
                packet.Write(loop);
                packet.Write(motionOption);
                packet.Write(tick);

                base.SendToVisible(packet);
            }
        }

        internal void SendMoverMoved(Vector3 direction, int motionEx, int loop, int motionOption, long tick, byte frame, float turnAngle)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.MOVERMOVED2);

                packet.Write(this.Position.X);
                packet.Write(this.Position.Y);
                packet.Write(this.Position.Z);
                packet.Write(direction.X);
                packet.Write(direction.Y);
                packet.Write(direction.Z);
                packet.Write(this.Angle);
                packet.Write(this.AngleFly);
                packet.Write(this.FlightSpeed);
                packet.Write(turnAngle);
                packet.Write((uint)this.MovingFlags);
                packet.Write((int)this.MotionFlags);
                packet.Write(this.ActionFlags);
                packet.Write(motionEx);
                packet.Write(loop);
                packet.Write(motionOption);
                packet.Write(tick);
                packet.Write(frame);

                base.SendToVisible(packet);
            }
        }

        public void SendMoverAngle(Vector3 direction, long tick, float turnAngle)
        {
            using (var packet = new FFPacket())
            {
                packet.StartNewMergedPacket(this.ObjectId, SnapshotType.MOVERANGLE);

                packet.Write(this.Position.X);
                packet.Write(this.Position.Y);
                packet.Write(this.Position.Z);
                packet.Write(direction.X);
                packet.Write(direction.Y);
                packet.Write(direction.Z);
                packet.Write(this.Angle);
                packet.Write(this.AngleFly);
                packet.Write(this.FlightSpeed);
                packet.Write(turnAngle);
                packet.Write(tick);

                base.SendToVisible(packet);
            }
        }
    }
}
