using Assets.Code.Game;
using Client.Net;
using Common.Networking.Packets;
using MapHandler;

public class PlayerBehaviour : MovingEntityBehaviour
{
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
