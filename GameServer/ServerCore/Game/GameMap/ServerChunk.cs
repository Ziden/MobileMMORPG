using MapHandler;
using ServerCore.GameServer.Players;
using System.Collections.Generic;

namespace ServerCore.Game.GameMap
{
    public class ServerChunk : Chunk
    {
        public List<OnlinePlayer> PlayersInChunk = new List<OnlinePlayer>();
    }
}