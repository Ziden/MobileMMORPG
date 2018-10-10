using Networking;
using Storage;
using Storage.TestDataBuilder;
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

            // Create test db to mock stuff
            TestDb.Create();

            // Starting the Server
            TcpHandler.StartServer(SERVER_PORT);
            TcpHandler.Listen(); 
        }
    }
}
