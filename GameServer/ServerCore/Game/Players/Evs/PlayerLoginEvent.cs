using ServerCore.GameServer;
using Storage.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore.Networking.NetworkEvents
{
    public class PlayerLoginEvent : IEvent
    {
        public Player Player;
        public ConnectedClientTcpHandler Client;
    }
}
