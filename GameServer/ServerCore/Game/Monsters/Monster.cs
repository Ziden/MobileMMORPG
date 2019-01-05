
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
            if(MovementBehaviour != null)
            {
                MovementBehaviour.PerformMovement(this);
                LastMovement = GameThread.TIME_MS_NOW;
                GameScheduler.Schedule(new SchedulerTask(MovementDelay, LastMovement)
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
