using Common.Entity;
using MapHandler;

namespace ServerCore.GameServer.Players.Evs
{
    public class EntityTargetEvent : IEvent
    {
        public LivingEntity Entity;
        public LivingEntity TargetedEntity;
    }
}
