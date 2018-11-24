using MapHandler;
using ServerCore.Game.Monsters.Behaviours;
using ServerCore.GameServer.Players;
using ServerCore.Utils.Scheduler;
using System;
using System.Collections.Generic;

namespace ServerCore.Game.Monsters
{
    public abstract class Monster
    {
        public string UID;
        public string Name;
        public Position Position;
        public int SpriteIndex = 2;
        public int Speed = 5;
        public long MovementDelay = 2000; // in millis
        public long LastMovement = 0;

        public IMonsterMovement MovementBehaviour;

        public Monster()
        {
            UID = $"mon_{Guid.NewGuid().ToString()}";
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

        public List<OnlinePlayer> GetNearbyPlayers()
        {
            List<OnlinePlayer> near = new List<OnlinePlayer>();
            var radius = MapHelpers.GetSquared3x3(new Position(Position.X >> 4, Position.Y >> 4));
            foreach (var position in radius)
            {
                var chunkThere = Server.Map.GetChunk(position.X, position.Y);
                if (chunkThere != null)
                {
                    foreach (var playerInChunk in chunkThere.PlayersInChunk)
                    {
                        near.Add(playerInChunk);
                    }
                }
            }
            return near;
        }
    }
}
