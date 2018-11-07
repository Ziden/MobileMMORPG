using Common.Networking.Packets;
using CommonCode.EventBus;
using CommonCode.Networking.Packets;
using MapHandler;
using System.IO;

namespace ServerCore.Networking.PacketListeners
{
    public class PlayerPacketListener : IEventListener
    {
        [EventMethod] // When client finishes updating assets
        public void OnPlayerMovePath(PlayerMovePacket packet)
        {
            var player = Server.GetPlayerByConnectionId(packet.ClientId);
        }
    }
}
