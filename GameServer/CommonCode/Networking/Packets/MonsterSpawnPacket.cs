using MapHandler;
using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class MonsterSpawnPacket : BasePacket
    {
        public string MonsterUid;
        public Position Position;
        public string MonsterName;
        public int SpriteIndex;
        public int MoveSpeed;

        // Will make the "smoke" spawning effect
        public bool SpawnAnimation;
    }
}
