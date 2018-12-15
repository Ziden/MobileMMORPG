using Common.Networking.Packets;
using CommonCode.EventBus;
using ServerCore.Game.Monsters;
using ServerCore.GameServer.Entities;
using ServerCore.GameServer.Players.Evs;

namespace ServerCore.Game.Entities
{
    public class EntityListener : IEventListener
    {

        [EventMethod]
        public void OnEntitySpawn(EntitySpawnEvent ev)
        {
            var chunkX = ev.Position.X >> 4;
            var chunkY = ev.Position.Y >> 4;

            // Track the entity in the chunk
            var chunk = Server.Map.GetChunk(chunkX, chunkY);
            chunk.EntitiesInChunk[ev.Entity.EntityType].Add(ev.Entity);

            // Track the entity position cache
            Server.Map.EntityPositions.AddEntity(ev.Entity, ev.Position);

            // Track in monsters list if its a monster
            if(ev.Entity.EntityType == EntityType.MONSTER)
                Server.Map.Monsters.Add(ev.Entity.UID, (Monster)ev.Entity);

            ev.Entity.Position = ev.Position;

        }

        [EventMethod]
        public void OnEntityMove(EntityMoveEvent ev)
        {
            var fromChunkX = ev.From.X >> 4;
            var fromChunkY = ev.From.Y >> 4;

            var toChunkX = ev.To.X >> 4;
            var toChunkY = ev.To.Y >> 4;

            var toChunk = Server.Map.GetChunk(toChunkX, toChunkY);

            var nearPlayers = ev.Entity.GetPlayersNear();
            var movePacket = new EntityMovePacket()
            {
                From = ev.From,
                To = ev.To,
                UID = ev.Entity.UID
            };

            // Update Entity Position Cache
            Server.Map.EntityPositions.RemoveEntity(ev.Entity, ev.From);
            Server.Map.EntityPositions.AddEntity(ev.Entity, ev.To);
            ev.Entity.LastPosition = ev.From;

            // Updating this movement to nearby players soo the client updates
            foreach (var nearPlayer in nearPlayers)
            {
                nearPlayer.Tcp.Send(movePacket);
            }

            // Changed chunk
            if (fromChunkX != toChunkX || fromChunkY != toChunkY)
            {
                var fromChunk = Server.Map.GetChunk(fromChunkX, fromChunkY);
                toChunk.MoveFromChunk(ev.Entity, fromChunk);
            }

            ev.Entity.Position = ev.To;
        }
    }
}
