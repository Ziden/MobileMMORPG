using Common;
using Common.Networking.Packets;
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Client.Net
{
    public class ClientPacketListener : BaseThread
    {
        private readonly ClientTcpHandler _clientTcphandler;
        public bool Listen = true;

        public ClientPacketListener(ClientTcpHandler clientTcpHandler)
        {
            _clientTcphandler = clientTcpHandler;
        }

        public override void RunThread()
        {
            while (Listen)
            {
                try
                {
                    var packetRead = _clientTcphandler.ListenForPacket();
                    if (packetRead != null && packetRead is BasePacket)
                    {
                        var packet = (BasePacket)packetRead;
                        Debug.Log("Recieved Packet " + packet.GetType().Name);

                        if (typeof(PingPacket) == packet.GetType())
                        {
                            var pong = (PingPacket)packet;
                            pong.recieveDate = DateTime.Now.ToString(UnityClient.DATE_FORMAT);
                            _clientTcphandler.Send(pong);
                        }
                        else
                        {
                            UnityClient.PacketsToProccess.Add(packet);
                        }
                    }
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("ERROR PACKET LISTENER");
                    Console.WriteLine(e.Message + " " + e.StackTrace.ToString());
                }
            }
            // Disconnected for some reason
            Debug.Log("DISCONNECTED FROM SERVER ");
            _clientTcphandler.Stop();
            Listen = false;
        }
    }
}
