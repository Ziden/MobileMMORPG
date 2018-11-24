using CommonCode.Pathfinder;
using Pathfinder;
using System;
using System.Collections.Generic;

namespace MapHandler
{
    public class WorldMap<ChunkType> where ChunkType : Chunk
    {
        public Dictionary<string, ChunkType> Chunks = new Dictionary<string, ChunkType>();

        public void AddChunk(ChunkType c)
        {
            Chunks.Add($"{c.x}_{c.y}", c);
        }

        public Int16 GetTile(int x, int y)
        {
            var chunkX = x >> 4;
            var chunkY = y >> 4;
            var chunk = GetChunk(chunkX, chunkY);
            if (chunk == null)
                return -1;
            else
            {
                var chunkTileX = x - (chunkX << 4);
                var chunkTileY = y - (chunkY << 4);
                return chunk.GetTile(chunkTileX, chunkTileY);
            }
        }

        public ChunkType GetChunk(int x, int y)
        {
            var key = $"{x}_{y}";
            if (!Chunks.ContainsKey(key))
                return null;
            return Chunks[key];
        }

        public bool IsPassable(int x, int y)
        {
            return TileProperties.IsPassable(GetTile(x, y));

        }

        public static List<Position> FindPath(Position start, Position goal, Dictionary<string, Chunk> chunks)
        {

            var passableMapResult = PathfinderHelper.GetPassableByteArray(start, goal, chunks);
            var pathfinder = new PathFinder(passableMapResult.PassableMap);

            var offsetedStart = new Position(start.X + (passableMapResult.OffsetX * 16), start.Y + (passableMapResult.OffsetY * 16));
            var offsetedGoal = new Position(goal.X + (passableMapResult.OffsetX * 16), goal.Y + (passableMapResult.OffsetY * 16));

            var result = pathfinder.FindPath(offsetedStart, offsetedGoal);
            if (result == null)
            {
                return null;
            }

            List<Position> returned = new List<Position>();

            foreach (var node in result)
            {
                returned.Add(new Position(node.X - (passableMapResult.OffsetX * 16), node.Y - (passableMapResult.OffsetY * 16)));
            }

            return returned;
        }

    }
}
