﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hellion.Database.Structures
{
    [Table("characters")]
    public class DbCharacter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("accountId")]
        public int AccountId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("gender")]
        public byte Gender { get; set; }

        [Column("level")]
        public int Level { get; set; }

        [Column("exp")]
        public long Experience { get; set; }

        [Column("classId")]
        public int ClassId { get; set; }

        [Column("gold")]
        public int Gold { get; set; }

        [Column("slot")]
        public int Slot { get; set; }

        [Column("strength")]
        public int Strength { get; set; }

        [Column("stamina")]
        public int Stamina { get; set; }

        [Column("dexterity")]
        public int Dexterity { get; set; }

        [Column("intelligence")]
        public int Intelligence { get; set; }

        [Column("hp")]
        public int Hp { get; set; }

        [Column("mp")]
        public int Mp { get; set; }

        [Column("fp")]
        public int Fp { get; set; }

        [Column("skinSetId")]
        public int SkinSetId { get; set; }

        [Column("hairId")]
        public int HairId { get; set; }

        [Column("hairColor")]
        public uint HairColor { get; set; }

        [Column("faceId")]
        public int FaceId { get; set; }

        [Column("mapId")]
        public int MapId { get; set; }

        [Column("posX")]
        public float PosX { get; set; }

        [Column("posY")]
        public float PosY { get; set; }

        [Column("posZ")]
        public float PosZ { get; set; }

        [Column("angle")]
        public float Angle { get; set; }

        [Column("bankCode")]
        public int BankCode { get; set; }

        [Column("statPoints")]
        public int StatPoints { get; set; }

        [Column("skillPoints")]
        public int SkillPoints { get; set; }

        public virtual ICollection<DbItem> Items { get; set; }

        public DbCharacter()
        {
            this.Items = new HashSet<DbItem>();
        }
    }
}
