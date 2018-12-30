using Assets.Code.Game;
using Client.Net;
using Common.Scheduler;
using System;
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
        var now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        GameScheduler.RunTasks(now);
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
