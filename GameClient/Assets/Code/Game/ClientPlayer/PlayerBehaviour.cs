using Assets.Code.Game;
using Client.Net;
using Common.Networking.Packets;
using MapHandler;
using System.Collections.Generic;
using System.Linq;

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

        var playerPos = UnityClient.Player.Position;

        var currentChunk = UnityClient.Map.GetChunkByTilePosition(playerPos.X, playerPos.Y);
        var newChunk = UnityClient.Map.GetChunkByTilePosition(movingTo.X, movingTo.Y);

        if(currentChunk.x != newChunk.x || currentChunk.y != newChunk.y)
        {
            var chunkX = UnityClient.Player.Position.X >> 4;
            var chunkY = UnityClient.Player.Position.Y >> 4;

            var chunkActivedParents = PositionExtensions.GetSquared3x3Around(new Position(chunkX, chunkY)).ToList();
            var chunkActivedParentsNames = new List<string>();
            chunkActivedParents.ForEach(c => chunkActivedParentsNames.Add($"chunk_{c.X}_{c.Y}"));

            foreach (var cp in UnityClient.Map.Chunks.Values)
                if (chunkActivedParentsNames.Contains(cp.GameObject.name))
                    cp.GameObject.SetActive(true);
                else
                    cp.GameObject.SetActive(false);
        }
    }
}
