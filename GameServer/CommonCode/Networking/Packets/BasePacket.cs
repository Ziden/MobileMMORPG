
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Networking.Packets
{
    [Serializable]
    public class BasePacket
    {
        [NonSerialized]
        public string ClientId;
    }
}
