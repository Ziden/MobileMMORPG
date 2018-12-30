
using MapHandler;

namespace Assets.Code.AssetHandling.Sprites.Animations
{
    public class AnimationBase
    {
        public bool IsOver = false;
        public SpriteSheet SpriteSheet;
        public int CurrentFrame = 0;
        public int OffsetX = 0;
        public int OffsetY = 0;

        public void Reset()
        {
            CurrentFrame = 0;
            OffsetX = 0;
            OffsetY = 0;
            IsOver = false;
            OnReset();
        }

        public virtual AnimationResult Loop(Direction dir) { return null; }

        public virtual void OnReset() { }

        public AnimationBase(SpriteSheet sheet)
        {
            this.SpriteSheet = sheet;
        }
    }
}
