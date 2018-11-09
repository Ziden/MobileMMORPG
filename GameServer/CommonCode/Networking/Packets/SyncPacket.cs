using MapHandler;
using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class SyncPacket : BasePacket
    {
        public Position Position;
    }
}
