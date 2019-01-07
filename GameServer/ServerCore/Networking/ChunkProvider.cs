using Common.Entity;
using Common.Networking.Packets;
using MapHandler;
using ServerCore.Game.Entities;
using ServerCore.Game.Monsters;
using ServerCore.GameServer.Players;

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
                var chunkX = client.OnlinePlayer.Position.X >> 4;
                var chunkY = client.OnlinePlayer.Position.Y >> 4;

                var shouldBeLoaded = PositionExtensions.GetSquared3x3Around(new Position(chunkX, chunkY));

                foreach (var position in shouldBeLoaded)
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
                                ChunkData = chunk.TilePacketData
                            });

                            foreach(var entity in chunk.EntitiesInChunk[EntityType.MONSTER])
                            {
                                var monsterInstance = (Monster)entity;
                                client.Send(monsterInstance.ToPacket());
                            }
                        }
                    }
                }
            }
        }
    }
}
