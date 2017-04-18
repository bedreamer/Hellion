using Hellion.Core.Structures;
using Hellion.World.Structures;

namespace Hellion.World.Systems
{
    public class RespawnerRegion : Region
    {
        /// <summary>
        /// Gets the region respawn time.
        /// </summary>
        public int RespawnTime { get; private set; }

        public RespawnerRegion(Vector3 position, Vector3 northWest, Vector3 southEast, int respawnTime)
            : base(position, northWest, southEast)
        {
            this.RespawnTime = respawnTime;
        }

        public override void Update()
        {
        }
    }
}
