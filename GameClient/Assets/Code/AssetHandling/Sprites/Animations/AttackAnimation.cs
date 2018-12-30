using Assets.Code.AssetHandling.Sprites.Animations;
using MapHandler;

namespace Assets.Code.AssetHandling
{
    public class AttackAnimation : AnimationBase
    {
        public AttackAnimation(SpriteSheet sheet) : base(sheet) { }

        public override AnimationResult Loop(Direction dir)
        {
            // Preparation
            if(CurrentFrame < 4)
            {
                return new AnimationResult()
                {
                    Sprite = this.SpriteSheet.GetSheet(dir)[CurrentFrame > 2 ? 2 : CurrentFrame]
                };
            } else if(CurrentFrame == 4)
            { // wait

            } else if(CurrentFrame < 6)
            {
                return new AnimationResult()
                {
                    Sprite = this.SpriteSheet.GetSheet(dir)[2- CurrentFrame > 2 ? 2 : CurrentFrame]
                };
            } else 
            {
                IsOver = true;
                return null;
            }
            CurrentFrame++;
            return null;
        }
    }
}
