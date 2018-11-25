using Common.Networking.Packets;
using MapHandler;
using ServerCore.GameServer.Players.Evs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCore.Game.Monsters.Behaviours.MoveBehaviours
{
    public class RandomWalk : IMonsterMovement
    {
        public void PerformMovement(Monster monster)
        {
            var oldPosition = monster.Position;
            var newPosition = new Position(oldPosition.X, oldPosition.Y);
            var rnd = new Random().Next(1, 5);

            switch (rnd)
            {
                case 1:
                    newPosition.X += 1; break;
                case 2:
                    newPosition.Y += 1; break;
                case 3:
                    newPosition.X -= 1; break;
                case 4:
                    newPosition.Y -= 1; break;
                default:
                    break;
            }

            if (Server.Map.IsPassable(newPosition.X, newPosition.Y))
            {
                var oldNearPlayers = monster.GetNearbyPlayers();
                monster.Position = newPosition;

                var monsterMoveEvent = new MonsterMoveEvent()
                {
                    Monster = monster,
                    From = oldPosition,
                    To = newPosition
                };

                if (!MapHelpers.IsSameChunk(newPosition, oldPosition))
                {
                    monsterMoveEvent.ChangedChunk = true;
                }

                Server.Events.Call(monsterMoveEvent);

                if (monsterMoveEvent.ChangedChunk)
                {
                    var chunk = Server.Map.GetChunk(newPosition.X >> 4, newPosition.Y >> 4);
                    var oldChunk = Server.Map.GetChunk(oldPosition.X >> 4, oldPosition.Y >> 4);

                    oldChunk.MonstersInChunk.Remove(monster);
                    chunk.MonstersInChunk.Add(monster);
                }

                var newNearPlayers = monster.GetNearbyPlayers();
                // if the monster moved chunk, some players wont be in the new list
                // so we gotta track them as well
                if (monsterMoveEvent.ChangedChunk)
                {
                    newNearPlayers.AddRange(
                        oldNearPlayers.Where(p => !newNearPlayers.Contains(p))
                    );
                }

                foreach (var player in monster.GetNearbyPlayers())
                {
                    player.Tcp.Send(new EntityMovePacket()
                    {
                        From = oldPosition,
                        To = newPosition,
                        UID = monster.UID
                    });
                }
            }

        }
    }
}
