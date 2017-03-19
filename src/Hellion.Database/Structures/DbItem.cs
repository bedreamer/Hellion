using System.ComponentModel.DataAnnotations.Schema;

namespace Hellion.Database.Structures
{
    [Table("items")]
    public class DbItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("itemId")]
        public int ItemId { get; set; }

        [Column("characterId")]
        public int CharacterId { get; set; }

        [Column("itemCount")]
        public int ItemCount { get; set; }

        [Column("itemSlot")]
        public int ItemSlot { get; set; }

        [Column("itemCreatorId")]
        public int CreatorId { get; set; }

        [Column("refine")]
        public byte Refine { get; set; }

        [Column("element")]
        public byte Element { get; set; }

        [Column("elementRefine")]
        public byte ElementRefine { get; set; }

        public DbCharacter Character { get; set; }
    }
}
