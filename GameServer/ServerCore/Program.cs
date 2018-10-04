using Networking;
using Storage;
using System;

namespace ServerCore
{
    class Program
    {
        private readonly static int SERVER_PORT = 8888;

        static void Main(string[] args)
        {
            // Starting Redis
            Redis redis = new Redis();
            redis.Start();

            // Starting the Server
            TcpHandler.StartServer(SERVER_PORT);
            TcpHandler.Listen(); 
        }
    }
}
