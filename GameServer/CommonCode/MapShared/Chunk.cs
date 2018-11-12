using System;

namespace MapHandler
{
    public class Chunk
    {
        public static int SIZE = 16;

        public int x;
        public int y;

        private Int16[,] _chunkTiles = new Int16[SIZE, SIZE];

        public Int16[,] GetData() => _chunkTiles;

        public void SetTile(int x, int y, Int16 tile)
        {
            _chunkTiles[x, y] = tile;
        }

        public Int16 GetTile(int x, int y)
        {
            return _chunkTiles[x, y];
        }
    }
}

