using System;namespace MapHandler
{
    public static class MapHelpers
    {

        public static int GetDistance(Position position, Position other)
        {
            return Math.Abs(other.X - position.X) + Math.Abs(other.Y - position.Y);
        }

        public static Direction GetDirection(Position from, Position to)
        {
            if(from.X < to.X)
                return Direction.LEFT;
            if (from.X > to.X)
                return Direction.RIGHT;
            if (from.Y > to.Y)
                return Direction.SOUTH;
            if (from.Y < to.Y)
                return Direction.NORTH;
            return Direction.NONE;
        }
    }
}
