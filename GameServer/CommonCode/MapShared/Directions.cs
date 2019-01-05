using System;
using System.Collections.Generic;
using System.Text;

namespace MapHandler
{
    public enum Direction
    {
        SOUTH = 2,
        NORTH = 8,
        LEFT = 4,
        RIGHT = 6,
        NONE = 5
    }

    public static class DirectionExtensions
    {
        public static Direction Rotate90(this Direction dir)
        {
            switch(dir)
            {
                case Direction.RIGHT:
                    return Direction.NORTH;
                case Direction.NORTH:
                    return Direction.LEFT;
                case Direction.LEFT:
                    return Direction.SOUTH;
                case Direction.SOUTH:
                    return Direction.RIGHT;
            }
            return Direction.NONE;
        }
    }
}
