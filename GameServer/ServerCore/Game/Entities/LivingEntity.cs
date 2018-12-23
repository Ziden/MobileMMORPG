using MapHandler;
using ServerCore.GameServer.Players;
using System.Collections.Generic;

namespace ServerCore.Game.Entities
{
    public class LivingEntity : Entity
    {
        public string Name;
        public int HP;
        public int MoveSpeed = 5;
        public long MovementDelay = 2000; // in millis

        // When a living entity is targeting another
        public LivingEntity Target;

        public List<OnlinePlayer> GetNearbyPlayers()
        {
            List<OnlinePlayer> near = new List<OnlinePlayer>();
            var radius = MapHelpers.GetSquared3x3(new Position(Position.X >> 4, Position.Y >> 4));
            foreach (var position in radius)
            {
                var chunkThere = Server.Map.GetChunkByChunkPosition(position.X, position.Y);
                if (chunkThere != null)
                {
                    foreach (var playerInChunk in chunkThere.EntitiesInChunk[EntityType.PLAYER])
                    {
                        near.Add((OnlinePlayer)playerInChunk);
                    }
                }
            }
            return near;
        }
    }
}
