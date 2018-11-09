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
        public ConnectedClientTcpHandler Tcp;

        public bool AssetsReady = false;

        public Position GetPosition()
        {
            return new Position(X, Y);
        }

        public long CanMoveAgainTime = 0;

    }
}
