using MapHandler;
using ServerCore.Game.Monsters;

namespace ServerCore.GameServer.Players.Evs
{
    public class MonsterMoveEvent : IEvent
    {
        public Monster Monster;
        public Position From;
        public Position To;
        public bool ChangedChunk = false;

        public bool IsCancelled = false;

    }
}
