using Common.Networking.Packets;
using MapHandler;
using ServerCore.Game.GameMap.MobSpawners;
using ServerCore.Game.Monsters;
using ServerCore.GameServer.Players.Evs;
using System;
using System.Collections.Generic;
using Common.Entity;
using ServerCore.GameServer.Entities;

namespace ServerCore.Game.GameMap
{
    public class MonsterSpawner : MapRegion
    {
        public List<SpawnerMob> SpawnerMobs = new List<SpawnerMob>();

        public Position FindSpawnPosition()
        {
            var rnd = new Random();
            var found = false;
            var tries = 10;
            while (tries > 0 && !found)
            {
                tries--;
                var randomX = rnd.Next(Min.X, Max.X);
                var randomY = rnd.Next(Min.Y, Max.Y);
                if (!Server.Map.IsPassable(randomX, randomY))
                    continue;

                return new Position(randomX, randomY);
            }
            return null;
        }

        public void SpawnTick()
        {
            foreach (var mob in SpawnerMobs)
            {
                if (mob.TrackedMonster == null)
                {
                    var monsterInstance = (Monster)Activator.CreateInstance(mob.MonsterClassType);
                    var spawnPosition = FindSpawnPosition();
                    if (spawnPosition == null)
                        return;

                    monsterInstance.Position = spawnPosition;
                    monsterInstance.OriginSpawner = this;

                    var spawnEvent = new EntitySpawnEvent()
                    {
                        Entity = monsterInstance,
                        Position = spawnPosition
                    };
                    Server.Events.Call(spawnEvent);

                    // Let players know this monster spawned
                    foreach (var player in monsterInstance.GetPlayersNear())
                    {
                        var monsterPacket = monsterInstance.ToPacket();
                        monsterPacket.SpawnAnimation = true;
                        player.Tcp.Send(monsterPacket);
                    }

                    monsterInstance.MovementTick();
                }
            }
        }

    }
}
