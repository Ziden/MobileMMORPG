using Assets.Code.AssetHandling;
using Assets.Code.Game;
using Client.Net;
using CommonCode.EntityShared;
using MapHandler;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntityBehaviour : MonoBehaviour
{
    // Add a position to the route to make the entity move, or add a route. :)
    public List<Position> Route = new List<Position>();
    public Position MapPosition = new Position(0, 0);
    public List<SpriteSheet> SpriteSheets = new List<SpriteSheet>();
    public int MoveSpeed;

    public Entity EntityWrapper;

    private Position _goingToPosition;
    private Direction _movingToDirection = Direction.NONE;
    private float _timeForLerp;
    private Vector3 _startPosition;
    private Vector3? _target;
    private float timeToReachTarget;
    private bool _lastMovement;

    void Start()
    {
        _target = _startPosition = transform.position;
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
        _timeForLerp += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(_startPosition, _target.Value, _timeForLerp);

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
            var timeToMove = (float)Formulas.GetTimeToMoveBetweenTwoTiles(MoveSpeed);

            SetDestination(_goingToPosition.ToUnityPosition(), timeToMove / 1000);

            SpriteSheets.ForEach(e => e.Direction = _movingToDirection);
            SpriteSheets.ForEach(e => e.Moving = true);

            UnityClient.Map.UpdateEntityPosition(EntityWrapper, MapPosition, _goingToPosition);

            MapPosition.X = _goingToPosition.X;
            MapPosition.Y = _goingToPosition.Y;

            _goingToPosition = null;
        }
    }

    public void SetDestination(Vector2 destination, float time)
    {
        _timeForLerp = 0;
        _startPosition = transform.position;
        timeToReachTarget = time;
        _target = destination;
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
