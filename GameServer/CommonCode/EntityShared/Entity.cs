using MapHandler;
using System;

namespace Common.Entity
{
    public abstract class Entity
    {
        public string UID;
        public Position Position;
        public Position LastPosition = new Position(0, 0);
        public EntityType EntityType;
        public int MoveSpeed = 5;
    }
}
