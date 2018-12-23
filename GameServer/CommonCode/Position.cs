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

        public string ToString()
        {
            return X + "_" + Y;
        }
        
    }

}
