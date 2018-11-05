using Client.Net;
using MapHandler;
using System;

namespace Assets.Code.Game
{
    public class ClientPathfinder
    {
        public static void FindPath(Position location)
        {
            var start = new Position(0, 0);

            var startChunkX = start.X >> 4;
            var startChunkY = start.Y >> 4;

            var endChunkX = location.X >> 4;
            var endChunkY = location.Y >> 4;

            var chunkDistanceX = Math.Abs(startChunkX - endChunkX) + 1;
            var chunkDistanceY = Math.Abs(startChunkY - endChunkY) + 1;

            var maxChunk = new Position(Math.Max(startChunkX, endChunkX), Math.Max(startChunkY, endChunkY));
            var minChunk = new Position(Math.Min(startChunkX, endChunkX), Math.Min(startChunkY, endChunkY));

            var tileArray = new Int16[chunkDistanceX, chunkDistanceY];

            for (int x = minChunk.X; x <= maxChunk.X; x++)
            {
                for (int y = minChunk.Y; y <= maxChunk.Y; y++)
                {

                }
            }

        }
    }
}
