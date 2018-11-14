using MapHandler;
using ServerCore.Game.Monsters;

namespace ServerCore.GameServer.Players.Evs
{
    public class MonsterSpawnEvent : IEvent
    {
        public Monster Monster;
        public Position Position;
    }
}
