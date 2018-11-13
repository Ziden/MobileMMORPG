using Storage.Players;

namespace ServerCore.GameServer.Players.Evs
{
    public class PlayerJoinEvent : IEvent
    {
        public OnlinePlayer Player;
    }
}
