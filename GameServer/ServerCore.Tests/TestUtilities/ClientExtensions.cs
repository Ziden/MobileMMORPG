using Common.Networking.Packets;
using ServerCore.Networking;
using Storage.Login;
using Storage.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore.Tests.TestUtilities
{
    public static class ClientExtensions
    {
        public static void Login(this MockedClient client, string user, string pass)
        {
            // "Connect"
            ServerTcpHandler.ClientsByConnectionId.Add(client.ConnectionId, client);
            client.SendToServer(new LoginPacket()
            {
                Login =user,
                Password = pass
            });
        }

        public static void FullLoginSequence(this MockedClient client, Player player)
        {
            AccountService.RegisterAccount(player.Login, player.Password, player.Email, player);
            client.Login(player.Login, player.Password);
            client.SendToServer(new AssetsReadyPacket() { UserId = player.UserId });
        }
    }
}
