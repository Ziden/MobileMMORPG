using Assets.Code.AssetHandling.Sprites.Animations;
using Assets.Code.Game.Entities.Animations;
using MapHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Game.Entities
{
    public class SpriteSheet : MonoBehaviour
    {
        private Direction Direction = Direction.SOUTH;

        private float _deltaTime = 0;

        public int RowSize = 3;

        public Sprite[] WalkNorth;
        public Sprite[] WalkSouth;
        public Sprite[] WalkRight;
        public Sprite[] WalkLeft;
        public Sprite Dead;
        private SpriteRenderer Renderer;

        private SpriteAnimationHandler Animations;

        private AnimationBase CurrentAnimation;

        private AnimationResult _animResult;

        private List<AnimationCallback> PendingCallbacks = new List<AnimationCallback>();

        public void SetDirection(Direction dir)
        {
            if (this.Direction != dir)
            {
                this.Direction = dir;
                if (CurrentAnimation != null)
                    _animResult = CurrentAnimation.Loop(this.Direction);
            }
        }

        public bool IsAnimationPlayng(SpriteAnimations anim)
        {
            if (CurrentAnimation == null && anim == SpriteAnimations.NONE)
                return true;
            return CurrentAnimation == Animations.GetAnimation(anim);
        }

        public void SetAnimationFrameCallback(int frame, Action action)
        {
            CurrentAnimation?.Callbacks.Add(new AnimationCallback()
            {
                Callback = action,
                PlayOnFrame = frame
            });
        }

        public void SetAnimation(SpriteAnimations animation, float animationTimeInMS = -1)
        {
            if (animation == SpriteAnimations.NONE)
            {
                if (CurrentAnimation != null)
                    CurrentAnimation.Reset();

                CurrentAnimation = null;
                return;
            }

            var animationToSet = Animations.GetAnimation(animation);

            if (animationToSet != CurrentAnimation)
            {

                // if my animation had callbacks i will still wanna run them 
                if (CurrentAnimation?.Callbacks.Count > 0)
                {
                    foreach (var animCallback in CurrentAnimation.Callbacks)
                    {
                        var framesUntilCallback = animCallback.PlayOnFrame - CurrentAnimation.CurrentFrame;
                        animCallback.PlayOnFrame = framesUntilCallback;
                        PendingCallbacks.Add(animCallback);
                    }
                }

                CurrentAnimation?.Reset();
                CurrentAnimation = animationToSet;
                CurrentAnimation.Reset();
                if (animationTimeInMS > 0)
                {
                    //  CurrentAnimation.AnimationTimeInSeconds = animationTimeInMS / GameCamera.GAME_OBJECTS_SCALE0;
                }

                if (animation == SpriteAnimations.DEAD)
                {
                    CurrentAnimation.Loop(Direction);
                }
            }
        }

        public void Start()
        {
            Animations = new SpriteAnimationHandler(this);
            Renderer = transform.GetComponent<SpriteRenderer>();
        }

        public void SetSheet(Sprite[] spriteRow, int rowSize = 3)
        {
            RowSize = rowSize;
            if (rowSize == 3)
            {
                WalkSouth = new Sprite[] { spriteRow[0], spriteRow[1], spriteRow[2] };
                WalkLeft = new Sprite[] { spriteRow[9], spriteRow[10], spriteRow[11] };
                WalkNorth = new Sprite[] { spriteRow[6], spriteRow[7], spriteRow[8] };
                WalkRight = new Sprite[] { spriteRow[3], spriteRow[4], spriteRow[5] };
                Dead = spriteRow[12];
            }
            else if (rowSize == 2)
            {
                WalkSouth = new Sprite[] { spriteRow[0], spriteRow[1] };
                WalkLeft = new Sprite[] { spriteRow[6], spriteRow[7] };
                WalkNorth = new Sprite[] { spriteRow[4], spriteRow[5] };
                WalkRight = new Sprite[] { spriteRow[2], spriteRow[3] };
                Dead = spriteRow[8];
            }
        }

        public Sprite[] GetSheet(Direction dir)
        {
            switch (dir)
            {
                case Direction.LEFT:
                    return WalkLeft;
                case Direction.NORTH:
                    return WalkNorth;
                case Direction.SOUTH:
                    return WalkSouth;
                case Direction.RIGHT:
                    return WalkRight;
            }
            return null;
        }

        void Update()
        {
            if(CurrentAnimation == Animations.GetAnimation(SpriteAnimations.DEAD))
            {
                Renderer.sprite = Dead;
                return;
            }

            // Pending callbacks (cause of animation switches)
            var pendingCb = PendingCallbacks
                .Where(c => c.PlayOnFrame == 0)
                .Select(c => c)
                .FirstOrDefault();
            if (pendingCb != null)
            {
                pendingCb.Callback();
                PendingCallbacks.Remove(pendingCb);
            }
            PendingCallbacks.ForEach(pendingCallback => pendingCallback.PlayOnFrame -= 1);

            if (CurrentAnimation == null)
            {
                Renderer.sprite = GetSheet(Direction)[1];
                return;
            }
            _deltaTime += Time.deltaTime;

            while (CurrentAnimation != null && _deltaTime >= CurrentAnimation.AnimationTimeInSeconds)
            {
                _deltaTime -= CurrentAnimation.AnimationTimeInSeconds;
                _animResult = CurrentAnimation.Loop(Direction);

                // Animation callbacks to be frame-specific
                var cb = CurrentAnimation.Callbacks?
                    .Where(c => c.PlayOnFrame == CurrentAnimation.CurrentFrame)
                    .Select(c => c)
                    .FirstOrDefault();
                if (cb != null)
                {
                    cb.Callback();
                    CurrentAnimation?.Callbacks?.Remove(cb);
                }

                transform.localPosition = new Vector2(_animResult.OffsetX, _animResult.OffsetY);

                if (CurrentAnimation.IsOver)
                {
                    Debug.Log("Animation is Over");
                    CurrentAnimation = null;
                }

            }
            if (_animResult != null)
                Renderer.sprite = _animResult.Sprite;
        }

        // SPRITE MAPPING
        private int[] WALK_NORTH = new int[] { 0, 1, 2 };
        private int[] WALK_RIGHT = new int[] { 3, 4, 5 };
        private int[] WALK_SOUTH = new int[] { 6, 7, 8 };
        private int[] WALK_LEFT = new int[] { 9, 10, 11 };
        private int DEAD = 12;

    }
}
