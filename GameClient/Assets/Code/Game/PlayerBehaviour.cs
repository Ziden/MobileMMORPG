using Assets.Code.AssetHandling;
using Assets.Code.Game;
using Client.Net;
using CommonCode.Player;
using MapHandler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Position _goingTo;

    private Direction _movingTo = Direction.NONE;

    private SpriteSheet spriteSheet;

    private float t;
    private Vector3 startPosition;
    private Vector3 target;
    private float timeToReachTarget;
    private bool _lastMovement;

    void Start()
    {
        startPosition = target = transform.position;
        spriteSheet = GetComponent<SpriteSheet>();
    }

    void Update()
    {
        _checkGoingTo();
        _prepareMovement();
        _move();
    }

    private void _move()
    {
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, t);

        if(transform.position == target && _movingTo != Direction.NONE)
        {
            _movingTo = Direction.NONE;
            spriteSheet.Moving = false;
            if(_lastMovement)
            {
                _lastMovement = false;
                Selectors.HideSelector();
            }
        }
    }

    private void _prepareMovement()
    {
        var player = UnityClient.Player;
        if (_goingTo != null && _movingTo == Direction.NONE)
        {
            _movingTo = player.Position.GetDirection(_goingTo);
            var timeToMove = (float)Formulas.GetTimeToMoveBetweenTwoTiles(player.Speed);
            SetDestination(new Vector3(_goingTo.X * 16, _goingTo.Y * 16, 0), timeToMove / 1000);
            Debug.Log("Moving Player To " + _goingTo.X + " - "+_goingTo.Y);
            spriteSheet.Direction = _movingTo;
            spriteSheet.Moving = true;
            UnityClient.Player.Position.X = _goingTo.X;
            UnityClient.Player.Position.Y = _goingTo.Y;
            _goingTo = null;
        }
    }

    public void SetDestination(Vector3 destination, float time)
    {
        t = 0;
        startPosition = transform.position;
        timeToReachTarget = time;
        target = destination;
    }

    private void _checkGoingTo()
    {
        if (_movingTo != Direction.NONE)
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
            _goingTo = nextStep;
            if(player.FollowingPath.Count == 0)
            {
                _lastMovement = true;
            }
        }
    }
}
