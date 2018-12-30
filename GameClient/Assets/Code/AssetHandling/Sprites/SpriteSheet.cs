using Assets.Code.AssetHandling.Sprites.Animations;
using MapHandler;
using UnityEngine;

namespace Assets.Code.AssetHandling
{
    public class SpriteSheet : MonoBehaviour
    {
        private Direction Direction = Direction.SOUTH;

        private float _deltaTime = 0;
        private float _frameSeconds = 0.12f;

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

        public void SetDirection(Direction dir)
        {
            if (this.Direction != dir)
            {
                this.Direction = dir;
                if (CurrentAnimation != null)
                    _animResult = CurrentAnimation.Loop(this.Direction);
            }
        }

        public void SetAnimation(SpriteAnimations animation)
        {
            if (animation == SpriteAnimations.NONE)
            {
                Debug.Log("RESET");
                if(CurrentAnimation != null)
                    CurrentAnimation.Reset();

                CurrentAnimation = null;
                return;
            }

            var animationToSet = Animations.GetAnimation(animation);

            if(animationToSet != CurrentAnimation)
            {
                CurrentAnimation = animationToSet;
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
                WalkNorth = new Sprite[] { spriteRow[0], spriteRow[1], spriteRow[2] };
                WalkRight = new Sprite[] { spriteRow[9], spriteRow[10], spriteRow[11] };
                WalkSouth = new Sprite[] { spriteRow[6], spriteRow[7], spriteRow[8] };
                WalkLeft = new Sprite[] { spriteRow[3], spriteRow[4], spriteRow[5] };
                Dead = spriteRow[12];
            }
            else if (rowSize == 2)
            {
                WalkNorth = new Sprite[] { spriteRow[0], spriteRow[1] };
                WalkRight = new Sprite[] { spriteRow[6], spriteRow[7] };
                WalkSouth = new Sprite[] { spriteRow[4], spriteRow[5] };
                WalkLeft = new Sprite[] { spriteRow[2], spriteRow[3] };
                Dead = spriteRow[1];
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
            if (CurrentAnimation == null)
            {
                Renderer.sprite = GetSheet(Direction)[1];
                return;
            }
            _deltaTime += Time.deltaTime;
            while (_deltaTime >= _frameSeconds)
            {
                _deltaTime -= _frameSeconds;
                 _animResult = CurrentAnimation.Loop(Direction);
                if (CurrentAnimation.IsOver)
                {
                    CurrentAnimation = null;
                }
            }
            if(_animResult != null)
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
