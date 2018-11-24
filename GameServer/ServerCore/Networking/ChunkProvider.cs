using Common.Networking.Packets;
using MapHandler;
using ServerCore.GameServer.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore.Networking
{
    public static class ChunkProvider
    {
        public static int VIEW_RADIUS = 3;

        public static void CheckChunks(this OnlinePlayer player)
        {
            var client = player.Tcp;
            if (client.OnlinePlayer != null && client.OnlinePlayer.AssetsReady)
            {
                var chunkX = client.OnlinePlayer.X >> 4;
                var chunkY = client.OnlinePlayer.Y >> 4;

                List<Position> shouldBeLoaded = MapUtils.GetRadius(chunkX, chunkY, VIEW_RADIUS);

                foreach(var position in shouldBeLoaded)
                {
                    var chunkKey = $"{position.X}_{position.Y}";
                    if (!client.ChunksLoaded.Contains(chunkKey))
                    {
                        if(Server.Map.Chunks.ContainsKey(chunkKey))
                        {
                            client.ChunksLoaded.Add(chunkKey);
                            var chunk = Server.Map.Chunks[chunkKey];
                            client.Send(new ChunkPacket()
                            {
                                X = position.X,
                                Y = position.Y,
                                ChunkData = chunk.GetData()
                            });

                            foreach(var monsterInstance in chunk.MonstersInChunk)
                            {
                                client.Send(new MonsterSpawnPacket()
                                {
                                    MonsterUid = monsterInstance.UID,
                                    MonsterName = monsterInstance.Name,
                                    Position = monsterInstance.Position,
                                    SpriteIndex = monsterInstance.SpriteIndex,
                                    MoveSpeed = monsterInstance.Speed,
                                    SpawnAnimation = false
                                });
                            }
                        }
                    }
                }
            }
        }
    }
}
