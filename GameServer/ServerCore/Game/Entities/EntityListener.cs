using Common.Networking.Packets;
using CommonCode.EventBus;
using ServerCore.Game.Monsters;
using ServerCore.GameServer.Entities;
using ServerCore.GameServer.Players.Evs;
using Common.Entity;
using ServerCore.Game.Combat;
using CommonCode.Networking.Packets;

namespace ServerCore.Game.Entities
{
    public class EntityListener : IEventListener
    {
        [EventMethod]
        public void OnEntityTarget(EntityTargetEvent ev)
        {
            ev.Entity.SetTarget(ev.TargetedEntity);
            ev.Entity.TryAttacking(ev.TargetedEntity);
        }

        [EventMethod]
        public void OnEntityDeath(EntityDeathEvent ev)
        {
            foreach (var player in ev.Entity.GetPlayersNear())
            {
                player.Tcp.Send(new EntityDeathPacket()
                {
                    EntityUID = ev.Entity.UID
                });
            }

            ev.Entity.Die();
        }

        [EventMethod]
        public void OnEntityDamage(EntityDamageEvent ev)
        {
            ev.Entity.HP -= ev.Damage;
            if (ev.Entity.HP <= 0)
            {
                Server.Events.Call(new EntityDeathEvent()
                {
                    Entity = ev.Entity
                });
            }
        }

        [EventMethod]
        public void OnEntityAttack(EntityAttackEvent ev)
        {
            var damage = ev.Attacker.Atk - ev.Defender.Def;

            Server.Events.Call(new EntityDamageEvent()
            {
                Damage = damage,
                Entity = ev.Defender,
                DamageCause = DamageCause.ATTACK
            });

            foreach (var player in ev.Defender.GetPlayersNear(includeSelf: true))
            {
                // If the player is the attacker he doesnt need his package
                player.Tcp.Send(new EntityAttackPacket()
                {
                    AttackerUID = ev.Attacker.UID,
                    DefenderUID = ev.Defender.UID,
                    Damage = damage
                });
            }
        }

        [EventMethod]
        public void OnEntitySpawn(EntitySpawnEvent ev)
        {
            ev.Entity.Position = ev.Position;
            Server.Map.UpdateEntityPosition(ev.Entity, from: null, to: ev.Entity.Position);

            // Track in monsters list if its a monster
            if (ev.Entity.EntityType == EntityType.MONSTER)
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
                if (nearPlayer.UID != ev.Entity.UID)
                    nearPlayer.Tcp.Send(movePacket);
            }

            // if im targeting something, i might attack it now after this movement
            var livingEntity = (LivingEntity)ev.Entity;
            if (livingEntity.Target != null)
            {
                livingEntity.TryAttacking(livingEntity.Target);
            }

            // if someone is targeting me, he might be able to hit me now
            foreach (var targettingThisEntity in livingEntity.BeingTargetedBy)
            {
                targettingThisEntity.TryAttacking(livingEntity);
            }
        }
    }
}
