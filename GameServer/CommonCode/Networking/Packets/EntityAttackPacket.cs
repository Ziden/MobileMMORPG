using Common.Networking.Packets;
using System;

namespace CommonCode.Networking.Packets
{
    [Serializable]
    public class EntityAttackPacket : BasePacket
    {
        public string AttackerUID;
        public string DefenderUID;
        public int Damage;
    }

}
