using Assets.Code.Game;
using Client.Net;
using Common.Networking.Packets;
using MapHandler;
using System.Diagnostics;

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
            //From = UnityClient.Player.Position, //TODO: Verify this line!
            To = movingTo
        });
    }

    /*
    private Position _nextStep;

    private Direction _movingToDirection = Direction.NONE;

    public List<SpriteSheet> SpriteSheets = new List<SpriteSheet>();

    private float t;
    private Vector3 startPosition;
    private Vector3? _target;
    private float timeToReachTarget;
    private bool _lastMovement;
    private long _shouldArriveAt;

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
        PerformNextStep();
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

        var now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        if (transform.position == _target && _movingToDirection != Direction.NONE && now > _shouldArriveAt)
        {
            _movingToDirection = Direction.NONE;

            SpriteSheets.ForEach(e => e.Moving = false);

            if (_lastMovement)
            {
                _lastMovement = false;
                if (UnityClient.Player.FollowingPath == null)
                    Selectors.HideSelector();
                _target = null;
            }
        } else
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, new Vector2(_target.Value.x, _target.Value.y), t / timeToReachTarget);
        }
    }

    private void PerformNextStep()
    {
        var player = UnityClient.Player;
        if (_nextStep != null && _movingToDirection == Direction.NONE)
        {
            _movingToDirection = player.Position.GetDirection(_nextStep);
            var moveTimeInMs = Formulas.GetTimeToMoveBetweenTwoTiles(player.Speed);

            var moveTimeInSeconds = (float)moveTimeInMs / 1000;

            var now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            _shouldArriveAt = now + moveTimeInMs;

            UnityClient.TcpClient.Send(new EntityMovePacket()
            {
                UID = UnityClient.Player.UID,
                From = UnityClient.Player.Position,
                To = _nextStep
            });

            StartMovement(_nextStep.ToUnityPosition(), moveTimeInSeconds);

            SpriteSheets.ForEach(e => e.Direction = _movingToDirection);
            SpriteSheets.ForEach(e => e.Moving = true);

            try
            {
                UnityClient.Map.UpdateEntityPosition(UnityClient.Player, UnityClient.Player.Position, _nextStep);
            }
            catch (Exception e)
            {
                var asd = 123;
                asd += 5;
            }

            UnityClient.Player.Position.X = _nextStep.X;
            UnityClient.Player.Position.Y = _nextStep.Y;

            _nextStep = null;
        }
    }

    public void StartMovement(Vector2 destination, float time)
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
            _nextStep = nextStep;
            if (player.FollowingPath.Count == 0)
            {
                player.FollowingPath = null;
                _lastMovement = true;
            }
        }
    }
    */
}
