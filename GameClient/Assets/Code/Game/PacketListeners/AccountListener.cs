using Common.Networking.Packets;
using CommonCode.EventBus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Net.PacketListeners
{
    public class AccountListener : IEventListener
    {
        [EventMethod]
        public void OnLoginResponse(LoginResponsePacket packet)
        {
            UnityClient.Player.SessionId = packet.SessionId;
            UnityClient.Player.UserId = packet.UserId;
            Debug.Log("Logged in userid "+packet.UserId);
            LoginScreen.Kill();
        }
    }
}
