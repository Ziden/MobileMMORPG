using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class AssetsReadyPacket : BasePacket
    {
        public string UserId;
    }
}
