using Hellion.World.Structures;
using Hellion.Core.Structures;

namespace Hellion.World.Systems
{
    public sealed class RevivalRegion : Region
    {
        /// <summary>
        /// Gets the revival map id.
        /// </summary>
        public int MapId { get; private set; }

        /// <summary>
        /// Gets the revival key.
        /// </summary>
        public string Key { get; private set; }

        public RevivalRegion(Vector3 position, Vector3 northWest, Vector3 southEast, int mapId, string key)
            : base(position, northWest, southEast)
        {
            this.MapId = mapId;
            this.Key = key;
        }

        public override void Update()
        {
        }
    }
}
