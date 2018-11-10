using CommonCode.Pathfinder;
using Pathfinder;
using System.Collections.Generic;
using System.Linq;

namespace MapHandler
{
    public class WorldMap<ChunkType> where ChunkType : Chunk
    {
        public Dictionary<string, byte[]> Tilesets = new Dictionary<string, byte[]>();

        public Dictionary<string, ChunkType> Chunks = new Dictionary<string, ChunkType>();

        public void AddChunk(ChunkType c)
        {
            Chunks.Add($"{c.x}_{c.y}", c);
        }

        public ChunkType GetChunk(int x, int y)
        {
            var key = $"{x}_{y}";
            if (!Chunks.ContainsKey(key))
                return null;
            return Chunks[key];
        }

        public static List<Position> FindPath(Position start, Position goal, Dictionary<string, Chunk> chunks)
        {

            var passableMapResult = PathfinderHelper.GetPassableByteArray(start, goal, chunks);
            var pathfinder = new PathFinderFast(passableMapResult.PassableMap);

            var offsetedStart = new Position(start.X + (passableMapResult.OffsetX * 16), start.Y + (passableMapResult.OffsetY * 16));
            var offsetedGoal = new Position(goal.X + (passableMapResult.OffsetX * 16), goal.Y + (passableMapResult.OffsetY * 16));

            var result = pathfinder.FindPath(offsetedStart, offsetedGoal);
            if (result == null)
            {
                return null;
            }
            result.Reverse();
            return result.Select(node => new Position(node.X - (passableMapResult.OffsetX * 16), node.Y - (passableMapResult.OffsetY * 16))).ToList();
        }

    }
}
