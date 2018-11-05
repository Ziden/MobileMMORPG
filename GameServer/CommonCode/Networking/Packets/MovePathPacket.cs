using MapHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Networking.Packets
{
    [Serializable]
    public class MovePathPacket : BasePacket
    {
        public List<Position> Path;
    }
}
