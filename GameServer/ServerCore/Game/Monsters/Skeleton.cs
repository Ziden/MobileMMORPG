using ServerCore.Game.Monsters.Behaviours;
using ServerCore.Game.Monsters.Behaviours.MoveBehaviours;
using System;

namespace ServerCore.Game.Monsters
{
    public class Skeleton : Monster
    {
        public Skeleton()
        {
            this.SpriteIndex = 1;
            this.Name = "Skeleton";
            this.MovementBehaviour = BehaviourPool.Get<RandomWalk>();
        }
    }
}
