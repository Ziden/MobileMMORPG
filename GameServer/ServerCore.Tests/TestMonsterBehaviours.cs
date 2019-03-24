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
using ServerCore.Game.Monsters.Behaviours.AggroBehaviours;
using ServerCore.Game.Monsters.Behaviours;
using ServerCore.GameServer.Players.Evs;

namespace GameTests
{
    [TestFixture]
    public class TestMonstersBehaviours
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
            _player = new StoredPlayer()
            {
                UserId = "wololo",
                Login = "login",
                Password = "password",
                MoveSpeed = 1,
                X = 0,
                Y = 0
            };
        }

        [TearDown]
        public void TearDown()
        {
            _server.Stop();
        }

        /// <summary>
        /// Will test when the client targets an TargetBack mob
        /// The target will be set back
        /// </summary>
        [Test]
        public void TestTargetBack()
        {
            var client = ServerMocker.GetClient();
            var player = client.FullLoginSequence(_player);

            var skeleton = new Skeleton()
            {
                Position = new Position(0, 1)
            };
            skeleton.AggroBehaviuor = BehaviourPool.Get<TargetBack>();

            Server.Events.Call(new EntitySpawnEvent()
            {
                Entity = skeleton,
                Position = skeleton.Position
            });

            var chunk = player.GetChunk();

            client.SendToServer(new EntityTargetPacket()
            {
                TargetUuid = skeleton.UID,
                WhoUuid = player.UID
            });

            Assert.That(skeleton.Target == player);
        }
    }
}

