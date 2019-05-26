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

            gameObj.transform.localScale = new Vector3(GameCamera.GAME_OBJECTS_SCALE, GameCamera.GAME_OBJECTS_SCALE);
            gameObj.tag = "Tile";
            var spriteRenderer = gameObj.AddComponent<SpriteRenderer>();

            var spriteFile = AssetHandler.LoadedAssets[DefaultAssets.TLE_SET1];

            var tilesW = spriteFile.GetLength(0);
            var tilesH = spriteFile.GetLength(1);

            opts.TileId--;

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
    }

    public class TileOptions
    {
        public Position Position;
        public short TileId;
        public GameObject Parent;
    }
}
