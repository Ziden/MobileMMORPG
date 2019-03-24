using Common.Networking.Packets;
using System;

namespace CommonCode.Networking.Packets
{
    [Serializable]
    public class PlayerPacket : BasePacket
    {
        public string UserId;
        public string Name;
        public int X;
        public int Y;
        public int Speed;
        public int BodySpriteIndex;
        public int HeadSpriteIndex;
        public int LegSpriteIndex;
        public int ChestSpriteIndex;
        public int Atk;
        public int Def;
        public int AtkSpeed;
        public int HP;
        public int MAXHP;
    }
}
