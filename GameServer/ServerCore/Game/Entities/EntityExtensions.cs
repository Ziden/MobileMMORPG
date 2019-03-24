using MapHandler;
using ServerCore.Game.Entities;
using ServerCore.Game.GameMap;
using ServerCore.GameServer.Players;
using System.Collections.Generic;
using Common.Entity;

namespace ServerCore.GameServer.Entities
{
    public static class EntityExtensions
    {
        public static ServerChunk GetChunk(this Entity player)
        {
            var chunkX = player.Position.X >> 4;
            var chunkY = player.Position.Y >> 4;
            return Server.Map.GetChunkByChunkPosition(chunkX, chunkY);
        }

        public static List<OnlinePlayer> GetPlayersNear(this Entity player, bool includeSelf = false)
        {
            List<OnlinePlayer> near = new List<OnlinePlayer>();
            var chunk = player.GetChunk();
            var radius = PositionExtensions.GetSquared3x3Around(new Position(chunk.x, chunk.y));
            foreach (var position in radius)
            {
                var chunkThere = Server.Map.GetChunkByChunkPosition(position.X, position.Y);
                if(chunkThere != null)
                {
                    var teste = chunkThere.EntitiesInChunk[EntityType.PLAYER];
                    foreach (var playerInChunk in chunkThere.EntitiesInChunk[EntityType.PLAYER])
                    {
                        if(includeSelf || (playerInChunk.UID != player.UID))
                            near.Add((OnlinePlayer)playerInChunk);
                    }
                }
                
            }
            return near;
        }
    }
}
