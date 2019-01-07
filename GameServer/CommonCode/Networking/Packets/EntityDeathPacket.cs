using Common.Networking.Packets;
using System;

namespace CommonCode.Networking.Packets
{
    [Serializable]
    public class EntityDeathPacket : BasePacket
    {
        public string EntityUID;
    }

}
