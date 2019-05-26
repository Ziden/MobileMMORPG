using Assets.Code.AssetHandling;
using Assets.Code.AssetHandling.Sprites.Animations;
using Assets.Code.Game;
using Assets.Code.Game.Entities;
using Assets.Code.Game.Factories;
using Client.Net;
using Common.Entity;
using CommonCode.EntityShared;
using MapHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LivingEntityBehaviour : MonoBehaviour
{
    public Vector2 PositionOffset = new Vector2(0, -8);

    // Add a position to the route to make the entity move, or add a route. :)
    public List<Position> Route = new List<Position>();
    public List<SpriteSheet> SpriteSheets = new List<SpriteSheet>();
    public LivingEntity Entity;
    public HealthBarBehaviour HealthBar;

    public bool Dead = false;
    public bool WilLDie = false;
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
        OnStart();
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

    public void PerformAttackAnimation(LivingEntityBehaviour target, int damage)
    {
        var atkSpeedDelay = Formulas.GetTimeBetweenAttacks(Entity.AtkSpeed);
        SpriteSheets.ForEach(e => e.SetDirection(Entity.Position.GetDirection(target.Entity.Position)));

        SpriteSheets.ForEach(e =>
        {
            e.SetAnimation(SpriteAnimations.ATTACKING, atkSpeedDelay);
        });

        // Make a animation callback to display the hit in a cool moment
        SpriteSheets[0].SetAnimationFrameCallback(AttackAnimation.HIT_FRAME, () =>
        {
            Debug.Log("TARGET: " + target.name);
            Debug.Log("ATKER:" + this.name);

            AnimationFactory.BuildAndInstantiate(new AnimationOpts()
            {
                AnimationImageName = DefaultAssets.ANM_BLOOD,
                MapPosition = target.Entity.Position
            });

            var unityPosition = target.Entity.Position.ToUnityPosition();
            TextFactory.BuildAndInstantiate<DamageText>(new TextOptions()
            {
                UnityX = unityPosition.x + 8,
                UnityY = unityPosition.y + 18,
                Text = damage.ToString(),
                TextColor = Color.red,
                Size = 7
            });

            target.Entity.HP -= damage;
            if (target.Entity.HP <= 0 || WilLDie)
            {
                target.Die();
            }
            else
            {
                target.HealthBar?.SetLife(target.Entity.HP, target.Entity.MAXHP);
            }
        });
    }

    public void Die()
    {
        // PLAYER DIES
        if (UnityClient.Player.UID == this.Entity.UID)
        {
            // TODO
        }
        else // other entity dies
        {
            Dead = true;
            UnityClient.Map.UpdateEntityPosition(Entity, Entity.Position, null);
            Destroy(this.GetComponent<BoxCollider2D>()); // so its not clickable
            Destroy(HealthBar.gameObject);
            SpriteSheets.ForEach(spriteSheet =>
            {
                spriteSheet.SetAnimation(SpriteAnimations.DEAD);
            });
            if (UnityClient.Player.Target?.UID == this.Entity.UID)
            {
                UnityClient.Player.Behaviour.StopMovement();
                Selectors.RemoveSelector(SelectorType.TARGET);
            }
        }
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
            // Something happened and its in between me and my goal
            if (!UnityClient.Map.IsPassable(_goingToPosition.X, _goingToPosition.Y))
            {
                // If i still had a decent route, i might try to find another way
                if (Route.Count > 0)
                {
                    var destination = UnityClient.Player.Behaviour.Route.Last();
                    var path = UnityClient.Map.FindPath(Entity.Position, destination);
                    if (path != null)
                    {
                        Route = path;
                    }
                }
                else
                {
                    // we only stop our animation if we are in the moving animation
                    var firstSpriteSheet = SpriteSheets[0];
                    if (firstSpriteSheet.IsAnimationPlayng(SpriteAnimations.MOVING))
                    {
                        SpriteSheets.ForEach(s => s.SetAnimation(SpriteAnimations.NONE));
                    }
                }
                return;
            }

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

            if (nextStep == null)
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
    public virtual void OnFinishRoute() { }

    public virtual void OnBeforeMoveTile(Position movingTo) { }

    public virtual void OnBeforeUpdate() { }

    public virtual void OnStart() { }
}
