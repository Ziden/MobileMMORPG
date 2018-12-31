using Assets.Code.AssetHandling.Sprites.Animations;
using Assets.Code.Game.Entities;
using MapHandler;

namespace Assets.Code.AssetHandling
{
    public class AttackAnimation : AnimationBase
    {

        public AttackAnimation(SpriteSheet sheet) : base(sheet) {
            this.AnimationTimeInSeconds = 0.06f;
        }

        public override AnimationResult Loop(Direction dir)
        {
            CurrentFrame++;

            float offset = 0;

            var spriteFrame = 0;
            var sriteDir = dir;

            // preparing (going back)
            if (CurrentFrame < 4)
            {
                offset = 0.005f * CurrentFrame;
                spriteFrame = CurrentFrame > 2 ? 2 : CurrentFrame;
            }
            // iddle
            else if (CurrentFrame < 6)
            { 
                offset = 0.02f;
                spriteFrame = 2;
            }
            // going forward
            else if (CurrentFrame < 8)
            {
                if(CurrentFrame==6)
                    offset = -0.00f;
                else
                    offset = -0.02f;
                spriteFrame = 8 - CurrentFrame;
            }
            else
            {
                Reset();
                IsOver = true;
                spriteFrame = 0;
            }

            var offsetX = 0f;
            var offsetY = 0f;

            if (dir == Direction.LEFT)
                offsetX = -offset;
            else if (dir == Direction.RIGHT)
                offsetX = offset;
            else if (dir == Direction.SOUTH)
                offsetY = offset;
            else if (dir == Direction.NORTH)
                offsetY = -offset;

                return new AnimationResult()
            {
                Sprite = this.SpriteSheet.GetSheet(sriteDir)[spriteFrame],
                OffsetX = offsetX,
                OffsetY = offsetY
            };
        }
    }
}
