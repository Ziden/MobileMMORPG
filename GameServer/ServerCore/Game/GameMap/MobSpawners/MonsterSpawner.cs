using Common.Networking.Packets;
using MapHandler;
using ServerCore.Game.GameMap.MobSpawners;
using ServerCore.Game.Monsters;
using ServerCore.GameServer.Players.Evs;
using System;
using System.Collections.Generic;

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

                    Server.Events.Call(new MonsterSpawnEvent()
                    {
                        Monster = monsterInstance,
                        Position = spawnPosition
                    });

                    var chunkX = spawnPosition.X >> 4;
                    var chunkY = spawnPosition.Y >> 4;

                    var chunk = Server.Map.GetChunk(chunkX, chunkY);

                    chunk.MonstersInChunk.Add(monsterInstance);
                    Server.Map.Monsters.Add(monsterInstance.UID, monsterInstance);

                    // Let players know this monster spawned
                    foreach (var player in monsterInstance.GetNearbyPlayers())
                    {
                        player.Tcp.Send(new MonsterSpawnPacket()
                        { 
                            MonsterUid = monsterInstance.UID,
                            MonsterName = monsterInstance.Name,
                            Position = monsterInstance.Position,
                            SpriteIndex = monsterInstance.GetSpriteAsset().SpriteRowIndex,
                            MoveSpeed = monsterInstance.MoveSpeed,
                            SpawnAnimation = true
                        });
                    }

                    monsterInstance.MovementTick();
                }
            }
        }

    }
}
