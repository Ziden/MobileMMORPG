using Common.Networking.Packets;
using CommonCode.EventBus;
using CommonCode.Networking.Packets;
using MapHandler;
using ServerCore.Game.GameMap;
using ServerCore.GameServer.Players.Evs;
using System.IO;

namespace ServerCore.Networking.PacketListeners
{
    public class AssetListener : IEventListener
    {
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

            // check if the player have the main sprites
            client.Send(new AssetPacket()
            {
                ResquestedImageName = "sprites.png",
                AssetType = AssetType.SPRITE
            });

            client.Send(new AssetPacket()
            {
                ResquestedImageName = "bodies.png",
                AssetType = AssetType.SPRITE
            });

            client.Send(new AssetPacket()
            {
                ResquestedImageName = "legs.png",
                AssetType = AssetType.SPRITE
            });

            client.Send(new AssetPacket()
            {
                ResquestedImageName = "heads.png",
                AssetType = AssetType.SPRITE
            });

            client.Send(new AssetPacket()
            {
                ResquestedImageName = "chests.png",
                AssetType = AssetType.SPRITE
            });

            client.Send(new AssetPacket()
            {
                ResquestedImageName = "monsters_1.png",
                AssetType = AssetType.SPRITE
            });

            // end of assets validation
            client.Send(new AssetsReadyPacket());
        }

        [EventMethod] // When client finishes updating assets
        public void OnAssetReady(AssetsReadyPacket packet)
        {
            var player = Server.GetPlayer(packet.UserId);
            if(player != null)
            {
                player.AssetsReady = true;
            }

            var client = ServerTcpHandler.GetClient(packet.ClientId); 

            // update chunks for that player
            ChunkProvider.CheckChunks(player);

            // make the player itself appear
            client.Send(new PlayerPacket()
            {
                Name = player.Login,
                SpriteIndex = player.SpriteIndex,
                UserId = player.UserId,
                X = player.X,
                Y = player.Y,
                Speed = player.MoveSpeed
            });

            Server.Events.Call(new PlayerJoinEvent()
            {
                Player = player
            });
        }

        [EventMethod]
        public void OnAsset(AssetPacket packet)
        {
            // if client doesnt have the asset we gotta send it
            if(packet.HaveIt == false)
            {
                byte[] bytes = null;
                if(packet.AssetType == AssetType.TILESET)
                {
                    bytes = Server.Map.Tilesets[packet.ResquestedImageName];
                } else if(packet.AssetType == AssetType.SPRITE)
                {
                    bytes = MapLoader.LoadImageData(packet.ResquestedImageName);
                }
                packet.Asset = bytes;
                ServerTcpHandler.GetClient(packet.ClientId).Send(packet);
            }
        }
    }
}
