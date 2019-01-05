using MapHandler;
using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class MonsterPacket : BasePacket
    {
        public string MonsterUid;
        public Position Position;
        public string MonsterName;
        public int SpriteIndex;
        public int MoveSpeed;
        public int MAXHP = 10;
        public int HP = 10;
        public int Atk = 100;
        public int Def = 1;
        public int AtkSpeed = 5;

        // Will make the "smoke" spawning effect
        public bool SpawnAnimation;
    }
}
