using Common.Networking;
using Common.Networking.Packets;
using ServerCore.GameServer.Players;
using ServerCore.GameServer.Players.Evs;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ServerCore.Networking
{
    public class ConnectedClientTcpHandler
    {
        // keep track of what chunks ive already sent to this client
        public List<String> ChunksLoaded = new List<String>();

        public TcpClient TcpClient;
        public string ConnectionId;
        public OnlinePlayer OnlinePlayer;
        public int Latency = 100; // Default Latency for smoother movement interpolation
        public bool Listening = false;

        public static int PING_CHECK_SECONDS = 10;

        public bool Authenticated = false;

        public ConnectedClientTcpHandler()
        {
        }

        public void Configure()
        {
            TcpClient.NoDelay = true;
        }

        public virtual bool Send(BasePacket packet)
        {
            if (!Listening)
                return false;

            try
            {
                var packetDeserialized = PacketSerializer.Serialize(packet);
                var stream = TcpClient.GetStream();
                // first we write the size of the packet so we know how much to read later
                Int32 packetSize = (Int32)packetDeserialized.Length;
                var packetSizeBytes = BitConverter.GetBytes(packetSize);
                stream.Write(packetSizeBytes, 0, packetSizeBytes.Length);
                stream.Write(packetDeserialized, 0, packetDeserialized.Length);

                if (packet.GetType() != typeof(PingPacket)) // fucking spam...
                    Log.Debug("Sent Packet " + packet.GetType().Name);
            }
            catch (Exception e)
            {
                if(Listening)
                {
                    Listening = false;
                    DisconnectClient();
                }
            }
            return Listening;
        }

        public static void RecievePacketWorker(object client)
        {
            ((ConnectedClientTcpHandler)client).Recieve();
        }

        public void Recieve()
        {
            DateTime lastPingCheck = DateTime.MinValue;
            Log.Debug("Starting Listener for client " + ConnectionId);
            Listening = true;
            while (Listening)
            {
                try
                {
                    // Check if needs to ping the client to see if its ok
                    DateTime now = DateTime.Now;
                    if (now > lastPingCheck.AddSeconds(PING_CHECK_SECONDS))
                    {
                        lastPingCheck = now;
                        CheckPing();
                    }

                    // Read data from socket
                    var socketData = ReadData();
                    if (socketData == null)
                        continue;
                    var packetRead = PacketSerializer.Deserialize(socketData);

                    if (packetRead != null && packetRead is BasePacket)
                    {
                        var packet = (BasePacket)packetRead;
                        packet.ClientId = ConnectionId;

                        if (typeof(PingPacket) == packet.GetType())
                        {
                            RecievePing((PingPacket)packet);
                        }
                        if (typeof(LoginPacket) != packet.GetType())
                        {
                            if (!Authenticated)
                            {
                                Log.Error($"Blocked packet {packet.GetType().Name}");
                                continue;
                            }
                        }
                         Log.Debug($"Packet {packet.GetType().Name} recieved");
                        // Put the packet to be processed by the main thread
                        Server.PacketsToProccess.Enqueue(packet);
                    }
                }
                catch (Exception e)
                {
                    Listening = false;
                }
            }
            DisconnectClient();
        }

        public void DisconnectClient()
        {
            Server.Events.Call(new PlayerQuitEvent()
            {
                Client = this,
                Player = OnlinePlayer,
                Reason = QuitReason.DISCONNECTED
            });
            Stop();
        }

        public void Stop()
        {
            Listening = false;
            try
            {
                TcpClient?.GetStream().Flush();
            }
            catch (Exception e)
            {
                // all good
            }
            TcpClient?.Close();
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

        private void CheckPing()
        {
            Send(new PingPacket()
            {
                sendDate = DateTime.Now.ToString(Server.DATE_FORMAT)
            });
        }

        private void RecievePing(PingPacket packet)
        {
            var send = DateTime.ParseExact(packet.sendDate, Server.DATE_FORMAT, null);
            var recieveDate = DateTime.ParseExact(packet.recieveDate, Server.DATE_FORMAT, null);
            var now = DateTime.Now;

            var clientLatency = (now.Subtract(send)).Milliseconds / 2;
            var client = Server.TcpHandler.GetClient(packet.ClientId);
            client.Latency = clientLatency;
            Log.Debug($"Recieved ping from {client.ConnectionId} latency ={clientLatency}");
        }
    }
}
