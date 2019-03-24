using CommonCode.EntityShared;
using ServerCore.Assets;
using ServerCore.Game.Monsters.Behaviours;
using ServerCore.Game.Monsters.Behaviours.AggroBehaviours;
using ServerCore.Game.Monsters.Behaviours.MoveBehaviours;

namespace ServerCore.Game.Monsters
{
    public class Skeleton : Monster
    {
        public Skeleton()
        {
            this.Name = "Skeleton";
            this.MovementBehaviour = BehaviourPool.Get<LeftRightWalk>();
            this.AggroBehaviuor = BehaviourPool.Get<TargetBack>();
        }

        private static SpriteAsset _sprite = new SpriteAsset()
        {
            ImageName = DefaultAssets.SPR_MONTERS_1,
            SpriteRowIndex = 2
        };

        public override SpriteAsset GetSprite()
        {
            return _sprite;
        }
    }
}
