using Client.Net;
using Common.Networking.Packets;
using CommonCode.Pathfinder;
using MapHandler;
using System;
using UnityEngine;

namespace Assets.Code.Game
{
    public class TouchHandler : MonoBehaviour
    {
        public static bool GameTouchOn = false;

        /// <summary>
        /// When user clicks any non object place, it clicks on a tile position
        /// </summary>
        private void ClickTile(Vector2 tile)
        {
            var player = UnityClient.Player;
            var path = WorldMap<Chunk>.FindPath(player.Position, new Position((int)tile.x, (int)tile.y), UnityClient.Map.Chunks);
            if(path != null)
            {
                player.FollowingPath = path;
            }
            Selectors.MoveSelector(tile);
        }

        private void ButtonDown(Vector2 position)
        {
            var realPosition = Camera.main.ScreenToWorldPoint(position);

            var realX = (int)Math.Floor(realPosition.x / 16);
            var realY = (int)Math.Floor(realPosition.y / 16);

            ClickTile(new Vector2(realX, realY));
        }

        public void Update()
        {
            if (!GameTouchOn)
                return;

            if(Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    ButtonDown(touch.position);
                }
            }
            if(Input.GetMouseButtonDown(0))
            {
                ButtonDown(Input.mousePosition);
            }
        }
    }
}
