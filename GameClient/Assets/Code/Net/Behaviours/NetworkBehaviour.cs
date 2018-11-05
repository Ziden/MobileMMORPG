using Assets.Code.Game;
using Assets.Code.Net;
using Client.Net;
using UnityEngine;

public class NetworkBehaviour : MonoBehaviour
{

    public static bool Looping = true;

    void Start()
    {

    }

    private void Update()
    {
        if (!Looping)
            return;
        ProcessPackets();
    }

    private void ProcessPackets()
    {
        if (UnityClient.PacketsToProccess.Count > 0)
        {
            // Are we recieving an asset ? (We wait to recieve it all to process other stuff)
            var missingAssets = AssetHandler.WaitingForAssets.ToArray();
            var packetsToProcess = UnityClient.PacketsToProccess.ToArray();

            // Not an asset, just process the packet
            var packet = UnityClient.PacketsToProccess[0];
            Debug.Log("Calling " + packet.GetType().Name);
            ClientEvents.Call(packet);
            UnityClient.PacketsToProccess.RemoveAt(0);
            Debug.Log("Called");
        }
    }
}
