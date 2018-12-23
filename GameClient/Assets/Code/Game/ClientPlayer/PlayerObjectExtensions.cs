using Client.Net;
using MapHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Game.ClientPlayer
{
    public static class PlayerObjectExtensions
    {
        public static void TeleportToTile(this PlayerWrapper player, int x,  int y)
        {
            var newPosition = new Position(x, y);

            UnityClient.Map.UpdateEntityPosition(player, player.Position, newPosition);
            UnityClient.Player.Position = newPosition;

            player.PlayerObject.transform.position = player.Position.ToUnityPosition();

        }

        public static bool FindPathTo(this PlayerWrapper player, Position position)
        {
            var path =UnityClient.Map.FindPath(player.Position, position);
            if (path != null)
            {
                player.FollowingPath = path;
            }
            return path != null;
        }
    }
}
