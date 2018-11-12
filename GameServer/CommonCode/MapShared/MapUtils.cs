using System;
using System.Collections.Generic;
using System.Text;

namespace MapHandler
{
   
    public class MapUtils
    {
        // Gets all positions in a squared radius
        public static List<Position> GetRadius(int x,int y, int range)
        {
            List<Position> list = new List<Position>();

            var currY = y + range;

            var rangeX = 0;

            while (currY >= y - range)
            {

                for(var currX = x - rangeX; currX <= x + rangeX; currX ++)
                {
                    list.Add(new Position(currX,  currY));
                }
                if (currY > y)
                    rangeX++;
                else
                    rangeX--;
                currY--;
            }

            return list;
        }

    }
}
