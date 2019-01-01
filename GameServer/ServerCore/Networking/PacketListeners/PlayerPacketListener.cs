using Common.Networking.Packets;
using CommonCode.EventBus;
using CommonCode.EntityShared;
using MapHandler;
using ServerCore.GameServer.Players.Evs;
using Storage.Players;
using Common.Entity;
using Common.Scheduler;

namespace ServerCore.Networking.PacketListeners
{
    public class PlayerPacketListener : IEventListener
    {
        [EventMethod]
        public void OnEntityTarget(EntityTargetPacket packet)
        {
            var player = Server.GetPlayer(packet.WhoUuid);

            if(packet.TargetUuid == null)
            {
                player.Target.BeingTargetedBy.Remove(player);
                player.Target = null;
                Log.Info("CANCEL TARGET");
                GameScheduler.CancelTask(player.AttackTaskId);
                return;
            }

            var targetEntityType = packet.TargetUuid.GetEntityTypeFromUid();
            if(targetEntityType == EntityType.MONSTER)
            {
                var monster = Server.Map.Monsters[packet.TargetUuid];
                Server.Events.Call(new EntityTargetEvent()
                {
                    Entity = player,
                    TargetedEntity = monster
                });
            }
        }

        [EventMethod] 
        // server recieving entitymove means its a player moving
        public void OnEntityMovePacket(EntityMovePacket packet)
        {
            var player = Server.GetPlayerByConnectionId(packet.ClientId);
            var originalPosition = new Position(player.Position.X, player.Position.Y);
            var distanceMoved = PositionExtensions.GetDistance(player.Position, packet.To);
            var timeToMove = Formulas.GetTimeToMoveBetweenTwoTiles(player.MoveSpeed);

            var now = GameThread.TIME_MS_NOW;

            // Player tryng to hack ?
            var isPassable = Server.Map.IsPassable(packet.To.X, packet.To.Y);
            if (distanceMoved > 1 || now < player.CanMoveAgainTime || !isPassable)
            {
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
                To = packet.To,
                Entity = player
            };
            Server.Events.Call(entityMoveEvent);

            // updating in database
            PlayerService.UpdatePlayerPosition(player.UID, player.Position.X, player.Position.Y);


            // update chunks for that player
            ChunkProvider.CheckChunks(player);
        }
    }
}
