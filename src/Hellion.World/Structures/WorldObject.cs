using Ether.Network.Packets;
using Hellion.Core.Data;
using Hellion.Core.Network;
using Hellion.Core.Structures;
using Hellion.World.Systems;
using System.Collections.Generic;
using System.Linq;
using EHelper = Ether.Network.Helpers;

namespace Hellion.World.Structures
{
    public abstract class WorldObject
    {
        public bool IsSpawned { get; set; }

        public int ObjectId { get; set; }

        public int ModelId { get; set; }

        public short Size { get; set; }

        public int MapId { get; set; }

        public float Angle { get; set; }

        public float AngleFly { get; set; }

        public float TurnAngle { get; set; }

        public Vector3 Position { get; set; }

        public ICollection<WorldObject> SpawnedObjects { get; set; }

        public virtual WorldObjectType Type
        {
            get { return WorldObjectType.Object; }
        }

        public WorldObject(int modelId)
        {
            this.ObjectId = EHelper.Helper.GenerateUniqueId();
            this.ModelId = modelId;
            this.Size = 100;
            this.MapId = -1;
            this.Position = new Vector3();
            this.Angle = 0;
            this.SpawnedObjects = new List<WorldObject>();
        }

        public bool CanSee(WorldObject otherObject)
        {
            return this.Position.IsInCircle(otherObject.Position, 75f);
        }

        public IEnumerable<WorldObject> GetSpawnedObjectsAround(int range = 50)
        {
            return this.SpawnedObjects.Where(x => x.Position.IsInCircle(this.Position, range));
        }

        public T GetSpawnedObjectById<T>(int objectId) where T : WorldObject
        {
            return this.SpawnedObjects.Where(x => x.ObjectId == objectId).Cast<T>().FirstOrDefault();
        }

        public virtual void DespawnObject(WorldObject obj)
        {
            this.SpawnedObjects.Remove(obj);
        }

        public virtual void SendToVisible(NetPacketBase packet)
        {
            foreach (Player player in this.SpawnedObjects.Where(x => x is Player))
                player.Send(packet);
        }
    }
}
