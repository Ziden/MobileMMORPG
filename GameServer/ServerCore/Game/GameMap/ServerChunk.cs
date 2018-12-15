using MapHandler;
using ServerCore.Game.Entities;
using ServerCore.GameServer.Players;
using System.Collections.Generic;
using System.Linq;

namespace ServerCore.Game.GameMap
{
    public class ServerChunk : Chunk
    {
        public Dictionary<EntityType, List<Entity>> EntitiesInChunk = new Dictionary<EntityType, List<Entity>>();

        public ServerChunk()
        {
            EntitiesInChunk.Add(EntityType.PLAYER, new List<Entity>());
            EntitiesInChunk.Add(EntityType.MONSTER, new List<Entity>());
        }

        public void MoveFromChunk(Entity e, ServerChunk from)
        {
            this.EntitiesInChunk[e.EntityType].Add(e);
            from.EntitiesInChunk[e.EntityType].Remove(e);
        }

        public List<OnlinePlayer> PlayersInChunk {
            get {
                return EntitiesInChunk[EntityType.PLAYER].Select(entity => (OnlinePlayer)entity).ToList();
            }
        }
    }
}