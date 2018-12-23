using MapHandler;
using System;
using System.Collections.Generic;

namespace CommonCode.Pathfinder
{
    public class PassableMapResult
    {
        public byte[,] PassableMap;
        public int OffsetX;
        public int OffsetY;
    }

    public static class PathfinderHelper
    {

        private static Position [] GetMinMaxChunks<ChunkType>(Dictionary<string, ChunkType> chunks) where ChunkType : Chunk
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;

            int maxX = int.MinValue;
            int maxY = int.MinValue;

            foreach(var chunk in chunks.Values)
            {
                if (chunk.x < minX)
                    minX = chunk.x;
                if (chunk.x > maxX)
                    maxX = chunk.x;
                if (chunk.y < minY)
                    minY = chunk.y;
                if (chunk.y > maxY)
                    maxY = chunk.y;
            }
            return new Position[] { new Position(minX, minY), new Position(maxX, maxY) };
        }

        public static byte[,] GetSubSquare(this byte [,] array, int chunkX, int chunkY)
        {
            var initialX = chunkX * 16;
            var initialY = chunkY * 16;

            byte[,] finalArray = new byte[16, 16];
            for(int x = 0; x <  16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    finalArray[x, y] = array[initialX + x, initialY + y];
                }
            }
            return finalArray;
        }

        public static string ToPrettyString(this byte[,] rawNodes)
        {
            int rowLength = rawNodes.GetLength(0);
            int colLength = rawNodes.GetLength(1);
            string arrayString = "";
            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    arrayString += string.Format("{0} ", rawNodes[i, j]);
                }
                arrayString += System.Environment.NewLine;
            }
            return arrayString;
        } 

        // This is a tricky function. It generates a byte array of [3][3] chunk area (48 tiles) to run the pathfinding algorithm
        public static PassableMapResult GetPassableByteArray<ChunkType>(this WorldMap<ChunkType> map, Position start, Position goal) where ChunkType : Chunk
        {
            var startChunkX = start.X >> 4;
            var startChunkY = start.Y >> 4;

            var minChunk = new Position(startChunkX - 1, startChunkY - 1);
            var maxChunk = new Position(startChunkX + 1, startChunkY + 1);

            var chunkDistanceX = 3; 
            var chunkDistanceY = 3;

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
                    if(!map.Chunks.ContainsKey(chunkX + "_" + chunkY))
                    {
                        continue;
                    }

                    Chunk c = map.Chunks[chunkX + "_" + chunkY];

                    for (var tileX = 0; tileX <= 15; tileX++)
                    {
                        for (var tileY = 0; tileY <= 15; tileY++)
                        {
                            var tile = c.Tiles[tileX, tileY].TileId;

                            var mapTileX = chunkX * 16 + tileX;
                            var mapTileY = chunkY * 16 + tileY;

                            if (map.IsPassable(mapTileX, mapTileY))
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
