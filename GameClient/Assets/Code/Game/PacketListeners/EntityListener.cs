using Assets.Code.Game.ClientPlayer;
using Client.Net;
using Common.Networking.Packets;
using CommonCode.EventBus;
using UnityEngine;

namespace Assets.Code.Game.PacketListeners
{
    public class EntityListener : IEventListener
    {
        [EventMethod]
        public void OnEntityMove(EntityMovePacket packet)
        {
            var entityObj = GameObject.Find(packet.UID);
            if (entityObj != null)
            {
                var movingEntity = entityObj.GetComponent<MovingEntityBehaviour>();
                if (movingEntity != null)
                {
                    movingEntity.Route.Add(packet.To);
                    movingEntity.ForceUpdate(); // to make sure its moved rightn away

                    if (UnityClient.Player.Target != null && UnityClient.Player.Target == movingEntity.Entity)
                    {
                        UnityClient.Player.FindPathTo(movingEntity.Entity.Position);
                    }
                }
            }
        }
    }
}
