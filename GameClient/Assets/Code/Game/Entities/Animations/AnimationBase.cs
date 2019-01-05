
using Assets.Code.Game.Entities;
using Assets.Code.Game.Entities.Animations;
using MapHandler;
using System;
using System.Collections.Generic;

namespace Assets.Code.AssetHandling.Sprites.Animations
{
    public class AnimationBase
    {
        public bool IsOver = false;
        public SpriteSheet SpriteSheet;
        public int CurrentFrame = 0;
        public int OffsetX = 0;
        public int OffsetY = 0;
        public float AnimationTimeInSeconds = 0.1f;

        // for when u want something to happen in a specific animation frame
        public List<AnimationCallback> Callbacks = new List<AnimationCallback>();

        public void Reset()
        {
            CurrentFrame = 0;
            OffsetX = 0;
            OffsetY = 0;
            IsOver = false;
            Callbacks.Clear();
            OnReset();
        }

        public virtual AnimationResult Loop(Direction dir) => null;

        public virtual void OnReset() { }

        public AnimationBase(SpriteSheet sheet)
        {
            this.SpriteSheet = sheet;
        }
    }
}
