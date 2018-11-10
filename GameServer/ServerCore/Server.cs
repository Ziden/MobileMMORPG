using AutoMapper;
using Common;
using Common.Networking.Packets;
using MapHandler;
using ServerCore.ConsoleCmds;
using ServerCore.Game.GameMap;
using ServerCore.GameServer.Players;
using ServerCore.Networking;
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
        public static WorldMap<ServerChunk> Map { get; set; }

        private bool _running = false;
        private readonly int PORT;

        private Server _instance;
        private ServerEvents _serverEvents;

        public Server(int port)
        {
            PORT = port;
            _instance = this;
            _serverEvents = new ServerEvents();
            CommandHandler = new CommandHandler();
            Map = MapLoader.LoadMapFromFile<ServerChunk>();

            Mapper.Initialize(cfg => {
                cfg.CreateMap<Player, OnlinePlayer>();
            });
        }

        public bool IsRunning()
        {
            return _running;
        }

        public void StartListeningForPackets()
        {
            TcpHandler = new ServerTcpHandler();
            TcpHandler.StartListening(PORT);
            _running = true;
        }

        public void StartGameThread()
        {
            GameThread = new GameThread();
            GameThread.Start();
            _running = true;
        }

        public void Stop()
        {
            GameThread.Abort();
            TcpHandler.Stop();
            _running = false;
        }

        public static OnlinePlayer GetPlayer(string UserId)
        {
            return Players.ToArray().First(p => p.UserId == UserId);
        }

        public static OnlinePlayer GetPlayerByConnectionId(string connectionId)
        {
            return Players.ToArray().First(p => p.Tcp.ConnectionId == connectionId);
        }

    }
}
