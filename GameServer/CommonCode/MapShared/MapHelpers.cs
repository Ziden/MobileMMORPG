using System;
namespace MapHandler
{
    public static class MapHelpers
    {

        public static int GetDistance(Position position, Position other)
        {
            return Math.Abs(other.X - position.X) + Math.Abs(other.Y - position.Y);
        }

        public static Position[] GetSquared1Radius(Position p)
        {
            return new Position[]
            {
                new Position(p.X + 1, p.Y),
                new Position(p.X, p.Y + 1),
                new Position(p.X + 1, p.Y + 1),
                new Position(p.X - 1, p.Y - 1),
                new Position(p.X, p.Y - 1),
                new Position(p.X - 1, p.Y),
                new Position(p.X - 1, p.Y - 1),
                new Position(p.X + 1, p.Y - 1),
                new Position(p.X + 1, p.Y - 1),
            };
        }

        public static Direction GetDirection(Position from, Position to)
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
