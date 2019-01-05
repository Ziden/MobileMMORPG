using Assets.Code.Game.ClientMap;
using Assets.Code.Game.Factories;
using Client.Net;
using Common.Networking.Packets;
using CommonCode.EntityShared;
using CommonCode.EventBus;
using MapHandler;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Net.PacketListeners
{
    public class MapListener : IEventListener
    {
        [EventMethod]
        public void OnMonsterSpawn(MonsterPacket packet)
        {
            MonsterFactory.BuildAndInstantiate(new MonsterFactoryOpts()
            {
                Position = packet.Position,
                Packet = packet
            });

            if(packet.SpawnAnimation)
            {
                AnimationFactory.BuildAndInstantiate(new AnimationOpts()
                {
                    AnimationImageName = DefaultAssets.ANM_SMOKE,
                    MapPosition = packet.Position
                });
            }
        }

        [EventMethod]
        public void OnChunkRecieve(ChunkPacket packet)
        {
            var chunkX = packet.X;
            var chunkY = packet.Y;
            var data = packet.ChunkData;
            
            if (!UnityClient.Map.Chunks.Any(cp => cp.Key.Equals($"{chunkX}_{chunkY}")))
            {
                var chunkParent = new GameObject($"chunk_{chunkX}_{chunkY}");

                ClientChunk c = new ClientChunk()
                {
                    x = chunkX,
                    y = chunkY,
                    GameObject = chunkParent
                };

                Debug.Log("Rendering Chunk " + chunkX + " " + chunkY);
                for (var x = 0; x < 16; x++)
                {
                    for (var y = 0; y < 16; y++)
                    {
                        var tileId = data[x, y];

                        var tilePosition = new Position(chunkX * 16 + x, chunkY * 16 + y);

                        c.Tiles[x, y] = new MapTile(tilePosition, tileId);

                        TileFactory.BuildAndInstantiate(new TileOptions()
                        {
                            Parent = chunkParent,
                            Position = tilePosition,
                            TileId = tileId
                        });
                    }
                }

                // To be a tracked chunk
                UnityClient.Map.AddChunk(c);
            }
        }
    }
}
