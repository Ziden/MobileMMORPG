
using Assets.Code.Game.Entities;
using System.Collections.Generic;

namespace Assets.Code.AssetHandling.Sprites.Animations
{

    public enum SpriteAnimations
    {
        NONE = 0,
        MOVING = 1,
        ATTACKING = 2,
        DEAD = 3
    }

    public class SpriteAnimationHandler
    {
        private Dictionary<SpriteAnimations, AnimationBase> Animations
            = new Dictionary<SpriteAnimations, AnimationBase>();

        public SpriteAnimationHandler(SpriteSheet sheet)
        {
            Animations.Add(SpriteAnimations.MOVING, new MovementAnimation(sheet));
            Animations.Add(SpriteAnimations.ATTACKING, new AttackAnimation(sheet));
            Animations.Add(SpriteAnimations.DEAD, new DeadAnimation(sheet));
        }

        public AnimationBase GetAnimation(SpriteAnimations animation)
        {
            return Animations[animation];
        }

    }


}
