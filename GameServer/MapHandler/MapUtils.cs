using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapHandler
{
   
    public class MapUtils
    {
        // Gets all positions in a squared radius
        public static List<Position> GetRadius(Position start, int range)
        {
            List<Position> list = new List<Position>();

            var currY = start.Y + range;

            var rangeX = 0;

            while (currY >= start.Y - range)
            {

                for(var currX = start.X - rangeX; currX <= start.X + rangeX; currX ++)
                {
                    list.Add(new Position(currX,  currY));
                }
                if (currY > start.Y)
                    rangeX++;
                else
                    rangeX--;
                currY--;
            }

            return list;
        }

    }
}
