using System;
using System.Collections.Generic;
using System.Linq;

namespace MapHandler
{
    public static class PositionExtensions
    {
        private static double Angle90 = Math.PI / 2;
        private static double Angle180 = Math.PI;
        private static double Angle270 = -Math.PI / 2;
        private static double Angle360 = -Math.PI;
        private static double Angle0 = 0;
        private static double[] Angles = new double[] { Angle90, Angle180, Angle270, Angle360, Angle0 };

        public static int GetDistance(this Position position, Position other)
        {
            return Math.Abs(other.X - position.X) + Math.Abs(other.Y - position.Y);
        }

        public static bool IsSameChunk(this Position a, Position b)
        {
            return a.X >> 4 == b.X >> 4 && a.Y >> 4 == b.Y >> 4;
        }

        // Gets all positions in a squared radius
        public static List<Position> GetRadius(this Position position, int range, bool includeSelf = true)
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
                    if (currX == position.X && currY == position.Y && !includeSelf)
                        continue;
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
            var angle = from.GetAngleRadiansTo(to);

            var closestAngle = Angles
                .Aggregate((x, y) => Math.Abs(x - angle) < Math.Abs(y - angle) ? x : y);

            if (closestAngle==Angle180)
                return Direction.LEFT;
            if (closestAngle==Angle360 || closestAngle == Angle0)
                return Direction.RIGHT;
            if (closestAngle == Angle90)
                return Direction.NORTH;
            if (closestAngle == Angle270)
                return Direction.SOUTH;
            return Direction.NONE;
        }

        public static double GetAngleRadiansTo(this Position pos1, Position pos2)
        {
            return Math.Atan2(pos2.Y - pos1.Y, pos2.X - pos1.X);
        }

        public static Position Get(this Position from, Direction dir)
        {
            if (dir == Direction.LEFT)
                return new Position(from.X - 1, from.Y);
            else if(dir == Direction.RIGHT)
                return new Position(from.X + 1, from.Y);
            else if (dir == Direction.NORTH)
                return new Position(from.X, from.Y + 1);
            else if (dir == Direction.RIGHT)
                return new Position(from.X, from.Y - 1);
            else return from;
        }
    }
}
