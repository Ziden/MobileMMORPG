using MapHandler;
using ServerCore.Networking;
using Storage.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore.GameServer.Players
{
    public class OnlinePlayer : Player
    {
        public List<Position> MovingPath;

        public ConnectedClientTcpHandler Tcp;

        public bool AssetsReady = false;

    }
}
