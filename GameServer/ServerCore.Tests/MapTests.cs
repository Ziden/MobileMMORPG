using NUnit.Framework;
using Storage.Players;
using ServerCore;
using Storage;
using Storage.TestDataBuilder;
using ServerCore.Tests.TestUtilities;
using ServerCore.Game.Monsters;
using MapHandler;
using ServerCore.GameServer.Players.Evs;
using System.Linq;
using Common.Networking.Packets;
using static ServerCore.Server;
using ServerCore.Game.Monsters.Behaviours;
using ServerCore.Game.Monsters.Behaviours.MoveBehaviours;

namespace MapTests
{
    [TestFixture]
    public class MapTests
    {
        private StoredPlayer _player;
        private Server _server;

        [SetUp]
        public void Start()
        {
            Redis redis = new Redis();
            redis.Start();

            _server = new Server(new ServerStartConfig() { Port = 123 }); 

            TestDb.Create();
            _player = new StoredPlayer()
            {
                UserId = "wololo",
                Login = "login",
                Password = "password",
                MoveSpeed = int.MaxValue,
                X = 0,
                Y = 0
            };
        }

        [OneTimeTearDown]
        public void Stop()
        {
            _server.Stop();
        }

        [Test]
        public void TestMonsterMovingCollision()
        {
            var client = ServerMocker.GetClient();
            client.FullLoginSequence(_player);

            var skeleton = new Skeleton();
            skeleton.Position = new Position(1, 1);
            skeleton.MovementBehaviour = BehaviourPool.Get<LeftRightWalk>();

            Server.Map.GetTile(2, 1).TileId = 2; // block

            var originalX = skeleton.Position.X;
            Server.Events.Call(new EntitySpawnEvent()
            {
                Entity = skeleton,
                Position = skeleton.Position
            });

            client.SendToServer(new EntityMovePacket()
            {
                To = new Position(_player.X, _player.Y + 1),
                UID = _player.UserId,
            });

            client.RecievedPackets.Clear();

            skeleton.MovementTick();

            Assert.That(skeleton.Position.X == originalX,
                "Skeleton couldnt have moved as he was in between 2 obstacles");

            Assert.That(client.RecievedPackets.Count == 0,
                "Player should not recieve monster move packets if the monster didnt moved");
        }

        [Test]
        public void TestMovingPlayerCollision()
        {
            var client = ServerMocker.GetClient();
            client.FullLoginSequence(_player);

            var tilePlayerIn = Server.Map.GetTile(_player.X, _player.Y);
            Assert.That(tilePlayerIn.Occupator.UID == _player.UserId, 
                "Player Did not occupy his tile when spawning");

            client.SendToServer(new EntityMovePacket()
            {
                To = new Position(_player.X - 1, _player.Y),
                UID = _player.UserId,
            });

            var playerWasin = Server.Map.GetTile(_player.X, _player.Y);
            var playerGoingTo = Server.Map.GetTile(_player.X - 1, _player.Y);

            Assert.That(Server.Map.IsPassable(_player.X - 1, _player.Y) == false,
                "Player new occupation was not properly set after moving");
            Assert.That(Server.Map.IsPassable(_player.X, _player.Y) == true,
                "Player new occupation was not properly set after moving");
            Assert.That(playerWasin.Occupator == null,
                "Player new occupation was not properly set after moving");
            Assert.That(playerGoingTo.Occupator.UID == _player.UserId,
                "Player new occupation was not properly set after moving");

            client.SendToServer(new EntityMovePacket()
            {
                To = new Position(_player.X, _player.Y),
                UID = _player.UserId,
            });

            playerWasin = Server.Map.GetTile(_player.X - 1, _player.Y);
            playerGoingTo = Server.Map.GetTile(_player.X, _player.Y);

            Assert.That(Server.Map.IsPassable(_player.X - 1, _player.Y) == true,
                "Player new occupation was not properly set after moving");
            Assert.That(Server.Map.IsPassable(_player.X, _player.Y) == false,
                "Player new occupation was not properly set after moving");
            Assert.That(playerWasin.Occupator == null,
                "Player new occupation was not properly set after moving");
            Assert.That(playerGoingTo.Occupator.UID == _player.UserId,
                "Player new occupation was not properly set after moving");
        }

        [Test]
        public void EntityPathfindingColiisionChecks()
        {
            var client = ServerMocker.GetClient();
            client.FullLoginSequence(_player);

            var path = Server.Map.FindPath(new Position(0, 0), new Position(-2, 0));

            Assert.That(path.Count == 3);

            var monster = Server.Map.Monsters.Values.First();

            var newPosition = new Position(-1, 0); // in between player and goal

            Server.Events.Call(new EntityMoveEvent()
            {
                Entity = monster,
                To = newPosition
            });

            var pathWithMonsterInBetween = Server.Map.FindPath(new Position(0, 0), new Position(-2, 0));

            Assert.That(pathWithMonsterInBetween.Count == 5, 
                "Player did not move around the monster that spawned between him and his goal");
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
                To = newPosition
            });

            Assert.That(Server.Map.IsPassable(newPosition.X, newPosition.Y) == false,
                "After skeleton moved his tile should be impassable");

            Assert.That(Server.Map.IsPassable(newPosition.X-1, newPosition.Y) == true,
                "Skeleton last position was not freed after he moved");

        }
    }
}

