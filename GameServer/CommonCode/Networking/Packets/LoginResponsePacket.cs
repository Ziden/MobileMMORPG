using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Networking.Packets
{
    [Serializable]
    public class LoginResponsePacket : BasePacket
    {
        public string SessionId;
        public string UserId;

        public int xLocation;
        public int yLocation;
    }
}
