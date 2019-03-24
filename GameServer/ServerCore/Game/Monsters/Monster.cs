
using ServerCore.Assets;
using ServerCore.Game.Entities;
using ServerCore.Game.GameMap;
using ServerCore.Game.Monsters.Behaviours;
using Common.Scheduler;
using System;
using Common.Entity;
using Common.Networking.Packets;

namespace ServerCore.Game.Monsters
{
    public abstract class Monster : LivingEntity, IClientRenderable
    {
        public long LastMovement = 0;
        public long MovementDelay = 2000; // in millis
        public MonsterSpawner OriginSpawner;
        public abstract SpriteAsset GetSprite();
        public Guid MovementTaskId = Guid.Empty;

        public Monster()
        {
            UID = $"mon_{Guid.NewGuid().ToString()}";
            EntityType = EntityType.MONSTER;
        }

        // Behaviours
        public IMonsterAggro AggroBehaviuor;
        public IMonsterMovement MovementBehaviour;

        public SpriteAsset GetSpriteAsset()
        {
            return GetSprite();
        }

        public MonsterPacket ToPacket()
        {
            return new MonsterPacket()
            {
                MonsterUid = this.UID,
                MonsterName = this.Name,
                Position = this.Position,
                SpriteIndex = this.GetSpriteAsset().SpriteRowIndex,
                MoveSpeed = this.MoveSpeed,
                Atk = this.Atk,
                Def = this.Def,
                AtkSpeed = this.AtkSpeed,
                HP = this.HP,
                MAXHP = this.MAXHP
            };
        }

        public void FromPacket(MonsterPacket packet)
        {
            UID = packet.MonsterUid;
            Name = packet.MonsterName;
            Position = packet.Position;
            GetSpriteAsset().SpriteRowIndex = packet.SpriteIndex;
            MoveSpeed = packet.MoveSpeed;
            Atk = packet.Atk;
            Def = packet.Def;
            AtkSpeed = packet.AtkSpeed;
            HP = packet.HP;
            MAXHP = packet.MAXHP;
        }

        public void MovementTick()
        {
            if (MovementTaskId == Guid.Empty)
                GameScheduler.CancelTask(MovementTaskId);

            MovementBehaviour?.PerformMovement(this);
            LastMovement = GameThread.TIME_MS_NOW;
            this.MovementTaskId = GameScheduler.Schedule(new SchedulerTask(MovementDelay, LastMovement)
            {
                Task = () =>
                {
                    MovementTaskId = Guid.Empty;
                    if(HP > 0)
                        MovementTick();
                }
            });
        }

        public override void BeTargeted(LivingEntity entity)
        {
            base.BeTargeted(entity);
            this.AggroBehaviuor.OnBeingTargeted(this, entity);
        }

        public override void Die()
        {
            Log.Debug(this.Name + " Died");
            Server.Map.UpdateEntityPosition(this, from: this.Position, to: null);
            Server.Map.Monsters.Remove(this.UID);
            this.OriginSpawner?.CreateSpawnTask();
            GameScheduler.CancelTask(MovementTaskId);
            GameScheduler.CancelTask(AttackTaskId);
        }
    }
}
