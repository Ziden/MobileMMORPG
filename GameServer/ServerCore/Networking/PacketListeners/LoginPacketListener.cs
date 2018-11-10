using Common.Networking.Packets;
using CommonCode.EventBus;
using ServerCore.GameServer.Players.Evs;
using ServerCore.Networking;
using ServerCore.Networking.NetworkEvents;
using Storage.Login;

namespace ServerCore.PacketListeners
{
    public class LoginPacketListener : IEventListener
    {
        [EventMethod]
        public void OnLogin(LoginPacket packet)
        {
            var client = ServerTcpHandler.GetClient(packet.ClientId);
            try
            {
                var user = AccountService.Login(packet.Login, packet.Password);
                Log.Info("Sending Login Response");

                client.Send(new LoginResponsePacket()
                {
                    SessionId = user.SessionId,
                    UserId = user.UserId,
                    xLocation = user.X,
                    yLocation = user.Y
                });

                ServerEvents.Call(new PlayerLoggedInEvent()
                {
                    Player = user,
                    Client = client
                });
            }
            catch (AccountError e)
            {
                Log.Info("Sending Login Error");
                client.Send(new DialogPacket()
                {
                    Message = e.ErrorMessage
                });

            }
        }
    }
}
