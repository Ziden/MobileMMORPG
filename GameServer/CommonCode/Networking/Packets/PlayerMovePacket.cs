using MapHandler;
using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class PlayerMovePacket : BasePacket
    {
        public Position From;
        public Position To;
    }
}
