using MapHandler;
using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class PlayerMovePacket : BasePacket
    {
        public string UserId;
        public Position From;
        public Position To;
    }
}
