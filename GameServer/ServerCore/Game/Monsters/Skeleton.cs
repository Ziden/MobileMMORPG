using CommonCode.EntityShared;
using ServerCore.Assets;
using ServerCore.Game.Entities;
using ServerCore.Game.Monsters.Behaviours;
using ServerCore.Game.Monsters.Behaviours.MoveBehaviours;
using ServerCore.Networking.PacketListeners;
using System;

namespace ServerCore.Game.Monsters
{
    public class Skeleton : Monster
    {
        public Skeleton()
        {
            this.Name = "Skeleton";
            this.MovementBehaviour = BehaviourPool.Get<RandomWalk>();
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
