using Common;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Code.Net
{
    public static class AssetHandler
    {
        public static ConcurrentList<string> WaitingForAssets = new ConcurrentList<string>();

        // TODO: MAKE THIS BETTER...
        //public static readonly string SPRITE_FILE = "sprites.png";

        public static readonly string BODIES = "bodies.png";
        public static readonly string CHESTS = "chests.png";
        public static readonly string LEGS = "legs.png";
        public static readonly string HEADS = "heads.png";

        public static readonly string TILESET_FILE = "Set1.png";

        public static readonly string MONSTERS_1 = "monsters_1.png"; 

        public static Dictionary<string, Sprite[,]> LoadedAssets = new Dictionary<string, Sprite[,]>();

        public static Sprite[,] LoadNewSprite(string FilePath, float PixelsPerUnit = 1f)
        {
            Texture2D SpriteTexture = LoadTexture(FilePath);
            SpriteTexture.filterMode = FilterMode.Point;
            var spritesX = SpriteTexture.width / 16;
            var spritesY = SpriteTexture.height / 16;

            var spriteMap = new Sprite[spritesX, spritesY];

            for (var x = 0; x < spritesX; x++)
            {
                for(var y = 0; y < spritesY; y++)
                {
                    spriteMap[x, spritesY-y-1] = Sprite.Create(SpriteTexture, new Rect(x * 16, y*16 , 16, 16), new Vector2(0, 0));
                }
            }
            return spriteMap;
        }

        public static Texture2D LoadTexture(string FilePath)
        {
            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(FilePath))
            {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(1, 1);
                Tex2D.filterMode = FilterMode.Point;
                Tex2D.wrapMode = TextureWrapMode.Repeat;
                if (Tex2D.LoadImage(FileData))           
                    return Tex2D;              
            }
            return null;      
        }
    }
}
