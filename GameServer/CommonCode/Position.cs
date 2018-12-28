using System;

namespace MapHandler
{
    [Serializable]
    public class Position
    {
        public int X;
        public int Y;

        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public new string ToString()
        {
            return X + "_" + Y;
        }

        public static bool operator ==(Position p1, Position p2)
        {
            return (p1?.X == p2?.X && p1?.Y == p2?.Y);
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return (p1?.X != p2?.X || p1?.Y != p2?.Y);
        }

    }

}
