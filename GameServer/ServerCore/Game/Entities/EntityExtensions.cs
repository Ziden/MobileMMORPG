using MapHandler;
using ServerCore.Game.GameMap;
using ServerCore.GameServer.Players;
using System.Collections.Generic;

namespace ServerCore.GameServer.Entities
{
    public static class EntityExtensions
    {
        public static ServerChunk GetChunk(this Entity player)
        {
            var chunkX = player.Position.X >> 4;
            var chunkY = player.Position.Y >> 4;
            return Server.Map.GetChunk(chunkX, chunkY);
        }

        public static List<OnlinePlayer> GetPlayersNear(this Entity player)
        {
            List<OnlinePlayer> near = new List<OnlinePlayer>();
            var chunk = player.GetChunk();
            var radius = MapHelpers.GetSquared3x3(new Position(chunk.x, chunk.y));
            foreach (var position in radius)
            {
                var chunkThere = Server.Map.GetChunk(position.X, position.Y);
                if(chunkThere != null)
                {
                    foreach (var playerInChunk in chunkThere.PlayersInChunk)
                    {
                        if(playerInChunk.UID != player.UID)
                            near.Add(playerInChunk);
                    }
                }
                
            }
            return near;
        }
    }
}
