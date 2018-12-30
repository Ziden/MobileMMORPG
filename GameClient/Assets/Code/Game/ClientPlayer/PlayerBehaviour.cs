using Assets.Code.Game;
using Client.Net;
using Common.Networking.Packets;
using CommonCode.EntityShared;
using CommonCode.Networking.Packets;
using MapHandler;
using System;
using UnityEngine;

public class PlayerBehaviour : MovingEntityBehaviour
{
    public static long NOW_MILLIS = 0;

    public override void OnBeforeUpdate()
    {
        NOW_MILLIS = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        PerformAttack();
    }

    private void PerformAttack()
    {
        if (UnityClient.Player.Target != null)
        {
            if (UnityClient.Player.NextAttackAt <= NOW_MILLIS)
            {
                var distanceToTarget = UnityClient.Player.Position.GetDistance(UnityClient.Player.Target.Position);
                if (distanceToTarget <= 1)
                {
                    UnityClient.TcpClient.Send(new EntityAttackPacket()
                    {
                        EntityUID = UnityClient.Player.Target.UID
                    });
                    var atkSpeedDelay = Formulas.GetTimeBetweenAttacks(UnityClient.Player.AtkSpeed);
                    var nextAttack = NOW_MILLIS + atkSpeedDelay;
                    UnityClient.Player.NextAttackAt = nextAttack;

                   // UnityClient.Player.Movement.SpriteSheets.ForEach(e => e.PerformAttackAnimation());
                }
            }
        }
    }

    public override void OnFinishRoute()
    {
        // Hide the green square on the ground when i finish moving
        if (this.Route.Count == 0)
            Selectors.HideSelector();
    }

    public override void OnBeforeMoveTile(Position movingTo)
    {
        // Inform the server i moved
        UnityClient.TcpClient.Send(new EntityMovePacket()
        {
            UID = UnityClient.Player.UID,
            To = movingTo
        });
    }
}
