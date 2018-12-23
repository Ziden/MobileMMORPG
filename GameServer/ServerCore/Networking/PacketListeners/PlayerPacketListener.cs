using Common.Networking.Packets;
using CommonCode.EventBus;
using CommonCode.EntityShared;
using MapHandler;
using ServerCore.GameServer.Players.Evs;
using Storage.Players;
using System;

namespace ServerCore.Networking.PacketListeners
{
    public class PlayerPacketListener : IEventListener
    {
        [EventMethod] 
        // server recieving entitymove means its a player moving
        public void OnEntityMovePacket(EntityMovePacket packet)
        {
            var player = Server.GetPlayerByConnectionId(packet.ClientId);
            var distanceMoved = PositionExtensions.GetDistance(player.Position, packet.To);
            var timeToMove = Formulas.GetTimeToMoveBetweenTwoTiles(player.MoveSpeed);

            Log.Info("TIME TO MOVE " + timeToMove);

            var now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            // Player tryng to hack ?
            if (distanceMoved > 1 || now < player.CanMoveAgainTime)
            {

                Log.Debug($"Player time to move {player.CanMoveAgainTime - now}");
                // send player back to the position client-side
                player.Tcp.Send(new SyncPacket()
                {
                    Position = player.Position
                });
                return;
            }

            // subtract the player latency for possibility of lag for a smoother movement
            player.CanMoveAgainTime = now + timeToMove - player.Tcp.Latency;

            var entityMoveEvent = new EntityMoveEvent()
            {
                From = packet.From,
                To = packet.To,
                Entity = player
            };
            Server.Events.Call(entityMoveEvent);

            if (entityMoveEvent.IsCancelled)
            {
                // send player back to the position client-side
                player.Tcp.Send(new SyncPacket()
                {
                    Position = player.Position
                });
                return;
            }
  
            // Updating player position locally
            player.Position.X = packet.To.X;
            player.Position.Y = packet.To.Y;

            // updating in database
            PlayerService.UpdatePlayerPosition(player.UID, player.Position.X, player.Position.Y);
        }
    }
}
