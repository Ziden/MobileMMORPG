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

        // Becomes true when all assets have been loaded so its a "valid" online player
        public bool AssetsReady = false;

        public Position GetPosition()
        {
            return new Position(X, Y);
        }

        // When this player can perform a movement again (in MS)
        public long CanMoveAgainTime = 0;

    }
}
