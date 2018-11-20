using MapHandler;
using ServerCore.GameServer.Players;
using System;
using System.Collections.Generic;

namespace ServerCore.Game.Monsters
{
    public class Monster
    {
        public string uuid;
        public string Name;
        public int X;
        public int Y;
        public int SpriteIndex = 2;

        public Monster()
        {
            uuid = $"mon_{Guid.NewGuid().ToString()}";
        }

        public void MovementTick()
        {
            //
        }

        public List<OnlinePlayer> GetNearbyPlayers(int searchRadius)
        {
            List<OnlinePlayer> near = new List<OnlinePlayer>();
            var chunk = Server.Map.GetChunk(X >> 4, Y >> 4);
            var radius = MapUtils.GetRadius(chunk.x, chunk.y, searchRadius);
            foreach (var position in radius)
            {
                var chunkThere = Server.Map.GetChunk(position.X, position.Y);
                if (chunkThere != null)
                {
                    foreach (var playerInChunk in chunkThere.PlayersInChunk)
                    {
                        near.Add(playerInChunk);
                    }
                }
            }
            return near;
        }
    }
}
