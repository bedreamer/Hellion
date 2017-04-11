using Ether.Network.Packets;
using Hellion.Core.Data;
using Hellion.Core.Structures;
using Hellion.World.Systems;
using System.Collections.Generic;
using System.Linq;
using EHelper = Ether.Network.Helpers;

namespace Hellion.World.Structures
{
    /// <summary>
    /// Implements the base class of a FlyFF world object.
    /// </summary>
    public abstract class WorldObject
    {
        /// <summary>
        /// Gets or sets the spawned state.
        /// </summary>
        public bool IsSpawned { get; set; }

        /// <summary>
        /// Gets or sets the object id.
        /// </summary>
        public int ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the object's model id.
        /// </summary>
        public int ModelId { get; set; }

        /// <summary>
        /// Gets or sets the object's model size.
        /// </summary>
        public short Size { get; set; }

        /// <summary>
        /// Gets or sets the object's map id where he is located.
        /// </summary>
        public int MapId { get; set; }

        /// <summary>
        /// Gets or sets the object's rotation angle.
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// Gets or sets the object's flying angle.
        /// </summary>
        public float AngleFly { get; set; }

        /// <summary>
        /// Gets or sets the object's flying rotation angle.
        /// </summary>
        public float TurnAngle { get; set; }

        /// <summary>
        /// Gets or sets the object's position in the world.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the object spawned list around him.
        /// </summary>
        public ICollection<WorldObject> SpawnedObjects { get; set; }

        /// <summary>
        /// Gets the object type.
        /// </summary>
        public virtual WorldObjectType Type
        {
            get { return WorldObjectType.Object; }
        }

        /// <summary>
        /// Creates a new <see cref="WorldObject"/> instance based on a model id.
        /// </summary>
        /// <param name="modelId">Object model id</param>
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

        /// <summary>
        /// Check if the current <see cref="WorldObject"/> can see an other <see cref="WorldObject"/>.
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns></returns>
        public bool CanSee(WorldObject otherObject)
        {
            return this.Position.IsInCircle(otherObject.Position, 75f);
        }

        /// <summary>
        /// Gets the list of spawned objects in a given range.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public IEnumerable<WorldObject> GetSpawnedObjectsAround(int range = 50)
        {
            return this.SpawnedObjects.Where(x => x.Position.IsInCircle(this.Position, range));
        }

        /// <summary>
        /// Gets an <see cref="WorldObject"/> in the spawned list by his object id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public T GetSpawnedObjectById<T>(int objectId) where T : WorldObject
        {
            return this.SpawnedObjects.Where(x => x.ObjectId == objectId).Cast<T>().FirstOrDefault();
        }

        /// <summary>
        /// Despawn a <see cref="WorldObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        public virtual void DespawnObject(WorldObject obj)
        {
            this.SpawnedObjects.Remove(obj);
        }

        /// <summary>
        /// Send a packet to every real Player around the <see cref="WorldObject"/>.
        /// </summary>
        /// <param name="packet"></param>
        public virtual void SendToVisible(NetPacketBase packet)
        {
            foreach (Player player in this.SpawnedObjects.Where(x => x is Player))
                player.Send(packet);
        }
    }
}
