using Common.Networking.Packets;
using CommonCode.EntityShared;
using CommonCode.EventBus;
using CommonCode.Networking.Packets;
using MapHandler;
using ServerCore.Game.GameMap;
using ServerCore.GameServer.Players.Evs;
using System.IO;

namespace ServerCore.Networking.PacketListeners
{
    public class AssetPacketListener : IEventListener
    {
        // This looks horrible, seriously
        // i gotta refactor this piece of shizza
        // maybe bundle stuff up ?
        public static void DownloadAssets(ConnectedClientTcpHandler client)
        {
            // Going to start sending asset validations
            client.Send(new AssetsReadyPacket());

            // check if the player already have the tilesets
            foreach (var tileset in Server.Map.Tilesets)
            {
                client.Send(new AssetPacket()
                {
                    ResquestedImageName = tileset.Key,
                    AssetType = AssetType.TILESET
                });
            }

            client.Send(new AssetPacket()
            {
                ResquestedImageName = DefaultAssets.SPR_BODIES,
                AssetType = AssetType.SPRITE
            });

            client.Send(new AssetPacket()
            {
                ResquestedImageName = DefaultAssets.SPR_LEGS,
                AssetType = AssetType.SPRITE
            });

            client.Send(new AssetPacket()
            {
                ResquestedImageName = DefaultAssets.SPR_HEADS,
                AssetType = AssetType.SPRITE
            });

            client.Send(new AssetPacket()
            {
                ResquestedImageName = DefaultAssets.SPR_CHESTS,
                AssetType = AssetType.SPRITE
            });

            client.Send(new AssetPacket()
            {
                ResquestedImageName = DefaultAssets.SPR_MONTERS_1,
                AssetType = AssetType.SPRITE
            });

            client.Send(new AssetPacket()
            {
                ResquestedImageName = DefaultAssets.SPR_WEAPONS,
                AssetType = AssetType.ITEMS
            });

            var animations = AssetLoader.LoadedAssets[AssetType.ANIMATION];

            foreach (var animationAsset in animations)
            {
                client.Send(new AssetPacket()
                {
                    ResquestedImageName = animationAsset.ImageName,
                    AssetType = AssetType.ANIMATION
                });
            }

            // end of assets validation
            client.Send(new AssetsReadyPacket());
        }

        [EventMethod] // When client finishes updating assets
        public void OnAssetReady(AssetsReadyPacket packet)
        {
            var players = Server.Players;
            var player = Server.GetPlayer(packet.UserId);
            if (player != null)
            {
                player.AssetsReady = true;
            }

            var client = Server.TcpHandler.GetClient(packet.ClientId);

            // update chunks for that player
            ChunkProvider.CheckChunks(player);

            // make the player itself appear
            var playerPacket = player.ToPacket();
            client.Send(playerPacket);

            // to track the entity spawning caches etc
            Server.Events.Call(new EntitySpawnEvent()
            {
                Entity = player,
                Position = player.Position
            });

            // to handle packets when a player joins
            Server.Events.Call(new PlayerJoinEvent()
            {
                Player = player
            });
        }

        [EventMethod]
        public void OnAsset(AssetPacket packet)
        {
            // if client doesnt have the asset we gotta send it
            if (packet.HaveIt == false)
            {
                var assetType = packet.AssetType;
                var asset = AssetLoader.LoadedAssets.GetAsset(packet.AssetType, packet.ResquestedImageName);
                var bytes = asset.ImageData;
                packet.Asset = bytes;
                Server.TcpHandler.GetClient(packet.ClientId).Send(packet);
            }
        }
    }
}
