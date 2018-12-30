using Common.Entity;
using MapHandler;
using ServerCore.Game.Entities;

namespace ServerCore.GameServer.Players.Evs
{
    public class EntitySpawnEvent : IEvent
    {
        public Entity Entity;
        public Position Position;
    }
}
