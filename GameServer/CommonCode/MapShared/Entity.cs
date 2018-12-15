using MapHandler;
using ServerCore.Game.Entities;

namespace MapHandler
{
    public abstract class Entity
    {
        public string UID;
        public Position Position;
        public Position LastPosition;
        public EntityType EntityType;
    }
}
