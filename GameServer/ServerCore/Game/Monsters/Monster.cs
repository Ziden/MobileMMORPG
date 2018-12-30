
using ServerCore.Assets;
using ServerCore.Game.Entities;
using ServerCore.Game.GameMap;
using ServerCore.Game.Monsters.Behaviours;
using ServerCore.Networking.PacketListeners;
using Common.Scheduler;
using System;
using Common.Entity;

namespace ServerCore.Game.Monsters
{
    public abstract class Monster : LivingEntity, IClientRenderable
    {
        public long LastMovement = 0;
        public long MovementDelay = 2000; // in millis
        public IMonsterMovement MovementBehaviour;

        public MonsterSpawner OriginSpawner;

        public Monster()
        {
            UID = $"mon_{Guid.NewGuid().ToString()}";
            EntityType = EntityType.MONSTER;
        }

        public abstract SpriteAsset GetSprite();

        public SpriteAsset GetSpriteAsset()
        {
            return GetSprite();
        }

        public void MovementTick()
        {
            if(MovementBehaviour != null)
            {
                MovementBehaviour.PerformMovement(this);
                LastMovement = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                GameScheduler.Schedule(new SchedulerTask(MovementDelay)
                {
                    Task = () =>
                    {
                        MovementTick();
                    }
                });
            }
        }
    }
}
