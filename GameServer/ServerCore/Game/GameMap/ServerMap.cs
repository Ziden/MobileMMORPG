using MapHandler;
using ServerCore.Game.Entities;
using ServerCore.Game.Monsters;
using System.Collections.Generic;

namespace ServerCore.Game.GameMap
{
    public class ServerMap : WorldMap<ServerChunk>
    {
        public Dictionary<string, byte[]> Tilesets = new Dictionary<string, byte[]>();

        public List<MonsterSpawner> Spawners = new List<MonsterSpawner>();

        public Dictionary<string, Monster> Monsters = new Dictionary<string, Monster>();

        public EntityPositions EntityPositions = new EntityPositions();

        public void LoadAllSpawners()
        {
            foreach (var spawner in Spawners)
            {
                spawner.SpawnTick();
            }
        }
    }

    public class EntityPositions : Dictionary<string, Entity>
    {
        public void RemoveEntity(Position pos)
        {
            var key = $"{pos.X}_{pos.Y}";
            if (this.ContainsKey(key))
            {
                this.Remove(key);
            }
        }

        public void AddEntity(Position position, Entity entity)
        {

        }
    }
}
