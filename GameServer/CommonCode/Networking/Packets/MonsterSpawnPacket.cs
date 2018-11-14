using MapHandler;
using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class MonsterSpawnPacket : BasePacket
    {

        public Position Position;
        public string MonsterName;
        public int SpriteIndex;

    }
}
