using Hellion.Core;
using Hellion.Core.Data;
using Hellion.Core.Data.Resources;
using Hellion.World.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hellion.World.Systems
{
    public class Map
    {
        private const int RI_TRIGGER = 10;
        private const int RI_ATTRIBUTE = 11;
        private const int RI_BEGIN = 12;
        private const int RI_REVIVAL = 13;
        private const int RI_STRUCTURE = 14;
        private const int RI_PLACE = 15;

        public const float MaxHeight = 270f;

        private Thread updateThread;

        private ICollection<Player> players;
        private object syncLockClient = new object();

        private ICollection<Npc> npcs;
        private object syncLockNpc = new object();

        private ICollection<Monster> monsters;
        private object syncLockMonster = new object();

        private ICollection<Region> regions;

        /// <summary>
        /// Gets the map id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the map name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the map width.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the map length.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Get the map heights.
        /// </summary>
        public float[] Heights { get; private set; }

        /// <summary>
        /// Creates a new Map instance with a name and id.
        /// </summary>
        /// <param name="id">Id of the map</param>
        /// <param name="mapName">Name of the map</param>
        public Map(int id, string mapName)
        {
            this.Id = id;
            this.Name = mapName;
            this.regions = new List<Region>();
            this.players = new HashSet<Player>();
            this.npcs = new HashSet<Npc>();
            this.monsters = new HashSet<Monster>();
        }

        /// <summary>
        /// Load a map.
        /// </summary>
        public void Load()
        {
            string mapPath = Path.Combine(Global.DataPath, "maps", this.Name);
            string wldMapPath = Path.Combine(mapPath, this.Name + ".wld");
            string dyoMapPath = Path.Combine(mapPath, this.Name + ".dyo");
            string rgnMapPath = Path.Combine(mapPath, this.Name + ".rgn");

            // Load .wld
            byte[] wldFileData = File.ReadAllBytes(wldMapPath);
            var wld = new WldFile(wldFileData);
            wld.Read();

            this.Width = wld.Width;
            this.Length = wld.Length;

            // Load .dyo
            byte[] dyoFileData = File.ReadAllBytes(dyoMapPath);
            var dyo = new DyoFile(dyoFileData);
            dyo.Read();

            foreach (NpcDyoElement dyoElement in dyo.Elements.Where(d => d is NpcDyoElement))
                this.npcs.Add(Npc.CreateFromDyo(dyoElement, this.Id));

            // Load .rgn
            byte[] rgnFileData = File.ReadAllBytes(rgnMapPath);
            var rgn = new RgnFile(rgnFileData);
            rgn.Read();

            foreach (RgnRespawn7 rgnElement in rgn.Elements.Where(r => r is RgnRespawn7))
            {
                var respawner = new RespawnerRegion(rgnElement.Position, rgnElement.StartPosition, rgnElement.EndPosition, rgnElement.Time);

                if (rgnElement.Type == 5)
                {
                    for (int i = 0; i < rgnElement.Count; ++i)
                    {
                        var monster = new Monster(rgnElement.Model, this.Id, respawner);

                        this.monsters.Add(monster);
                        monster.IsSpawned = true;
                    }
                }

                this.regions.Add(respawner);
            }

            foreach (RgnRegion3 rgnElement in rgn.Elements.Where(r => r is RgnRegion3))
            {
                switch (rgnElement.Index)
                {
                    case RI_REVIVAL:
                        // Load revival region
                        break;
                }
            }

            // Load .lnd
            //this.Heights = new float[wld.Length * wld.Width];

            //for (int i = 0; i < wld.Length; ++i)
            //    for (int j = 0; j < wld.Width; ++j)
            //    {
            //        // build lnd file format
            //        // read lnd
            //    }
        }

        /// <summary>
        /// Start the map update thread.
        /// </summary>
        public void StartThread()
        {
            if (this.updateThread == null)
            {
                this.updateThread = new Thread(this.Update);
                this.updateThread.Start();
            }
        }

        /// <summary>
        /// Adds a new <see cref="WorldObject"/> to this map.
        /// </summary>
        /// <param name="worldObject"></param>
        public void AddObject(WorldObject worldObject)
        {
            worldObject.IsSpawned = true;

            if (worldObject is Player)
            {
                lock (syncLockClient)
                    this.players.Add(worldObject as Player);
            }

            if (worldObject is Monster)
            {
                lock (syncLockMonster)
                    this.monsters.Add(worldObject as Monster);
            }
        }

        /// <summary>
        /// Remove a <see cref="WorldObject"/> from this map and notify all the players spawned around him.
        /// </summary>
        /// <param name="worldObject"></param>
        public void RemoveObject(WorldObject worldObject)
        {
            if (worldObject is Player)
            {
                var player = worldObject as Player;

                lock (syncLockClient)
                {
                    this.players.Remove(player);
                    foreach (var otherPlayer in this.players)
                        otherPlayer.DespawnObject(player);
                }
            }
            else if (worldObject is Monster)
            {
                var monster = worldObject as Monster;

                lock (syncLockMonster)
                {
                    this.monsters.Remove(monster);
                    foreach (var player in monster.SpawnedObjects.Where(o => o is Player))
                        player.DespawnObject(monster);
                }
            }

            worldObject.IsSpawned = false;
        }

        /// <summary>
        /// Update the map objects.
        /// </summary>
        public void Update()
        {
            while (true)
            {
                this.UpdatePlayers();
                this.UpdateMonsters();
                this.UpdateNpc();

                foreach (var region in this.regions)
                    region.Update();

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Update the player visibility with other players.
        /// </summary>
        private void UpdatePlayers()
        {
            lock (syncLockClient)
            {
                foreach (Player player in this.players)
                {
                    if (!player.IsSpawned)
                        continue;

                    player.Update();
                    foreach (WorldObject obj in this.players)
                    {
                        if (!obj.IsSpawned || player.GetHashCode() == obj.GetHashCode())
                            continue;

                        if (player.CanSee(obj))
                        {
                            if (!player.SpawnedObjects.Contains(obj))
                                player.SpawnObject(obj);
                        }
                        else
                            player.DespawnObject(obj);
                    }

                    lock (syncLockNpc)
                    {
                        foreach (var npc in this.npcs)
                        {
                            if (player.CanSee(npc))
                            {
                                if (!player.SpawnedObjects.Contains(npc))
                                    player.SpawnObject(npc);
                            }
                            else
                                player.DespawnObject(npc);
                        }
                    }

                    lock (syncLockMonster)
                    {
                        foreach (var monster in this.monsters)
                        {
                            if (player.CanSee(monster) && monster.IsSpawned)
                            {
                                if (!player.SpawnedObjects.Contains(monster))
                                    player.SpawnObject(monster);
                            }
                            else
                                player.DespawnObject(monster);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update the monsters and their visibility.
        /// </summary>
        private void UpdateMonsters()
        {
            lock (syncLockMonster)
            {
                foreach (var monster in this.monsters)
                {
                    monster.Update();

                    lock (syncLockClient)
                    {
                        foreach (var player in this.players)
                        {
                            if (monster.CanSee(player) && monster.IsSpawned)
                            {
                                if (!monster.SpawnedObjects.Contains(player))
                                    monster.SpawnedObjects.Add(player);
                            }
                            else
                                monster.DespawnObject(player);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update the npcs.
        /// </summary>
        private void UpdateNpc()
        {
            lock (syncLockNpc)
            {
                foreach (var npc in this.npcs)
                {
                    npc.Update();

                    lock (syncLockClient)
                    {
                        foreach (var player in this.players)
                        {
                            if (npc.CanSee(player))
                            {
                                if (!npc.SpawnedObjects.Contains(player))
                                    npc.SpawnedObjects.Add(player);
                            }
                            else
                                npc.DespawnObject(player);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update all the regions.
        /// </summary>
        private void UpdateRegions()
        {
        }
    }
}
