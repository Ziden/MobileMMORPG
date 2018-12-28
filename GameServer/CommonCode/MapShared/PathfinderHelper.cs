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

        public byte GetPassableFromTilePosition(int tileX, int tileY)
        {
            return PassableMap[tileX + OffsetX * 16, tileY + OffsetY * 16];
        }
    }

    // Big Chunks of Ugly Code
    public static class PathfinderHelper
    {
        public static Position FindClosestPassable<ChunkType>(this WorldMap<ChunkType> map, Position origin, Position goal) where ChunkType : Chunk
        {
            var bestDirection = goal.GetDirection(origin);
            var supposedBestTile = goal.Get(bestDirection);
            if(map.IsPassable(supposedBestTile.X, supposedBestTile.Y))
            {
                return supposedBestTile;
            } else
            {
                foreach(var pos in goal.GetRadius(1, includeSelf:false))
                {
                    if (pos == supposedBestTile)
                        continue; // we already checked it
                    if(map.IsPassable(pos.X, pos.Y))
                    {
                        return pos;
                    }
                }
            }
            return null;
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
