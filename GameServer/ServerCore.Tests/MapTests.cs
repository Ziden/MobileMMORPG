using NUnit.Framework;
using Storage.Players;
using ServerCore;
using Storage;
using Storage.TestDataBuilder;
using ServerCore.Tests.TestUtilities;
using ServerCore.Game.Monsters;
using MapHandler;
using ServerCore.GameServer.Players.Evs;

namespace MapTests
{
    [TestFixture]
    public class MapTests
    {
        private StoredPlayer _player;
        private Server _server = new Server(123, mapName: "test");

        [SetUp]
        public void Start()
        {
            Redis redis = new Redis();
            redis.Start();

            TestDb.Create();
            _player = new StoredPlayer()
            {
                UserId = "wololo",
                Login = "login",
                Password = "password",
                MoveSpeed = 1,
                X = 5,
                Y = 5
            };
        }

        [Test]
        public void EntityEntityPassabilityCheck()
        {
            Assert.That(Server.Map.IsPassable(_player.X, _player.Y) == true,
               "Tile is returning its not passable, but it should be");

            var client = ServerMocker.GetClient();
            client.FullLoginSequence(_player);

            Assert.That(Server.Map.IsPassable(_player.X, _player.Y) == false,
                "A tile that a player is in is returning passable");

            var skeleton = new Skeleton();
            skeleton.Position = new Position(0, 3);

            Assert.That(Server.Map.IsPassable(skeleton.Position.X, skeleton.Position.Y) == true,
                "Tile should be flagged as passable as the skeleton is not there yet");

            Server.Events.Call(new EntitySpawnEvent()
            {
                Entity = skeleton,
                Position = skeleton.Position
            });

            Assert.That(Server.Map.IsPassable(skeleton.Position.X, skeleton.Position.Y) == false,
                "Tile should not be passable as there is a skeleton there now");

            var newPosition = new Position(skeleton.Position.X+1, skeleton.Position.Y);

            Assert.That(Server.Map.IsPassable(newPosition.X, newPosition.Y) == true,
              "Skeleton didnt move yet so it should be passable");

            Server.Events.Call(new EntityMoveEvent()
            {
                Entity = skeleton,
                From = skeleton.Position,
                To = newPosition
            });

            Assert.That(Server.Map.IsPassable(newPosition.X, newPosition.Y) == false,
                "After skeleton moved his tile should be impassable");

            Assert.That(Server.Map.IsPassable(newPosition.X-1, newPosition.Y) == true,
                "Skeleton last position was not freed after he moved");

        }
    }
}

