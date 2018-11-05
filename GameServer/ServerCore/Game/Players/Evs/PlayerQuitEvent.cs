using ServerCore.Networking;
using Storage.Players;

namespace ServerCore.GameServer.Players.Evs
{
    public class PlayerQuitEvent : IEvent
    {
        public ConnectedClientTcpHandler Client;
        public OnlinePlayer Player;
        public QuitReason Reason;
    }

    public enum QuitReason
    {
        DISCONNECTED = 0,
        KICKED = 1
    }
}
