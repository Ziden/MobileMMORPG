using Assets.Code.Game;
using Assets.Code.Game.ClientPlayer;
using Assets.Code.Game.Factories;
using Client.Net;
using Common.Networking.Packets;
using CommonCode.EventBus;
using CommonCode.Networking.Packets;
using MapHandler;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Net.PacketListeners
{
    public class PlayerListener : IEventListener
    {

        [EventMethod]
        public void OnPlayerSync(SyncPacket packet)
        {

            Position destination = null;
            if (UnityClient.Player.FollowingPath != null && UnityClient.Player.FollowingPath.Count > 0)
            {
                destination = UnityClient.Player.FollowingPath.Last();
            }
            UnityClient.Player.PlayerObject.GetComponent<PlayerBehaviour>().StopMovement();
            UnityClient.Player.FollowingPath = null;
            UnityClient.Player.TeleportToTile(packet.Position.X, packet.Position.Y);
            Debug.Log("SYNC TO " + packet.Position.X + " - " + packet.Position.Y);

            // recalculating route to destination
            if (destination != null)
            {
                var path = WorldMap<Chunk>.FindPath(UnityClient.Player.Position, destination, UnityClient.Map.Chunks);
                if (path != null)
                {
                    UnityClient.Player.FollowingPath = path;
                }
            }
        }

        [EventMethod]
        public void OnPlayerMove(PlayerMovePacket packet)
        {
            var playerObj = GameObject.Find(packet.UserId);
            if(playerObj != null)
            {
               var movingEntity = playerObj.GetComponent<MovingEntityBehaviour>();
                movingEntity.Route.Add(packet.To);
            }
        }

        [EventMethod]
        public void OnPlayerAppears(PlayerPacket packet)
        {
            // instantiate the player if needed
            PlayerHandler.BuildAndInstantiate(new PlayerFactoryOptions()
            {
                SpriteIndex = packet.SpriteIndex,
                UserId = packet.UserId,
                Speed = packet.Speed,
                tileX = packet.X,
                tileY = packet.Y,
                IsMainPlayer = packet.UserId == UnityClient.Player.UserId
            });

            TouchHandler.GameTouchOn = true;
        }
    }
}
