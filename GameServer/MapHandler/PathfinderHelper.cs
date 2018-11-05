using MapHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonCode.Pathfinder
{
    public class PassableMapResult
    {
        public byte[,] PassableMap;
        public int OffsetX;
        public int OffsetY;
    }

    public class PathfinderHelper
    {
        public static PassableMapResult GetPassableByteArray(Position start, Position goal, Dictionary<string, Chunk> chunks)
        {
            var startChunkX = start.X >> 4;
            var startChunkY = start.Y >> 4;

            var endChunkX = goal.X >> 4;
            var endChunkY = goal.Y >> 4;

            var chunkDistanceX = Math.Abs(startChunkX - endChunkX) + 1;
            var chunkDistanceY = Math.Abs(startChunkY - endChunkY) + 1;

            var maxChunk = new Position(Math.Max(startChunkX, endChunkX), Math.Max(startChunkY, endChunkY));
            var minChunk = new Position(Math.Min(startChunkX, endChunkX), Math.Min(startChunkY, endChunkY));

            int gridSize = Math.Max(chunkDistanceX, chunkDistanceY);

            byte[,] passableMap = new byte[gridSize * 16, gridSize * 16];

            var xOffset = 0;
            if (minChunk.X < 0)
            {
                xOffset -= minChunk.X;
            }

            var yOffset = 0;
            if (minChunk.Y < 0)
            {
                yOffset -= minChunk.Y;
            }

            for (int chunkX = minChunk.X; chunkX <= maxChunk.X; chunkX++)
            {
                for (int chunkY = minChunk.Y; chunkY <= maxChunk.Y; chunkY++)
                {
                    Chunk c = chunks[chunkX + "_" + chunkY];

                    for (var tileX = 0; tileX <= 15; tileX++)
                    {
                        for (var tileY = 0; tileY <= 15; tileY++)
                        {
                            var tile = c.GetTile(tileX, tileY);
                            if (TileProperties.PassableTiles.Any(tileId => tileId == tile))
                            {
                                passableMap[(chunkX + xOffset) * 16 + tileX, (chunkY + yOffset) * 16 + tileY] = 1;
                            }
                            else
                            {
                                passableMap[(chunkX + xOffset) * 16 + tileX, (chunkY + yOffset) * 16 + tileY] = 0;
                            }
                        }
                    }
                }
            }
            return new PassableMapResult()
            {
                OffsetX = xOffset,
                OffsetY = yOffset,
                PassableMap = passableMap
            };
        }
    }
}
