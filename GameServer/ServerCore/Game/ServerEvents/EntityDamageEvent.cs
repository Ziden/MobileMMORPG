using Common.Entity;

namespace ServerCore.GameServer.Players.Evs
{
    public class EntityDamageEvent : IEvent
    {
        public LivingEntity Entity;
        public int Damage;
        public DamageCause DamageCause;
    }

    public enum DamageCause
    {
        ATTACK = 1
    }
}
