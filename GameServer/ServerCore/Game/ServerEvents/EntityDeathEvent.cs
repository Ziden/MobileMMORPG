using Common.Entity;

namespace ServerCore.GameServer.Players.Evs
{
    public class EntityDeathEvent : IEvent
    {
        public LivingEntity Entity;
    }

    public enum DeathCause
    {
        DAMAGE = 1
    }
}
