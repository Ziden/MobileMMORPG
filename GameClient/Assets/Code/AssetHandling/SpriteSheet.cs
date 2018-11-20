using Assets.Code.Net;
using MapHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Code.AssetHandling
{
    public class SpriteSheet : MonoBehaviour
    {
        public Direction Direction = Direction.SOUTH;

        private int _frame = 0;
        private float _deltaTime = 0;
        private float _frameSeconds = 0.1f;

        public bool Moving = false;

        public Sprite[] WalkNorth;
        public Sprite[] WalkSouth;
        public Sprite[] WalkRight;
        public Sprite[] WalkLeft;
        public Sprite Dead;
        private SpriteRenderer Renderer;

        private bool _animDown = false;

        public void Start()
        {
            Renderer = transform.GetComponent<SpriteRenderer>();
        }

        public void SetSheet(Sprite[] spriteRow, int rowSize = 3)
        {
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

        private Sprite[] GetSheet(Direction dir)
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

        // Update is called once per frame
        void Update()
        {

            var sprites = GetSheet(Direction);

            if (sprites == null)
                return;

            if (!Moving)
            {
                Renderer.sprite = sprites[1];
                return;
            }

            _deltaTime += Time.deltaTime;

            while (_deltaTime >= _frameSeconds)
            {
                _deltaTime -= _frameSeconds;
                if (_animDown)
                {
                    _frame--;
                }
                else
                {
                    _frame++;
                }

                if (_frame == sprites.Length - 1)
                    _animDown = true;
                if (_frame == 0)
                    _animDown = false;
            }
            Renderer.sprite = sprites[_frame];
        }

        // SPRITE MAPPING
        private int[] WALK_NORTH = new int[] { 0, 1, 2 };
        private int[] WALK_RIGHT = new int[] { 3, 4, 5 };
        private int[] WALK_SOUTH = new int[] { 6, 7, 8 };
        private int[] WALK_LEFT = new int[] { 9, 10, 11 };
        private int DEAD = 12;

    }
}
