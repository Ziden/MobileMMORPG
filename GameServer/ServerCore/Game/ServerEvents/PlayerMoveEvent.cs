using MapHandler;

namespace ServerCore.GameServer.Players.Evs
{
    public class PlayerMoveEvent : IEvent
    {
        public OnlinePlayer Player;
        public Position From;
        public Position To;

        public bool IsCancelled = false;

    }
}
