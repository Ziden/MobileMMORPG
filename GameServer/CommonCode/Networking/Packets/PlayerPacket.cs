using Common.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonCode.Networking.Packets
{
    [Serializable]
    public class PlayerPacket : BasePacket
    {
        public string UserId;
        public string Name;
        public int X;
        public int Y;
        public int SpriteIndex;
        public int Speed;
    }

}
