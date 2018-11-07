using Client.Net;
using Common.Networking.Packets;
using CommonCode.EventBus;
using CommonCode.Networking.Packets;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Code.Net.PacketListeners
{
    public class AssetListener : IEventListener
    {
        public enum AssetLoadingState
        {
            BEGINING = 1,
            RECIEVING = 2,
            UPDATED = 3
        }

        public static AssetLoadingState State = AssetLoadingState.UPDATED;

        public static int NumberOfAssetsToRecieve = 0;
        public static int AssetsRecieved = 0;

        [EventMethod]
        public void OnDialog(DialogPacket dialog)
        {
            GenericDialog.Get().Show("Dialog Packet", dialog.Message);
        }

        [EventMethod]
        public void OnAssetReady(AssetsReadyPacket assetReady)
        {
            Debug.Log("ASSET READY STATE " + State);
            if (State != AssetLoadingState.BEGINING)
            {
                State = AssetLoadingState.BEGINING;
            }
            else
            {
                State = AssetLoadingState.RECIEVING;
                NumberOfAssetsToRecieve = _assetRequests.Count;
                if (NumberOfAssetsToRecieve > 0)
                {
                    foreach (var packet in _assetRequests)
                    {
                        packet.HaveIt = false; // asking for the asset
                        UnityClient.TcpClient.Send(packet);
                        Debug.Log("Asking for asset " + packet.ResquestedImageName);
                    }
                    LoadingBehaviour.Loading.StartLoading("Downloading Assets");
                }
            }
        }

        private static List<AssetPacket> _assetRequests = new List<AssetPacket>();

        [EventMethod]
        public void OnAsset(AssetPacket packet)
        {

            Debug.Log("ASSET PACKET");
            // If im recieving from the server that i need an asset
            if (packet.Asset == null)
            {
                // if i dont have it
               // if (!File.Exists(Path.Combine(Application.persistentDataPath, packet.ResquestedImageName)))
               // {
                    _assetRequests.Add(packet);
              //  }
             //   else
            //    {
            //        var spriteMap = AssetHandler.LoadNewSprite(Path.Combine(Application.persistentDataPath, packet.ResquestedImageName));
            //        AssetHandler.LoadedAssets.Add(packet.ResquestedImageName, spriteMap);
           //         UnityClient.TcpClient.Send(new AssetsReadyPacket()
            //        {
           //             UserId = UnityClient.UserId
            //        });
            //    }
            }
            else
            {
                Debug.Log("Saving asset " + packet.ResquestedImageName);
                File.WriteAllBytes(Path.Combine(Application.persistentDataPath, packet.ResquestedImageName), packet.Asset);
                var spriteMap = AssetHandler.LoadNewSprite(Path.Combine(Application.persistentDataPath, packet.ResquestedImageName));
                AssetHandler.LoadedAssets.Add(packet.ResquestedImageName, spriteMap);
                AssetsRecieved++;
                if (AssetsRecieved == NumberOfAssetsToRecieve && AssetsRecieved > 0)
                {
                    LoadingBehaviour.Loading.StopLoading();
                    _assetRequests.Clear();
                    NumberOfAssetsToRecieve = 0;
                    AssetsRecieved = 0;
                    State = AssetLoadingState.UPDATED;
                    Debug.Log("Assets of user " + UnityClient.Player.UserId + " ready");
                    UnityClient.TcpClient.Send(new AssetsReadyPacket()
                    {
                        UserId = UnityClient.Player.UserId
                    });
                }
            }
        }
    }
}
