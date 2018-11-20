using MapHandler;
using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class MonsterSpawnPacket : BasePacket
    {
        public int TileX;
        public int TileY;
        public string MonsterUid;
        public Position Position;
        public string MonsterName;
        public int SpriteIndex;

        // Will make the "smoke" spawning effect
        public bool SpawnAnimation;
    }
}
