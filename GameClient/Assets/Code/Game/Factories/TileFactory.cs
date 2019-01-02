using Assets.Code.Net;
using CommonCode.EntityShared;
using MapHandler;
using System;
using UnityEngine;

namespace Assets.Code.Game.Factories
{
    public class TileFactory
    {
        public static void BuildAndInstantiate(TileOptions opts)
        {
            if (opts.TileId == 0)
                return;
            var gameObj = new GameObject("Tile_"+opts.Position.X + "_" + opts.Position.Y);

            if (opts.TileId != 1)
                Debug.Log($"Id: { opts.TileId } - pos: { opts.Position.ToUnityPosition() }");

            gameObj.transform.localScale = new Vector3(100, 100);
            gameObj.tag = "Tile";
            var spriteRenderer = gameObj.AddComponent<SpriteRenderer>();

            var spriteFile = AssetHandler.LoadedAssets[DefaultAssets.TLE_SET1];

            var tilesW = spriteFile.GetLength(0); // 4
            var tilesH = spriteFile.GetLength(1); // 3

            //id = 1 => (0, 2)
            //id = 2 => (1, 2)
            //id = 3 => (2, 2)
            //id = 4 => (3, 2)
            //id = 5 => (0, 1)

            opts.TileId--;

            var testinho = GetCoordinate(opts.TileId, tilesW, tilesH);

            int tileIndexY = (int)Math.Floor((double)opts.TileId / tilesW);
            var tileIndexX = opts.TileId % tilesW;

            var tileSprite = spriteFile[tileIndexX, tileIndexY];
            spriteRenderer.sprite = tileSprite;

            gameObj.transform.position = opts.Position.ToUnityPosition();

            if (opts.Parent != null)
            {
                gameObj.transform.SetParent(opts.Parent.transform);
            }
        }
        
        public static Vector2 GetCoordinate(int index, int colums, int rows)
        {
            for (int i = 0; i < rows; i++)
                if (index < (colums * i) + colums && index >= colums * i)
                    return new Vector2(index - colums * i, i);

            return new Vector2(-1, -1);
        }

        //public static int GetIndex(int colums, int x, int y) => y * colums + x;
    }

    public class TileOptions
    {
        public Position Position;
        public short TileId;
        public GameObject Parent;
    }
}
