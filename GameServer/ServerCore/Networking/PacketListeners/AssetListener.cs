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
                Speed = player.speed
            });

            ServerEvents.Call(new PlayerJoinEvent()
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
