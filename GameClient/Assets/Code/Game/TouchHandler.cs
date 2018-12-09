using Assets.Code.Game.Factories;
using Assets.Code.Net.PacketListeners;
using Client.Net;
using MapHandler;
using System;
using UnityEngine;

namespace Assets.Code.Game
{
    public class TouchHandler : MonoBehaviour
    {
        public static bool GameTouchEnabled = false;

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

            var clickedObject = SelectedObjectByMouse();
            if(clickedObject != null)
            {
                Selectors.RemoveSelector("targeted");
                var objType = FactoryMethods.GetType(clickedObject);
                if(objType == FactoryObjectTypes.MONSTER)
                {
                    Selectors.AddSelector(clickedObject, "targeted", Color.red);
                    PlayerListener.PlayerSetTarget(clickedObject);
                }
  
            } else
            {
                //if (EventSystem.current.()) // check if didnt clickd UI elements
                ClickTile(new Vector2(realX, -realY));
            }
        }

        public GameObject SelectedObjectByMouse()
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hitCollider = Physics2D.OverlapPoint(mousePosition);
            if(hitCollider != null && hitCollider.transform != null)
            {
                return hitCollider.gameObject;
            }
            return null;
        }

        public void Update()
        {
            if (!GameTouchEnabled)
                return;

            if (Input.touchCount > 0)
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
