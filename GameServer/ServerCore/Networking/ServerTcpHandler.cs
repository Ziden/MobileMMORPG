using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;

namespace ServerCore.Networking
{
    public class ServerTcpHandler
    {
        private static Dictionary<string, ConnectedClientTcpHandler> _clientsByConnectionId = new Dictionary<string, ConnectedClientTcpHandler>();

        private TcpListener Listener { get; set; }
        public bool IsRunning { get; set; } = false;

        public Thread connectionsThread = null;

        public void StartListening(int port)
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            Listener = new TcpListener(address, port);

            Listener.Start();
            IsRunning = true;

            connectionsThread = new Thread(new ThreadStart(SocketConnectHandler));
            connectionsThread.IsBackground = true;
            connectionsThread.Start();

            Log.Info($"Server started. Listening to TCP clients at 127.0.0.1:{port}");
        }

        private void SocketConnectHandler()
        {
            while (IsRunning)
            {
                var clientTask = Listener.AcceptTcpClientAsync();
                if (clientTask.Result != null)
                {
                 
                    var socketClient = new ConnectedClientTcpHandler(clientTask.Result);
                    Log.Info($"Client {socketClient.ConnectionId} connected.");
                    _clientsByConnectionId.Add(socketClient.ConnectionId, socketClient);
                    ThreadPool.QueueUserWorkItem(ConnectedClientTcpHandler.RecievePacketWorker, socketClient);
                }
            }
        }

        public static ConnectedClientTcpHandler GetClient(string id)
        {
            return _clientsByConnectionId[id];
        }

        public static int ConnectedSockets()
        {
            return _clientsByConnectionId.Count;
        }

        public void Stop()
        {
            foreach (var client in _clientsByConnectionId.Values)
            {
                client.Stop();
            }
        }
    }
}