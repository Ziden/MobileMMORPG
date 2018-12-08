
using ServerCore.Assets;
using ServerCore.Game.Entities;
using ServerCore.Game.GameMap;
using ServerCore.Game.Monsters.Behaviours;
using ServerCore.Networking.PacketListeners;
using ServerCore.Utils.Scheduler;
using System;

namespace ServerCore.Game.Monsters
{
    public abstract class Monster : LivingEntity, IClientRenderable
    {
        public long LastMovement = 0;

        public IMonsterMovement MovementBehaviour;

        public MonsterSpawner OriginSpawner;

        public Monster()
        {
            UID = $"mon_{Guid.NewGuid().ToString()}";
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
