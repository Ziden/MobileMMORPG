using System;

namespace GameServer
{
    public class Main
    {

        public static readonly Int16 SERVER_PORT = 8888;

        public static void Main(string[] args)
        {
            TcpHelper.StartServer(SERVER_PORT);
            TcpHelper.Listen(); 
        }
    }
}
