using Assets.Code.AssetHandling.Sprites.Animations;
using Assets.Code.Game.Entities;
using MapHandler;

namespace Assets.Code.AssetHandling
{
    public class MovementAnimation : AnimationBase
    {
        private bool _animDown = false;

        public MovementAnimation(SpriteSheet sheet) : base(sheet) {
            this.AnimationTimeInSeconds = 0.12f;
        }

        public override void OnReset()
        {
            _animDown = false;
        }

        public override AnimationResult Loop(Direction dir)
        {
            var sprites = this.SpriteSheet.GetSheet(dir);

            if (_animDown)
            {
                CurrentFrame--;
            }
            else
            {
                CurrentFrame++;
            }
            if (CurrentFrame == this.SpriteSheet.RowSize - 1)
                _animDown = true;
            if (CurrentFrame == 0)
                _animDown = false;

            return new AnimationResult()
            {
                OffsetX = 0,
                OffsetY = 0,
                Sprite = sprites[CurrentFrame]
            };
        }
    }
}
