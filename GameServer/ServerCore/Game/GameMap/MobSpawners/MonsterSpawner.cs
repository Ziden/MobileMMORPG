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
                var randomX = rnd.Next(minX, maxX);
                var randomY = rnd.Next(minY, maxY);
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

                    Server.Events.Call(new MonsterSpawnEvent()
                    {
                        Monster = monsterInstance,
                        Position = spawnPosition
                    });

                    var chunkX = spawnPosition.X >> 4;
                    var chunkY = spawnPosition.Y >> 4;

                    var chunk = Server.Map.GetChunk(chunkX, chunkY);

                    chunk.MonstersInChunk.Add(monsterInstance);

                    // Let players know this monster spawned
                    foreach (var player in monsterInstance.GetNearbyPlayers())
                    {
                        player.Tcp.Send(new MonsterSpawnPacket()
                        { 
                            MonsterUid = monsterInstance.UID,
                            MonsterName = monsterInstance.Name,
                            Position = monsterInstance.Position,
                            SpriteIndex = monsterInstance.SpriteIndex,
                            MoveSpeed = monsterInstance.Speed,
                            SpawnAnimation = true
                        });
                    }

                    monsterInstance.MovementTick();
                }
            }
        }

    }
}
