using System;

namespace MapHandler
{
    public class TileProperties
    {
        public static Int16[] PassableTiles = new Int16[1]
        {
            1
        };

        public static bool IsPassable(short tileId)
        {
            foreach (var tile in PassableTiles)
                if (tile == tileId)
                    return true;
            return false;
        }
    }
}
