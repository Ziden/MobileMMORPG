using MapHandler;
using ServerCore.Game.Monsters;
using ServerCore.GameServer.Players;
using System.Collections.Generic;

namespace ServerCore.Game.GameMap
{
    public class ServerChunk : Chunk
    {
        public List<OnlinePlayer> PlayersInChunk = new List<OnlinePlayer>();

        public List<Monster> MonstersInChunk = new List<Monster>();
    }
}