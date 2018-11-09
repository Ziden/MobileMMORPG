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
            if(UnityClient.Player.FollowingPath != null)
            {
                destination = UnityClient.Player.FollowingPath.Last();
            }
            UnityClient.Player.PlayerObject.GetComponent<PlayerBehaviour>().StopMovement();
            UnityClient.Player.FollowingPath = null;
            UnityClient.Player.MoveToTile(packet.Position.X, packet.Position.Y);
            Debug.Log("SYNC TO " + packet.Position.X + " - " + packet.Position.Y);

            // recalculating route to destination
            var path = Map.FindPath(UnityClient.Player.Position, destination, UnityClient.Map.Chunks);
            if (path != null)
            {
                UnityClient.Player.FollowingPath = path;
            }
        }

        [EventMethod]
        public void OnPlayerUpdate(PlayerPacket packet)
        {
            // Its me ! Might have to update my own state
            if(packet.UserId == UnityClient.Player.UserId)
            {
                // instantiate the player
                PlayerHandler.BuildAndInstantiate(new PlayerFactoryOptions()
                {
                    SpriteIndex = packet.SpriteIndex,
                    UserId = packet.UserId,
                    Speed = packet.Speed
                });

                TouchHandler.GameTouchOn = true;
            }
        }
    }
}
