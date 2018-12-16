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
            var packetsToProcess = UnityClient.PacketsToProccess.ToArray();

            // Not an asset, just process the packet
            var packet = UnityClient.PacketsToProccess[0];
            ClientEvents.Call(packet);
            UnityClient.PacketsToProccess.RemoveAt(0);
        }
    }
}
