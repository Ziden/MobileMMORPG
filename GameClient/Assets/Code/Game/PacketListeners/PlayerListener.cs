using Assets.Code.Game;
using Assets.Code.Game.Factories;
using Client.Net;
using CommonCode.EventBus;
using CommonCode.Networking.Packets;
using UnityEngine;

namespace Assets.Code.Net.PacketListeners
{
    public class PlayerListener : IEventListener
    {
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
