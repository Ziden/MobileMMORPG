using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class PingPacket : BasePacket
    {
        public string sendDate;
        public string recieveDate;
    }
}
