using Assets.Code.Game.Factories;
using Client.Net;
using Common.Networking.Packets;
using CommonCode.EventBus;
using MapHandler;
using System;
using UnityEngine;

namespace Assets.Code.Net.PacketListeners
{
    public class MapListener : IEventListener
    {
        [EventMethod]
        public void OnMonsterSpawn(MonsterSpawnPacket packet)
        {
            MonsterFactory.BuildAndInstantiate(new MonsterFactoryOpts()
            {
                MonsterName = packet.MonsterName,
                MonsterUid = packet.MonsterUid,
                Position = packet.Position,
                SpriteIndex = packet.SpriteIndex,
                MoveSpeed = packet.MoveSpeed
            });
        }

        [EventMethod]
        public void OnChunkRecieve(ChunkPacket packet)
        {
            var chunkX = packet.X;
            var chunkY = packet.Y;
            var data = packet.ChunkData;

            var chunkParent = new GameObject("chunk_" + chunkX + "_" + chunkY);

            Chunk c = new Chunk()
            {
                x = chunkX,
                y = chunkY
            };

            Debug.Log("Rendering Chunk " + chunkX + " " + chunkY);
            for (var x = 0; x < 16; x++)
            {
                for(var y = 0; y < 16; y++)
                {
                    var tileId = data[x, y];
                    c.SetTile(x, y, tileId);
                    TileFactory.BuildAndInstantiate(chunkX * 16 + x, chunkY * 16 + y, tileId, chunkParent);
                }
            }

            // To be a tracked chunk
            UnityClient.Map.AddChunk(c);
        }
    }
}
