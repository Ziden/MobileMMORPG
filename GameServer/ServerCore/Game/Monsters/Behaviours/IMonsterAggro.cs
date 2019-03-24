using Common.Entity;

namespace ServerCore.Game.Monsters.Behaviours
{
    public interface IMonsterAggro : IMonsterBehaviour
    {
        void OnBeingTargeted(Monster monster, LivingEntity targetting);
    }
}
