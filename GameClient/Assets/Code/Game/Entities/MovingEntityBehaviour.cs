using Assets.Code.AssetHandling;
using Assets.Code.AssetHandling.Sprites.Animations;
using Assets.Code.Game;
using Assets.Code.Game.Entities;
using Client.Net;
using Common.Entity;
using CommonCode.EntityShared;
using MapHandler;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntityBehaviour : MonoBehaviour
{
    public Vector2 PositionOffset = new Vector2(0, -8);

    // Add a position to the route to make the entity move, or add a route. :)
    public List<Position> Route = new List<Position>();
    public List<SpriteSheet> SpriteSheets = new List<SpriteSheet>();
    public Entity Entity;

    private Position _goingToPosition;
    private Direction _movingToDirection = Direction.NONE;
    private float _timeForLerp;
    private Vector3 _startPosition;
    private Vector3? _target;
    private float _timeToReachTarget;
    private bool _lastMovement;
    private long _shouldArriveAt;

    void Start()
    {
        _target = _startPosition = transform.position;
    }

    public void ForceUpdate()
    {
        Update();
    }

    void Update()
    {
        OnBeforeUpdate();
        ReadPathfindingNextMovement();
        PerformMovement();
        MoveTick();
    }

    public void StopMovement()
    {
        _movingToDirection = Direction.NONE;
        SpriteSheets.ForEach(e => e.SetAnimation(SpriteAnimations.NONE));
        _goingToPosition = null;
        _target = null;
    }

    private void MoveTick()
    {
        if (!_target.HasValue)
            return;

        var now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        if (transform.position == _target && _movingToDirection != Direction.NONE && now >= _shouldArriveAt)
        {
            _movingToDirection = Direction.NONE;
            if (_lastMovement && Route.Count == 0)
            {
                SpriteSheets.ForEach(e => e.SetAnimation(SpriteAnimations.NONE));
                _lastMovement = false;
                _target = null;
                OnFinishRoute();
            }
        }
        else
        {
            _timeForLerp += Time.deltaTime;
            transform.position = Vector3.Lerp(_startPosition, new Vector2(_target.Value.x, _target.Value.y), _timeForLerp / _timeToReachTarget);
        }
    }

    private void PerformMovement()
    {
        if (_goingToPosition != null && _movingToDirection == Direction.NONE)
        {
            _movingToDirection = Entity.Position.GetDirection(_goingToPosition);
            var timeToMoveInMillis = Formulas.GetTimeToMoveBetweenTwoTiles(Entity.MoveSpeed);

            var timeToMoveInSeconds = (float)timeToMoveInMillis / 1000;

            var now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            OnBeforeMoveTile(_goingToPosition);

            SetDestination(_goingToPosition.ToUnityPosition(), timeToMoveInSeconds);

            SpriteSheets.ForEach(e => e.SetAnimation(SpriteAnimations.MOVING));
            SpriteSheets.ForEach(e => e.SetDirection(_movingToDirection));         

            UnityClient.Map.UpdateEntityPosition(Entity, Entity.Position, _goingToPosition);

            Entity.Position.X = _goingToPosition.X;
            Entity.Position.Y = _goingToPosition.Y;

            _goingToPosition = null;

            _shouldArriveAt = now + timeToMoveInMillis;
        }
    }

    public void SetDestination(Vector2 destination, float time)
    {
        _timeForLerp = 0;
        _startPosition = transform.position;
        _timeToReachTarget = time;
        _target = destination;
    }

    public void ReadPathfindingNextMovement()
    {
        if (_movingToDirection != Direction.NONE)
            return;
        if (Route.Count > 0)
        {
            var nextStep = Route[0];

            if(nextStep == null)
            {
                return;
            }

            // tryng to move where i am
            if (Entity.Position.X == nextStep.X && Entity.Position.Y == nextStep.Y)
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

    // OVERRIDABLES
    public virtual void OnFinishRoute(){}

    public virtual void OnBeforeMoveTile(Position movingTo) { }

    public virtual void OnBeforeUpdate() { }
}
