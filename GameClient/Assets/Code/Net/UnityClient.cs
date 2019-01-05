using Assets.Code.Game;
using Assets.Code.Game.ClientMap;
using Common;
using Common.Networking.Packets;
using MapHandler;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Net
{
    public static class UnityClient
    {
        public static PlayerWrapper Player = new PlayerWrapper();

        public static readonly string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss,fff";

        public static ClientTcpHandler TcpClient;

        public static ConcurrentList<BasePacket> PacketsToProccess = new ConcurrentList<BasePacket>();

        public static WorldMap<ClientChunk> Map = new WorldMap<ClientChunk>();

    }
}
