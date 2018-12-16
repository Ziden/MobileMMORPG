using Common.Networking.Packets;
using CommonCode.EventBus;
using UnityEngine;

namespace Client.Net.PacketListeners
{
    public class AccountListener : IEventListener
    {
        [EventMethod]
        public void OnLoginResponse(LoginResponsePacket packet)
        {
            UnityClient.Player.SessionId = packet.SessionId;
            UnityClient.Player.UID = packet.UserId;
            Debug.Log("Logged in userid "+packet.UserId);
            LoginScreen.Kill();
        }
    }
}
