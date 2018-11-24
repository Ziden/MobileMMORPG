using Assets.Code.AssetHandling;
using Assets.Code.Game;
using Client.Net;
using Common.Networking.Packets;
using CommonCode.EntityShared;
using MapHandler;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Position _nextStep;

    private Direction _movingToDirection = Direction.NONE;

    public List<SpriteSheet> SpriteSheets = new List<SpriteSheet>();

    private float t;
    private Vector3 startPosition;
    private Vector3? _target;
    private float timeToReachTarget;
    private bool _lastMovement;

    void Start()
    {
        _target = startPosition = transform.position;
        SpriteSheets = new SpriteSheet[] {
             transform.Find("body").GetComponent<SpriteSheet>(),
             transform.Find("chest").GetComponent<SpriteSheet>(),
             transform.Find("head").GetComponent<SpriteSheet>(),
             transform.Find("legs").GetComponent<SpriteSheet>()
        }.ToList();
    }

    void Update()
    {
        ReadPathfindingNextMovement();
        SetRoute();
        MoveTick();
    }

    public void StopMovement()
    {
        _movingToDirection = Direction.NONE;
        SpriteSheets.ForEach(e => e.Moving = false);
        _nextStep = null;
        _target = null;
    }

    private void MoveTick()
    {
        if (!_target.HasValue)
            return;
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, new Vector2(_target.Value.x, _target.Value.y), t);

        if (transform.position == _target && _movingToDirection != Direction.NONE)
        {
            _movingToDirection = Direction.NONE;

            SpriteSheets.ForEach(e => e.Moving = false);

            if (_lastMovement)
            {
                _lastMovement = false;
                if(UnityClient.Player.FollowingPath == null)
                    Selectors.HideSelector();
                _target = null;
            }
        }
    }

    private void SetRoute()
    {
        var player = UnityClient.Player;
        if (_nextStep != null && _movingToDirection == Direction.NONE)
        {
            _movingToDirection = MapHelpers.GetDirection(player.Position, _nextStep);
            var timeToMove = (float)Formulas.GetTimeToMoveBetweenTwoTiles(player.Speed);

            UnityClient.TcpClient.Send(new EntityMovePacket()
            {
                UID = UnityClient.Player.UserId,
                From = UnityClient.Player.Position,
                To = _nextStep
            });

            SetDestination(new Vector3(_nextStep.X * 16, _nextStep.Y * 16, 0), timeToMove / 1000);
            Debug.Log("Moving Player To " + _nextStep.X + " - " + _nextStep.Y);

            SpriteSheets.ForEach(e => e.Direction = _movingToDirection);
            SpriteSheets.ForEach(e => e.Moving = true);

            UnityClient.Player.Position.X = _nextStep.X;
            // minus cause <reason>
            UnityClient.Player.Position.Y = _nextStep.Y;
            _nextStep = null;
        }
    }

    public void SetDestination(Vector3 destination, float time)
    {
        t = 0;
        startPosition = transform.position;
        timeToReachTarget = time;
        _target = new Vector2(destination.x, -destination.y);
    }

    private void ReadPathfindingNextMovement()
    {
        if (_movingToDirection != Direction.NONE)
            return;
        var player = UnityClient.Player;
        if (player.FollowingPath != null && player.FollowingPath.Count > 0)
        {
            var nextStep = player.FollowingPath[0];

            if (player.Position.X == nextStep.X && player.Position.Y == nextStep.Y)
            {
                player.FollowingPath.RemoveAt(0);
                nextStep = player.FollowingPath[0];
            }

            player.FollowingPath.RemoveAt(0);
            _nextStep = nextStep;
            if (player.FollowingPath.Count == 0)
            {
                player.FollowingPath = null;
                _lastMovement = true;
            }
        }
    }
}
