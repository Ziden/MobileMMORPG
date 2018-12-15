using MapHandler;

namespace ServerCore.Game.Entities
{
    public abstract class Entity
    {
        public string UID;
        public Position Position;
        public Position LastPosition;
        public EntityType EntityType;
    }
}
