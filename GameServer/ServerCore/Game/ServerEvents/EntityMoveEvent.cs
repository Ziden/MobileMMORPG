using Common.Entity;
using MapHandler;

namespace ServerCore.GameServer.Players.Evs
{
    public class EntityMoveEvent : IEvent
    {
        public Entity Entity;
        public Position To;
    }
}
