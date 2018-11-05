using Assets.Code.AssetHandling;
using Assets.Code.Net;
using Client.Net;
using MapHandler;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Game.Factories
{
    public class PlayerHandler
    {
        private static Dictionary<string, GameObject> PlayerTracker = new Dictionary<string, GameObject>();

        public static void BuildAndInstantiate(PlayerFactoryOptions opts)
        {
            var playerObj = GameObject.Find(opts.UserId);
            if (playerObj == null)
            {
                playerObj = new GameObject(opts.UserId);
                playerObj.transform.localScale = new Vector3(100, 100);
               
                playerObj.tag = "Player";
                var spriteRenderer = playerObj.AddComponent<SpriteRenderer>();
                var spriteSheet = playerObj.AddComponent<SpriteSheet>();
                var playerBehaviour = playerObj.AddComponent<PlayerBehaviour>();
                spriteRenderer.sortingOrder = 2;
                var spriteFile = AssetHandler.LoadedAssets[AssetHandler.SPRITE_FILE];
                var spriteRow = spriteFile.SliceRow(opts.SpriteIndex).ToArray();
                spriteSheet.SetSheet(spriteRow);
                spriteRenderer.sprite = spriteSheet.WalkSouth[1];
                PlayerTracker.Add(opts.UserId, playerObj);

                playerObj.transform.position = new Vector2(opts.tileX * 16, opts.tileY * 16);

                UnityClient.Player.Position = new Position(opts.tileX, opts.tileY);
                UnityClient.Player.Speed = opts.Speed;
                UnityClient.Player.PlayerObject = playerObj;
            }
        }
    }

    public class PlayerFactoryOptions
    {
        public string UserId;
        public int SpriteIndex;
        public int tileX;
        public int tileY;
        public int Speed;
    }
}
