using Common.Networking.Packets;
using ServerCore.GameServer.Players;
using ServerCore.GameServer.Players.Evs;
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
            Server.TcpHandler.ClientsByConnectionId.Add(client.ConnectionId, client);
            client.SendToServer(new LoginPacket()
            {
                Login =user,
                Password = pass
            });
        }

        public static void Logout(this MockedClient client)
        {
            Server.TcpHandler.ClientsByConnectionId.Remove(client.ConnectionId);
            Server.Events.Call(new PlayerQuitEvent() { Player = client.OnlinePlayer });
        }

        public static OnlinePlayer FullLoginSequence(this MockedClient client, StoredPlayer player)
        {
            AccountService.RegisterAccount(player.Login, player.Password, player.Email, player);
            client.Login(player.Login, player.Password);
            client.SendToServer(new AssetsReadyPacket() { UserId = player.UserId });
            return Server.GetPlayer(player.UserId);
        }
    }
}
