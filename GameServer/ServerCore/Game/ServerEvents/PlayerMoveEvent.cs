using MapHandler;
using ServerCore.Game.Entities;

namespace ServerCore.GameServer.Players.Evs
{
    public class EntityMoveEvent : IEvent
    {
        public Entity Entity;
        public Position To;

        public bool IsCancelled = false;

    }
}
