using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Networking.Packets
{
    [Serializable]
    public class DialogPacket : BasePacket
    {
        public string Message;
    }
}
