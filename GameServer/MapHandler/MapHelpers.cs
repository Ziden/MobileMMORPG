using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapHandler
{
    public static class MapHelpers
    {
        public static Direction GetDirection(this Position from, Position to)
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
