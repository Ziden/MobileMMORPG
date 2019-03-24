using Assets.Code.Game;
using Assets.Code.Game.ClientPlayer;
using Assets.Code.Game.Factories;
using Client.Net;
using Common.Entity;
using Common.Networking.Packets;
using CommonCode.EventBus;
using CommonCode.Networking.Packets;
using MapHandler;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Net.PacketListeners
{
    public class PlayerListener : IEventListener
    {
        [EventMethod]
        // When something goes wrong, we sync (force) the player to a state
        public void OnPlayerSync(SyncPacket packet)
        {
            Position destination = null;
            if (UnityClient.Player.Behaviour.Route.Count > 0)
            {
                destination = UnityClient.Player.Behaviour.Route.Last();
            }
            UnityClient.Player.PlayerObject.GetComponent<PlayerBehaviour>().StopMovement();
            UnityClient.Player.Behaviour.Route.Clear();
            UnityClient.Player.TeleportToTile(packet.Position.X, packet.Position.Y);

            // recalculating route to destination
            if (destination != null)
            {
                var path = UnityClient.Map.FindPath(UnityClient.Player.Position, destination);
                if (path != null)
                {
                    UnityClient.Player.Behaviour.Route = path;
                }
            }
        }

        [EventMethod]
        public void OnPlayerAppears(PlayerPacket packet)
        {
            // instantiate the player if needed
            var player = PlayerFactory.BuildAndInstantiate(new PlayerFactoryOptions()
            {
                HeadSpriteIndex = packet.HeadSpriteIndex,
                BodySpriteIndex = packet.BodySpriteIndex,
                LegsSpriteIndex = packet.LegSpriteIndex,
                ChestSpriteIndex = packet.ChestSpriteIndex,
                UserId = packet.UserId,
                Speed = packet.Speed,
                Position = new Position(packet.X, packet.Y),
                IsMainPlayer = packet.UserId == UnityClient.Player.UID
            });
            player.Entity.Atk = packet.Atk;
            player.Entity.Def = packet.Def;
            player.Entity.HP = packet.HP;
            player.Entity.MAXHP = packet.MAXHP;
            player.Entity.AtkSpeed = packet.AtkSpeed;
            TouchHandler.GameTouchEnabled = true;
        }

        public static void PlayerSetTarget(GameObject target)
        {
            var livingEntityBhv = target.GetComponent<LivingEntityBehaviour>();
            if (livingEntityBhv == null)
            {
                return; 
            }
            Selectors.RemoveSelector(SelectorType.TARGET);
            var targetEntity = livingEntityBhv.Entity;
            var entityType = targetEntity.EntityType;
            if (entityType == EntityType.MONSTER)
            {
                UnityClient.Player.Target = targetEntity;
                UnityClient.TcpClient.Send(new EntityTargetPacket()
                {
                    WhoUuid = UnityClient.Player.UID,
                    TargetUuid = UnityClient.Player.Target.UID
                });
                Selectors.AddSelector(target, SelectorType.TARGET, Color.red);
                var path = UnityClient.Map.FindPath(UnityClient.Player.Position, target.GetMapPosition());
                if (path != null)
                {
                    UnityClient.Player.Behaviour.Route = path;
                    Selectors.HideSelector();
                }
            } 
        }
    }
}
