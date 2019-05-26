using NUnit.Framework;
using MapHandler;
using Storage;
using Storage.TestDataBuilder;
using ServerCore;
using ServerCore.Tests.TestUtilities;
using Common.Networking.Packets;
using Storage.Players;
using ServerCore.Game.Monsters;
using System.Linq;
using ServerCore.GameServer.Entities;
using static ServerCore.Server;
using Common.Entity;
using System.Collections.Generic;
using ServerCore.GameServer.Players.Evs;
using CommonCode.Networking.Packets;
using System.Threading;
using ServerCore.Game.Combat;
using Common.Scheduler;

namespace GameTests
{
    [TestFixture]
    public class TestPlayer
    {
        private StoredPlayer _player;
        private Server _server;

        [SetUp]
        public void Start()
        {
            Redis redis = new Redis();
            redis.Start();
            TestDb.Create();
            _server = new Server(new ServerStartConfig() { Port = 123 });
            _server.StartGameThread();
            _player = TestDb.TEST_PLAYER;
            _player.Login = "wololo";
        }

        [TearDown]
        public void TearDown()
        {
            _server.Stop();
        }

        [Test]
        public void TestPlayerMovingOnVariousDiferentChunk()
        {
            var client = ServerMocker.GetClient();
            var player = client.FullLoginSequence(_player);

            var chunkPackets = new List<BasePacket>();

            client.RecievedPackets.Clear();

            player.MoveSpeed = int.MaxValue;

            for (int i = 1; i <= 30; i++)
            {
                var newPosition = new Position(player.Position.X + 1, player.Position.Y);

                client.SendToServer(new EntityMovePacket
                {
                    ClientId = client.ConnectionId,
                    UID = player.UID,
                    To = newPosition
                });
                
                chunkPackets = client.RecievedPackets
                    .Where(p => p.GetType() == typeof(ChunkPacket))
                    .ToList();
            }

            Assert.That(chunkPackets.Count == 3,
                "Player did not recieve updates from chunk map");
        }
    }
}

