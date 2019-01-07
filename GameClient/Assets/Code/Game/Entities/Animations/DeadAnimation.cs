using Assets.Code.AssetHandling.Sprites.Animations;
using Assets.Code.Game.Entities;
using MapHandler;

namespace Assets.Code.AssetHandling
{
    public class DeadAnimation : AnimationBase
    {

        public static readonly int HIT_FRAME = 8;

        public DeadAnimation(SpriteSheet sheet) : base(sheet) {
            this.AnimationTimeInSeconds = int.MaxValue;
        }

        public override AnimationResult Loop(Direction dir)
        {
            return new AnimationResult()
            {
                Sprite = this.SpriteSheet.Dead,
                OffsetX = 0,
                OffsetY = 0
            };
        }
    }
}
