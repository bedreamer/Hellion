using Hellion.Core.Helpers;
using Hellion.Core.Structures;

namespace Hellion.World.Structures
{
    public abstract class Region
    {
        public Vector3 Middle { get; protected set; }

        public Vector3 NorthWest { get; protected set; }

        public Vector3 SouthEast { get; protected set; }

        public float Width { get; private set; }

        public float Length { get; private set; }

        public Region(Vector3 middle, Vector3 northWest, Vector3 southEast)
        {
            this.Middle = middle;
            this.NorthWest = northWest;
            this.SouthEast = southEast;
        }

        public Vector3 GetRandomPosition()
        {
            var position = new Vector3();
            
            position.X = RandomHelper.FloatRandom(this.SouthEast.X, this.NorthWest.X);
            position.Y = this.Middle.Y;
            position.Z = RandomHelper.FloatRandom(this.SouthEast.Z, this.NorthWest.Z);

            return position;
        }

        public abstract void Update();
    }
}
