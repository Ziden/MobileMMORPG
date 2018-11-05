using Assets.Code.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Game.Factories
{
    public class TileFactory
    {
        public static void BuildAndInstantiate(int x, int y, int tileId, GameObject parent = null)
        {
            if (tileId == 0)
                return;
            var gameObj = new GameObject(x + "_" + y); ;
            gameObj.transform.localScale = new Vector3(100, 100);
            gameObj.tag = "Tile";
            var spriteRenderer = gameObj.AddComponent<SpriteRenderer>();

            var spriteFile = AssetHandler.LoadedAssets[AssetHandler.TILESET_FILE];

            var tilesW = spriteFile.GetLength(0);
            var tilesH = spriteFile.GetLength(1);

            tileId--;

            int tileIndexY = (int)Math.Floor((double)tileId / tilesW);
            var tileIndexX = tileId % tilesW;

            var tileSprite = spriteFile[tileIndexX, tileIndexY];
            spriteRenderer.sprite = tileSprite;

            gameObj.transform.position = new Vector3(x * 16, -y * 16, 0);

            if (parent != null)
            {
                gameObj.transform.SetParent(parent.transform);
            }
        }
    }
}
