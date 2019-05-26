using Common;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Code.Net
{
    public static class AssetHandler
    {
        public static ConcurrentList<string> WaitingForAssets = new ConcurrentList<string>();

        public static Dictionary<string, Sprite[,]> LoadedAssets = new Dictionary<string, Sprite[,]>();

        public static Sprite[,] LoadNewSprite(string FilePath, float PixelsPerUnit = 1f)
        {
            Texture2D SpriteTexture = LoadTexture(FilePath);
            SpriteTexture.filterMode = FilterMode.Point;
            SpriteTexture.wrapMode = TextureWrapMode.Repeat;
            SpriteTexture.anisoLevel = 0;
            SpriteTexture.Apply();
            var spritesX = SpriteTexture.width / 16;
            var spritesY = SpriteTexture.height / 16;
            var spriteMap = new Sprite[spritesX, spritesY];
            for (var x = 0; x < spritesX; x++)
            {
                for(var y = 0; y < spritesY; y++)
                {
                    var sprite = Sprite.Create(SpriteTexture, new Rect(x * 16, y * 16, 16, 16), new Vector2(0, 0));
                    spriteMap[x, spritesY - y - 1] = sprite;
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
                Tex2D = new Texture2D(16, 16, TextureFormat.ARGB32, false);
                if (Tex2D.LoadImage(FileData))
                {
                    return Tex2D;
                }                
            }
            return null;      
        }
    }
}
