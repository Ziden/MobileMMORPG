using MapHandler;
using System.Collections.Generic;

namespace ServerCore.Game.GameMap
{
    public class ServerMap : WorldMap<ServerChunk>
    {
        public Dictionary<string, byte[]> Tilesets = new Dictionary<string, byte[]>();

        public List<MonsterSpawner> Spawners = new List<MonsterSpawner>();

        public void LoadAllSpawners()
        {
            foreach (var spawner in Spawners)
            {
                spawner.SpawnTick();
            }
        }
    }
}
