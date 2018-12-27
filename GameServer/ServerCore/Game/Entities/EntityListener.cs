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
            Log.Info("ENTITY " + ev.Entity.UID + " GOING TO " + ev.Entity.Position.ToString());
            ev.Entity.Position = ev.Position;

            Server.Map.UpdateEntityPosition(ev.Entity, null, ev.Entity.Position);

            // Track in monsters list if its a monster
            if(ev.Entity.EntityType == EntityType.MONSTER)
                Server.Map.Monsters.Add(ev.Entity.UID, (Monster)ev.Entity);

         
        }

        [EventMethod]
        public void OnEntityMove(EntityMoveEvent ev)
        {
            var nearPlayers = ev.Entity.GetPlayersNear();

            var movePacket = new EntityMovePacket()
            {
                To = ev.To,
                UID = ev.Entity.UID
            };

            ev.Entity.LastPosition = ev.Entity.Position;

            // Updating this movement to nearby players soo the client updates
            foreach (var nearPlayer in nearPlayers)
            {
                if(nearPlayer.UID != ev.Entity.UID) 
                    nearPlayer.Tcp.Send(movePacket);
            }

            Log.Info("ENTITY " + ev.Entity.UID + " GOING TO " + ev.To.ToString());

            Server.Map.UpdateEntityPosition(ev.Entity, ev.Entity.Position, ev.To);
            ev.Entity.Position = ev.To;
        }
    }
}
