using MapHandler;
using ServerCore.Game.Entities;
using ServerCore.GameServer.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerCore.Game.GameMap
{
    public class ServerChunk : Chunk
    {
        // Packet Data
        public short[,] TilePacketData = new short[SIZE, SIZE];

        public void BuildChunkPacketData()
        {
            for (int x = 0; x < SIZE; x++)
            {
                for (int y = 0; y < SIZE; y++)
                {
                    TilePacketData[x, y] = Tiles[x, y].TileId;
                }
            }
        }
    }
}