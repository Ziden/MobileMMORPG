using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class DialogPacket : BasePacket
    {
        public string Title;
        public string Message;
    }
}
