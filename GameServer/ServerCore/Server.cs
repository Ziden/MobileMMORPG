using Common;
using Common.Networking.Packets;
using ServerCore.ConsoleCmds;
using ServerCore.Game.GameMap;
using ServerCore.Game.Monsters;
using ServerCore.GameServer.Players;
using ServerCore.Networking;
using Common.Scheduler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ServerCore
{
    public class Server
    {
        public static readonly string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss,fff";

        public static ConcurrentList<OnlinePlayer> Players = new ConcurrentList<OnlinePlayer>();

        public static ConcurrentQueue<BasePacket> PacketsToProccess = new ConcurrentQueue<BasePacket>();

        public static ServerTcpHandler TcpHandler { get; set; }
        public static GameThread GameThread { get; set; }
        public static CommandHandler CommandHandler { get; set; }
        public static ServerMap Map { get; set; }
        public static ServerEvents Events;

        public static bool Running = false;
        private readonly int PORT;

        private Server _instance;

        public Server(ServerStartConfig config)
        {
            PORT = config.Port;
            _instance = this;
            Events?.Clear();
            Events = new ServerEvents();
            CommandHandler = new CommandHandler();
            TcpHandler = new ServerTcpHandler();
            AssetLoader.LoadServerAssets();
            Map = AssetLoader.LoadMapFromFile(config.MapName);
            if (!config.DisableSpawners)
                Map.LoadAllSpawners();
        }

        public bool IsRunning()
        {
            return Running;
        }

        public void StartListeningForPackets()
        {
            TcpHandler.StartListening(PORT);
            Running = true;
        }

        public void StartGameThread()
        {
            GameThread = new GameThread();
            GameThread.Start();
            Running = true;
        }

        // mainly for tests to make sure everything gets cleaned up
        public void Stop()
        {
            AssetLoader.Clear();
            GameScheduler.Tasks.Clear();
            Events.Clear();
            GameThread?.Stop();
            TcpHandler?.Stop();
            Running = false;
            Server.Map.Chunks = new Dictionary<string, ServerChunk>();
            foreach(var player in Players.ToList())
            {
                player.Tcp.Stop();
            }
            Players.Clear();
            PacketsToProccess.Clear();
    }

        public static OnlinePlayer GetPlayer(string UserId)
        {
            return Players.ToArray().FirstOrDefault(p => p.UID == UserId);
        }

        public static OnlinePlayer GetPlayerByConnectionId(string connectionId)
        {
            return Players.ToArray().FirstOrDefault(p => p.Tcp.ConnectionId == connectionId);
        }

        public static Monster GetMonster(string monsterUid)
        {
            return Map.Monsters[monsterUid];
        }

        public class ServerStartConfig
        {
            public bool DisableSpawners = false;
            public int Port;
            public string MapName = "test";
        }

    }
}
