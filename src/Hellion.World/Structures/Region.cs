using Hellion.Core.Helpers;
using Hellion.Core.Structures;

namespace Hellion.World.Structures
{
    public abstract class Region
    {
        public Vector3 Position { get; protected set; }

        public Vector3 TopLeft { get; protected set; }

        public Vector3 BottomRight { get; protected set; }

        public float Width { get; private set; }

        public float Length { get; private set; }

        public Region(Vector3 position, Vector3 topLeft, Vector3 bottomRight)
        {
            this.Position = position;
            this.TopLeft = topLeft;
            this.BottomRight = bottomRight;
        }

        public Vector3 GetRandomPosition()
        {
            var position = new Vector3();
            
            position.X = RandomHelper.FloatRandom(this.BottomRight.X, this.TopLeft.X);
            position.Y = this.Position.Y;
            position.Z = RandomHelper.FloatRandom(this.BottomRight.Z, this.TopLeft.Z);

            return position;
        }

        public bool IsInRegion(Vector3 position)
        {
            return (this.TopLeft.X <= position.X && position.X <= this.BottomRight.X) && 
                (this.TopLeft.Z >= position.Z && position.Z >= this.BottomRight.Z);
        }

        public abstract void Update();
    }
}
