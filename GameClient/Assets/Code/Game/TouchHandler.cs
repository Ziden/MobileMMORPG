using Assets.Code.Game.ClientPlayer;
using Assets.Code.Net.PacketListeners;
using Client.Net;
using Common.Networking.Packets;
using MapHandler;
using System;
using UnityEngine;

namespace Assets.Code.Game
{
    public class TouchHandler : MonoBehaviour
    {
        public static bool GameTouchEnabled = false;

        private void ClickTile(Position tile)
        {
            var willMove = UnityClient.Player.FindPathTo(tile);
            if(willMove)
            {
                // put the green square on the gruond to indicate where u going to
                Selectors.MoveMovementSelectorTo(tile);
            }
            // remove the indicators that we are targeting any monsters
            Selectors.RemoveSelector(SelectorType.TARGET);
            if(UnityClient.Player.Target != null)
            {
                UnityClient.Player.Target = null;
                UnityClient.TcpClient.Send(new EntityTargetPacket()
                {
                    WhoUuid = UnityClient.Player.UID,
                    TargetUuid = null // not targetting anyone
                });
            }
        }

        private void ButtonDown(Vector2 position)
        {
            var realPosition = Camera.main.ScreenToWorldPoint(position);

            var realX = (int)Math.Floor(realPosition.x / 16);
            var realY = (int)Math.Floor(realPosition.y / 16);

            var clickedObject = SelectedObjectByMouse();
            if(clickedObject != null)
            {
                PlayerListener.PlayerSetTarget(clickedObject);
            } else
            {
                //if (EventSystem.current.()) // check if didnt clickd UI elements
                ClickTile(new Position(realX, -realY));
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
