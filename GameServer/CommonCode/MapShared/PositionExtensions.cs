using System;
using System.Collections.Generic;

namespace MapHandler
{
    public static class PositionExtensions
    {
        public static int GetDistance(this Position position, Position other)
        {
            return Math.Abs(other.X - position.X) + Math.Abs(other.Y - position.Y);
        }

        public static bool IsSameChunk(this Position a, Position b)
        {
            return a.X >> 4 == b.X >> 4 && a.Y >> 4 == b.Y >> 4;
        }

        // Gets all positions in a squared radius
        public static List<Position> GetRadius(this Position position, int range)
        {
            List<Position> list = new List<Position>();

            var x = position.X;
            var y = position.Y;

            var currY = y + range;

            var rangeX = 0;

            while (currY >= y - range)
            {

                for (var currX = x - rangeX; currX <= x + rangeX; currX++)
                {
                    list.Add(new Position(currX, currY));
                }
                if (currY > y)
                    rangeX++;
                else
                    rangeX--;
                currY--;
            }

            return list;
        }

        public static Position[] GetSquared3x3Around(this Position p)
        {
            return new Position[]
            {
                new Position(p.X + 1, p.Y),

                new Position(p.X, p.Y + 1),

                new Position(p.X + 1, p.Y + 1),

                new Position(p.X - 1, p.Y - 1),

                new Position(p.X, p.Y - 1),

                new Position(p.X - 1, p.Y),

                new Position(p.X, p.Y),

                new Position(p.X + 1, p.Y - 1),

                new Position(p.X - 1, p.Y + 1),
            };
        }

        public static Direction GetDirection(this Position from, Position to)
        {
            if (from.X < to.X)
                return Direction.LEFT;
            if (from.X > to.X)
                return Direction.RIGHT;
            if (from.Y > to.Y)
                return Direction.NORTH;
            if (from.Y < to.Y)
                return Direction.SOUTH;
            return Direction.NONE;
        }
    }
}
