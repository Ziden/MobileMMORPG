using Common.Networking;
using Common.Networking.Packets;
using System;
using System.Net.Sockets;
using UnityEngine;

namespace Client.Net
{
    public class ClientTcpHandler
    {
        public readonly TcpClient TcpClient;

        public ClientPacketListener PacketListener;

        public ClientTcpHandler(string serverIp, int serverPort)
        {
            TcpClient = new TcpClient(serverIp, serverPort);
        }

        public ClientTcpHandler(TcpClient client)
        {
            TcpClient = client;
        }

        // use this only inside the server to send to a client
        public void Send(BasePacket packet)
        {
            try
            {
                var packetDeserialized = PacketSerializer.Serialize(packet);
                var stream = TcpClient.GetStream();
                // first we write the size of the packet so we know how much to read later, its out header
                Int32 packetSize = (Int32)packetDeserialized.Length;
                var packetSizeBytes = BitConverter.GetBytes(packetSize);
                stream.Write(packetSizeBytes, 0, packetSizeBytes.Length);
                // then write the friggin packet
                stream.Write(packetDeserialized, 0, packetDeserialized.Length);
                Debug.Log("Sent Packet " + packet.GetType().Name);
            }
            catch (Exception e)
            {
                // Call in main thread
                Debug.Log("SOCKET ERROR");
                this.Stop();
            }
        }

        public void Stop()
        {
            Debug.Log("Stopping Client Listener Thread");
            if (PacketListener != null)
                PacketListener.Listen = false;
            TcpClient.Close(); 
        }

        public bool IsListening()
        {
            return PacketListener != null && PacketListener.Listen;
        }

        public void StartListening()
        {
            PacketListener = new ClientPacketListener(this);
            PacketListener.Start();
        }

        public object ListenForPacket()
        {
            var socketData = ReadData();
            if (socketData != null)
                return PacketSerializer.Deserialize(socketData);
            return null;
        }

        private byte[] ReadData()
        {
            var stream = TcpClient.GetStream();
            if (stream.DataAvailable)
            {
                // first we read the header telling us the packet size
                var packetSizeHeader = new byte[4];
                stream.Read(packetSizeHeader, 0, 4);
                var packetSize = BitConverter.ToInt32(packetSizeHeader, 0);

                // then we read the next packet
                var bytesRead = new byte[packetSize];
                stream.Read(bytesRead, 0, packetSize);
                return bytesRead;
            }
            return null;
        }

        public bool CanRead()
        {
            return TcpClient.Connected;
        }

    }
}
