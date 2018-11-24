using Assets.Code.AssetHandling;
using CommonCode.EntityShared;
using MapHandler;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntityBehaviour : MonoBehaviour
{
    private Position _goingToPosition;

    private Direction _movingToDirection = Direction.NONE;

    public Position MapPosition = new Position(0, 0);
    public int Speed;
    public List<Position> Route = new List<Position>();
    public List<SpriteSheet> SpriteSheets = new List<SpriteSheet>();

    private float t;
    private Vector3 startPosition;
    private Vector3? _target;
    private float timeToReachTarget;
    private bool _lastMovement;

    void Start()
    {
        _target = startPosition = transform.position;
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

            SpriteSheets.ForEach(e => e.Moving = false);

            if (_lastMovement)
            {
                _lastMovement = false;
                _target = null;
            }
        }
    }

    private void SetRoute()
    {
        if (_goingToPosition != null && _movingToDirection == Direction.NONE)
        {
            _movingToDirection = MapHelpers.GetDirection(MapPosition, _goingToPosition);
            var timeToMove = (float)Formulas.GetTimeToMoveBetweenTwoTiles(Speed);

            SetDestination(new Vector3(_goingToPosition.X * 16, _goingToPosition.Y * 16, 0), timeToMove / 1000);
            Debug.Log("Moving Entity To " + _goingToPosition.X + " - " + _goingToPosition.Y);

            SpriteSheets.ForEach(e => e.Direction = _movingToDirection);
            SpriteSheets.ForEach(e => e.Moving = true);

            MapPosition.X = _goingToPosition.X;
            MapPosition.Y = _goingToPosition.Y;
            _goingToPosition = null;
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
        if (Route.Count > 0)
        {
            var nextStep = Route[0];

            // tryng to move where i am
            if (MapPosition.X == nextStep.X && MapPosition.Y == nextStep.Y)
            {
                Route.RemoveAt(0);
                ReadPathfindingNextMovement();
                return;
            }

            Route.RemoveAt(0);
            _goingToPosition = nextStep;
            if (Route.Count == 0)
            {
                _lastMovement = true;
            }
        }
    }
}
