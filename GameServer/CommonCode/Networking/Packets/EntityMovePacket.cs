using MapHandler;
using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class EntityMovePacket : BasePacket
    {
        public string UID;
        public Position From;
        public Position To;
    }
}
