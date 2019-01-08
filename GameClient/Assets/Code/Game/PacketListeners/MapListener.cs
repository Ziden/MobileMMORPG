using Assets.Code.Game.Factories;
using Common.Networking.Packets;
using CommonCode.EntityShared;
using CommonCode.EventBus;

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
            ChunkManager.Instance().LoadChunk(packet);
        }
    }
}
