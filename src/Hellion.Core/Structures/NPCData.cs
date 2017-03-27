using System.Collections.Generic;

namespace Hellion.Core.Structures
{
    public struct NPCData
    {
        /// <summary>
        /// Gets or sets the NPC id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the NPC name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the NPC model id.
        /// </summary>
        public int ModelId { get; set; }

        /// <summary>
        /// Gets or sets the NPC hair id.
        /// </summary>
        public int HairId { get; set; }

        /// <summary>
        /// Gets or sets the NPC hair color.
        /// </summary>
        public int HairColor { get; set; }

        /// <summary>
        /// Gets or sets the NPC face id.
        /// </summary>
        public int FaceId { get; set; }

        /// <summary>
        /// Gets or sets the NPC viewable models.
        /// </summary>
        public ICollection<int> Items { get; set; }
    }
}
