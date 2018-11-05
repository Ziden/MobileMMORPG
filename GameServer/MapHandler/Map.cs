using CommonCode.Pathfinder;
using Pathfinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapHandler
{
    public class Map
    {
        public Dictionary<string, byte[]> Tilesets = new Dictionary<string, byte[]>();

        public Dictionary<string, Chunk> Chunks = new Dictionary<string, Chunk>();

        public void AddChunk(Chunk c)
        {
            Chunks.Add($"{c.x}_{c.y}", c);
        }

        public Chunk GetChunk(int x, int y)
        {
            return Chunks[$"{x}_{y}"];
        }

        public static List<Position> FindPath(Position start, Position goal, Dictionary<string, Chunk> chunks)
        {

            var passableMapResult = PathfinderHelper.GetPassableByteArray(start, goal, chunks);
            var pathfinder = new PathFinderFast(passableMapResult.PassableMap);

            var offsetedStart = new Position(start.X + (passableMapResult.OffsetX * 16), start.Y + (passableMapResult.OffsetY * 16));
            var offsetedGoal = new Position(goal.X + (passableMapResult.OffsetX * 16), goal.Y + (passableMapResult.OffsetY * 16));

            var result = pathfinder.FindPath(offsetedStart, offsetedGoal);
            if(result == null)
            {
                return null;
            }
            result.Reverse();
            return result.Select(node => new Position(node.X - (passableMapResult.OffsetX * 16), node.Y - (passableMapResult.OffsetY * 16))).ToList();
        }

    }
}
