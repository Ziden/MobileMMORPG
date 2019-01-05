using Client.Net;
using MapHandler;

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
                player.Behaviour.Route = path;
            }
            return path != null;
        }
    }
}
