using Common.Networking.Packets;
using CommonCode.EventBus;
using ServerCore.Game.Monsters;
using ServerCore.GameServer.Entities;
using ServerCore.GameServer.Players.Evs;
using Common.Entity;
using ServerCore.Game.Combat;
using CommonCode.Networking.Packets;
using Storage.Players;
using MapHandler;

namespace ServerCore.Game.Entities
{
    public class EntityListener : IEventListener
    {
        [EventMethod]
        public void OnEntityTarget(EntityTargetEvent ev)
        {
            Log.Info("SET TARGET");
            ev.Entity.Target = ev.TargetedEntity;
            ev.TargetedEntity.BeingTargetedBy.Add(ev.Entity);
            ev.Entity.TryAttacking(ev.TargetedEntity);
        }

        [EventMethod]
        public void OnEntityAttack(EntityAttackEvent ev)
        {
            foreach (var player in ev.Defender.GetPlayersNear())
            {
                Log.Debug("Entity Attack PLAYER found");
                player.Tcp.Send(new EntityAttackPacket()
                {
                    AttackerUID = ev.Attacker.UID,
                    DefenderUID = ev.Defender.UID,
                    Damage = ev.Damage
                });
            }

            ev.Defender.HP -= ev.Damage;
            // entity die code
        }

        [EventMethod]
        public void OnEntitySpawn(EntitySpawnEvent ev)
        {
            ev.Entity.Position = ev.Position;

            Server.Map.UpdateEntityPosition(ev.Entity, null, ev.Entity.Position);

            // Track in monsters list if its a monster
            if(ev.Entity.EntityType == EntityType.MONSTER)
                Server.Map.Monsters.Add(ev.Entity.UID, (Monster)ev.Entity);
        }

        [EventMethod]
        public void OnEntityMove(EntityMoveEvent ev)
        {
            ev.Entity.LastPosition.X = ev.Entity.Position.X;
            ev.Entity.LastPosition.Y = ev.Entity.Position.Y;
            ev.Entity.Position.X = ev.To.X;
            ev.Entity.Position.Y = ev.To.Y;

            Server.Map.UpdateEntityPosition(ev.Entity, ev.Entity.LastPosition, ev.To);

            var nearPlayers = ev.Entity.GetPlayersNear();

            var movePacket = new EntityMovePacket()
            {
                To = ev.Entity.Position,
                UID = ev.Entity.UID
            };

            // Updating this movement to nearby players soo the client updates
            foreach (var nearPlayer in nearPlayers)
            {
                if(nearPlayer.UID != ev.Entity.UID) 
                    nearPlayer.Tcp.Send(movePacket);
            }

            if(typeof(LivingEntity).IsAssignableFrom(ev.Entity.GetType()))
            {
                // if im targeting something, i might attack it now after this movement
                var livingEntity = (LivingEntity)ev.Entity;
                if(livingEntity.Target != null)
                {
                    livingEntity.TryAttacking(livingEntity.Target);
                }

                // if someone is targeting me, he might be able to hit me now
                foreach(var targettingThisEntity in livingEntity.BeingTargetedBy)
                {
                    targettingThisEntity.TryAttacking(livingEntity);
                }
            }
        }
    }
}
