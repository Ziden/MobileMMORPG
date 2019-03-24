using Common.Entity;

namespace ServerCore.GameServer.Players.Evs
{
    public class EntityAttackEvent : IEvent
    {
        public LivingEntity Attacker;
        public LivingEntity Defender;
    }
}
