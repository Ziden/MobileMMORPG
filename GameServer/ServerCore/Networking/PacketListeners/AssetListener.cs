using Common.Networking.Packets;
using CommonCode.EventBus;
using CommonCode.Networking.Packets;
using MapHandler;
using ServerCore.Game.GameMap;
using ServerCore.GameServer.Players.Evs;
using System.IO;

namespace ServerCore.Networking.PacketListeners
{
    // Assets pre-baked into the server
    public static class DefaultAssets
    {
        public static string SPR_BODIES = "bodies.png";
        public static string SPR_LEGS = "legs.png";
        public static string SPR_HEADS = "heads.png";
        public static string SPR_CHESTS = "chests.png";
        public static string SPR_MONTERS_1 = "monsters_1.png";
    }


    public class AssetListener : IEventListener
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

            foreach (var animationName in AssetLoader.GetAnimations())
            {
                client.Send(new AssetPacket()
                {
                    ResquestedImageName = animationName,
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

            var client = ServerTcpHandler.GetClient(packet.ClientId);

            // update chunks for that player
            ChunkProvider.CheckChunks(player);

            var playerPacket = player.ToPacket();

            // make the player itself appear
            client.Send(playerPacket);

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
                byte[] bytes = null;
                if (packet.AssetType == AssetType.TILESET)
                {
                    bytes = Server.Map.Tilesets[packet.ResquestedImageName];
                }
                else if (packet.AssetType == AssetType.SPRITE)
                {
                    bytes = AssetLoader.LoadImageData(packet.ResquestedImageName);
                }
                else if (packet.AssetType == AssetType.ANIMATION)
                {
                    bytes = AssetLoader.GetAnimation(packet.ResquestedImageName);
                }
                packet.Asset = bytes;
                ServerTcpHandler.GetClient(packet.ClientId).Send(packet);
            }
        }
    }
}
