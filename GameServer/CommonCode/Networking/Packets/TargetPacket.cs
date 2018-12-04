using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class TargetPacket : BasePacket
    {
        public string WhoUuid;
        public string TargetUuid;
    }
}
