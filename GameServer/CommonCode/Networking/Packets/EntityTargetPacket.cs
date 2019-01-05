using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class EntityTargetPacket : BasePacket
    {
        public string WhoUuid;
        public string TargetUuid;
    }
}
