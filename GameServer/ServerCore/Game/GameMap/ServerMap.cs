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

        public EntityPositionsCache EntityPositions = new EntityPositionsCache();

        public void LoadAllSpawners()
        {
            foreach (var spawner in Spawners)
            {
                spawner.SpawnTick();
            }
        }

        public bool IsPassable(int x, int y)
        {
            if (!TileProperties.IsPassable(GetTile(x, y)))
            {
                return false;
            }
            if (!EntityPositions.IsVacant(new Position(x, y))) {
                return false;
            }
            return true;
        }
    }

    public class EntityPositionsCache : Dictionary<string, List<Entity>>
    {

        public bool IsVacant(Position pos)
        {
            var teste = this;
            var key = $"{pos.X}_{pos.Y}";
            return !ContainsKey(key);
        }

        public void RemoveEntity(Entity e, Position pos)
        {
            var key = $"{pos.X}_{pos.Y}";
            if (ContainsKey(key))
            {
                var entities = this[key];
                entities.Remove(e);
                if (entities.Count == 0)
                    Remove(key);
            }
        }

        public void AddEntity(Entity entity, Position pos)
        {
            var key = $"{pos.X}_{pos.Y}";
            if (!ContainsKey(key))
            {
                Add(key, new List<Entity>());
            }
            this[key].Add(entity);
        }
    }
}
