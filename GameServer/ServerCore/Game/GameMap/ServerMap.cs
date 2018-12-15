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

        public void LoadAllSpawners()
        {
            foreach (var spawner in Spawners)
            {
                spawner.SpawnTick();
            }
        }
    }

    
}
