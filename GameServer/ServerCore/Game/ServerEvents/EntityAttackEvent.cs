using Common.Entity;
using MapHandler;
using ServerCore.Game.Entities;

namespace ServerCore.GameServer.Players.Evs
{
    public class EntityAttackEvent : IEvent
    {
        public LivingEntity Attacker;
        public LivingEntity Defender;
    }
}
