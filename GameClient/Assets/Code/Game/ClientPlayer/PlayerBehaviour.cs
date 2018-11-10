using Assets.Code.AssetHandling;
using Assets.Code.Game;
using Client.Net;
using Common.Networking.Packets;
using CommonCode.Player;
using MapHandler;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Position _goingToPosition;

    private Direction _movingToDirection = Direction.NONE;

    private SpriteSheet _bodySpriteSheet;
    private SpriteSheet _chestSpriteSheet;
    private SpriteSheet _headSpriteSheet;
    private SpriteSheet _legsSpriteSheet;

    private List<SpriteSheet> _spriteSheets = new List<SpriteSheet>();

    private float t;
    private Vector3 startPosition;
    private Vector3? _target;
    private float timeToReachTarget;
    private bool _lastMovement;

    void Start()
    {
        _target = startPosition = transform.position;
        _spriteSheets = new SpriteSheet[] {
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
        _spriteSheets.ForEach(e => e.Moving = false);
        _goingToPosition = null;
        _target = null;
    }

    private void MoveTick()
    {
        if (!_target.HasValue)
            return;
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, _target.Value, t);

        if (transform.position == _target && _movingToDirection != Direction.NONE)
        {
            _movingToDirection = Direction.NONE;

            _spriteSheets.ForEach(e => e.Moving = false);

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
        if (_goingToPosition != null && _movingToDirection == Direction.NONE)
        {
            _movingToDirection = player.Position.GetDirection(_goingToPosition);
            var timeToMove = (float)Formulas.GetTimeToMoveBetweenTwoTiles(player.Speed);

            UnityClient.TcpClient.Send(new PlayerMovePacket()
            {
                From = UnityClient.Player.Position,
                To = _goingToPosition
            });

            SetDestination(new Vector3(_goingToPosition.X * 16, _goingToPosition.Y * 16, 0), timeToMove / 1000);
            Debug.Log("Moving Player To " + _goingToPosition.X + " - " + _goingToPosition.Y);

            _spriteSheets.ForEach(e => e.Direction = _movingToDirection);
            _spriteSheets.ForEach(e => e.Moving = true);

            UnityClient.Player.Position.X = _goingToPosition.X;
            UnityClient.Player.Position.Y = _goingToPosition.Y;
            _goingToPosition = null;
        }
    }

    public void SetDestination(Vector3 destination, float time)
    {
        t = 0;
        startPosition = transform.position;
        timeToReachTarget = time;
        _target = destination;
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
            _goingToPosition = nextStep;
            if (player.FollowingPath.Count == 0)
            {
                player.FollowingPath = null;
                _lastMovement = true;
            }
        }
    }
}
