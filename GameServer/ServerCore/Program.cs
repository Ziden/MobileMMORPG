using Storage;
using Storage.TestDataBuilder;
using System;
using System.Linq;

namespace ServerCore
{
    class Program
    {
        private readonly static int SERVER_PORT = 8888; 

        static void Main(string[] args)
        {
            Log.Info("Connecting to Redis");
            Redis redis = new Redis();
            redis.Start();

            Log.Info("Creating Test Database");
            TestDb.Create();

            Log.Info("Starting Server");
            var server = new Server(new Server.ServerStartConfig() { Port = SERVER_PORT });
            server.StartListeningForPackets();
            server.StartGameThread();

            // Make the server thread join this thread so the code execution dont stop here 
            //server.TcpHandler.connectionsThread.Join();
            while(server.IsRunning())
            {
                string consoleInput = Console.ReadLine();

                string[] split = consoleInput.Split(" ");

                string cmd = split[0];

                var cmdArgs = split.Skip(1).ToArray();
                if(cmd == "help")
                {
                    Log.Info("Registered Commands:", ConsoleColor.Green);
                    var allCommands = Server.CommandHandler.RegisteredCommands();
                    var output = "help |";
                    foreach(var command in allCommands)
                    {
                        output += $" {command} |";
                    }
                    Log.Info(output, ConsoleColor.Green);

                }
                if (!Server.CommandHandler.RunCommand(cmd, cmdArgs))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Log.Error("[Console] Command Not Found. Use 'help' for help");
                }
            }
        }

    }
}
