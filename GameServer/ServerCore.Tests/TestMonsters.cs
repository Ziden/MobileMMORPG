using NUnit.Framework;
using MapHandler;
using Storage;
using Storage.TestDataBuilder;
using ServerCore;
using ServerCore.Tests.TestUtilities;
using Common.Networking.Packets;
using Storage.Players;
using ServerCore.GameServer.Players;
using ServerCore.Game.Monsters;
using System.Linq;

namespace MapTests
{
    [TestFixture]
    public class TestMonsters
    {
        private Player _player;
        private Server _server = new Server(123, "test");

        [SetUp]
        public void Start()
        {
            Redis redis = new Redis();
            redis.Start();
            TestDb.Create();
            _server.StartGameThread();
            _player = new Player()
            {
                UserId = "wololo",
                Login = "login",
                Password = "password",
                MoveSpeed = 1,
                X = 0,
                Y = 0
            };
        }

        [Test]
        public void TestPlayerMoving()
        {
            var client = ServerMocker.GetClient();
            var player = client.FullLoginSequence(_player);

            var skeleton = new Skeleton()
            {
                Position = new Position(0, 1)
            };
            var chunk = player.GetChunk();
            chunk.MonstersInChunk.Add(skeleton);

            client.RecievedPackets.Clear();

            skeleton.MovementTick();
            skeleton.MovementTick();
            skeleton.MovementTick();

            var monsterMovePackets = client.RecievedPackets
                .Where(p => p.GetType() == typeof(EntityMovePacket))
                .Select(p => (EntityMovePacket)p)
                .Where(p => p.UID == skeleton.UID)
                .ToList();

            Assert.That(monsterMovePackets.Count > 0,
                "Player did not recieve updates from monster moving");
        }

        [Test]
        public void TestPlayerTooFarAwayToRecieveUpdates()
        {
            var client = ServerMocker.GetClient();
            var player = client.FullLoginSequence(_player);

            var skeleton = new Skeleton()
            {
                Position = new Position(33, 0)
            };

            var monsterChunk = Server.Map.GetChunk(2,0);
            var playerChunki = Server.Map.GetChunk(0, 0);

            var chunks = Server.Map.Chunks;

            monsterChunk.MonstersInChunk.Add(skeleton);

            client.RecievedPackets.Clear();

            skeleton.MovementTick();
            skeleton.MovementTick();
            skeleton.MovementTick();

            var monsterMovePackets = client.RecievedPackets
                .Where(p => p.GetType() == typeof(EntityMovePacket))
                .Select(p => (EntityMovePacket)p)
                .Where(p => p.UID == skeleton.UID)
                .ToList();

            Assert.That(monsterMovePackets.Count == 0,
                "Player Recieved Updates from a too far away monster");
        }

        [Test]
        public void TestPlayerMovingOnDifferentChunk()
        {
            var client = ServerMocker.GetClient();
            var player = client.FullLoginSequence(_player);

            var skeleton = new Skeleton()
            {
                Position = new Position(0, 22)
            };

            var monsterChunk = Server.Map.GetChunk(0, 1);
            var playerChunki = Server.Map.GetChunk(0, 0);

            monsterChunk.MonstersInChunk.Add(skeleton);

            client.RecievedPackets.Clear();

            skeleton.MovementTick();
            skeleton.MovementTick();
            skeleton.MovementTick();

            var monsterMovePackets = client.RecievedPackets
                .Where(p => p.GetType() == typeof(EntityMovePacket))
                .Select(p => (EntityMovePacket)p)
                .Where(p => p.UID == skeleton.UID)
                .ToList();

            Assert.That(monsterMovePackets.Count > 0,
                "Player did not recieve updates from monster moving");
        }

    }
}

