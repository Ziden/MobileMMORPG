using MapHandler;
using System;
using Common.Entity;

namespace MapHandler
{
    public class MapTile
    {
        public string uid = Guid.NewGuid().ToString();
        public Position Position;
        public Int16 TileId;
        public Entity Occupator;

        public MapTile(Position Position, Int16 TileId)
        {
            this.Position = Position;
            this.TileId = TileId;
        }
    }
}
