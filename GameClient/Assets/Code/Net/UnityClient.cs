using Assets.Code.Game;
using Client.Net;
using Common;
using Common.Networking.Packets;
using MapHandler;

namespace Client.Net
{
    public static class UnityClient
    {
        public static PlayerWrapper Player = new PlayerWrapper();

        public static readonly string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss,fff";

        public static ClientTcpHandler TcpClient;

        public static ConcurrentList<BasePacket> PacketsToProccess = new ConcurrentList<BasePacket>();

        public static WorldMap<Chunk> Map = new WorldMap<Chunk>();
    }
}
