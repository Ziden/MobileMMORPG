using System;

namespace Common.Networking.Packets
{
    [Serializable]
    public class ChunkPacket : BasePacket
    {
        public int X;
        public int Y;
        public Int16[,] ChunkData;
    }
}
