using MapHandler;
using System;
using System.Collections.Generic;
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

        public static PassableMapResult GetPassableByteArray<ChunkType>(Position start, Position goal, Dictionary<string, ChunkType> chunks, Func<int, int, bool> PassableCheck) where ChunkType : Chunk
        {
            var minMaxChunks = GetMinMaxChunks(chunks);
            var minChunk = minMaxChunks[0];
            var maxChunk = minMaxChunks[1];

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
                    if(!chunks.ContainsKey(chunkX + "_" + chunkY))
                    {
                        continue;
                    }

                    Chunk c = chunks[chunkX + "_" + chunkY];

                    for (var tileX = 0; tileX <= 15; tileX++)
                    {
                        for (var tileY = 0; tileY <= 15; tileY++)
                        {
                            var tile = c.GetTile(tileX, tileY);

                            var mapTileX = chunkX * 16 + tileX;
                            var mapTileY = chunkY * 16 + tileY;

                            if (PassableCheck.Invoke(mapTileX, mapTileY))
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
