using Common.Networking.Packets;
using MapHandler;
using ServerCore.Game.GameMap;
using System.Collections.Generic;

namespace ServerCore.GameServer.Players
{
    public static class PlayerExtensions
    {
        public static ServerChunk GetChunk(this OnlinePlayer player)
        {
            var chunkX = player.X >> 4;
            var chunkY = player.Y >> 4;
            return Server.Map.GetChunk(chunkX, chunkY);
        }

        public static List<OnlinePlayer> GetPlayersNear(this OnlinePlayer player)
        {
            List<OnlinePlayer> near = new List<OnlinePlayer>();
            var chunk = player.GetChunk();
            var radius = MapUtils.GetRadius(chunk.x, chunk.y, 2);
            foreach (var position in radius)
            {
                var chunkThere = Server.Map.GetChunk(position.X, position.Y);
                if(chunkThere != null)
                {
                    foreach (var playerInChunk in chunkThere.PlayersInChunk)
                    {
                        if(playerInChunk.UserId != player.UserId)
                            near.Add(playerInChunk);
                    }
                }
                
            }
            return near;
        }
    }
}
