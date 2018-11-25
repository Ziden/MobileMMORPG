using AutoMapper;
using Common;
using Common.Networking.Packets;
using MapHandler;
using ServerCore.ConsoleCmds;
using ServerCore.Game.GameMap;
using ServerCore.GameServer.Players;
using ServerCore.Networking;
using ServerCore.Utils.Scheduler;
using Storage.Players;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
        

        public Server(int port)
        {
            PORT = port;
            _instance = this;
            Events?.Clear();
            Events = new ServerEvents();
            CommandHandler = new CommandHandler();
            Map = MapLoader.LoadMapFromFile("test");
            Map.LoadAllSpawners();
            Mapper.Reset();
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Player, OnlinePlayer>();
            });
        }

        public bool IsRunning()
        {
            return Running;
        }

        public void StartListeningForPackets()
        {
            TcpHandler = new ServerTcpHandler();
            TcpHandler.StartListening(PORT);
            Running = true;
        }

        public void StartGameThread()
        {
            GameThread = new GameThread();
            GameThread.Start();
            Running = true;
        }

        public void Stop()
        {
            GameScheduler.Tasks.Clear();
            Events.Clear();
            GameThread.Stop();
            TcpHandler?.Stop();
            Running = false;
        }

        public static OnlinePlayer GetPlayer(string UserId)
        {
            return Players.ToArray().FirstOrDefault(p => p.UserId == UserId);
        }

        public static OnlinePlayer GetPlayerByConnectionId(string connectionId)
        {
            return Players.ToArray().FirstOrDefault(p => p.Tcp.ConnectionId == connectionId);
        }

    }
}
