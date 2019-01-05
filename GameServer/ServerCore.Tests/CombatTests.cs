using NUnit.Framework;
using MapHandler;
using Storage;
using Storage.TestDataBuilder;
using ServerCore;
using ServerCore.Tests.TestUtilities;
using Common.Networking.Packets;
using Storage.Players;
using ServerCore.Game.Monsters;
using ServerCore.GameServer.Players.Evs;
using System.Linq;
using CommonCode.Networking.Packets;
using CommonCode.EntityShared;
using Common.Scheduler;
using ServerCore;
using System;

namespace CombatTests
{
    [TestFixture]
    public class AttackTests
    {
        private StoredPlayer _player;
        private Server _server;
        private MockedClient _client;
        private Monster _monster;

        [SetUp]
        public void Start()
        {
            Redis redis = new Redis();
            redis.Start();
            TestDb.Create();
            _server = new Server(new Server.ServerStartConfig() { Port = 123, DisableSpawners = true });
            // _server.StartGameThread();
            GameThread.TIME_MS_NOW = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            _player = new StoredPlayer()
            {
                UserId = "wololo",
                Login = "login",
                Password = "password",
                MoveSpeed = 1,
                X = 0,
                Y = 0
            };
            _client = ServerMocker.GetClient();
            _client.FullLoginSequence(_player);
            _monster = new Skeleton()
            {
                HP = 10,
                Position = new Position(1, 0)
            };

            Server.Events.Call(new EntitySpawnEvent()
            {
                Entity = _monster,
                Position = _monster.Position
            });

            // So we can send move events w/o worryng about cooldown check
            Server.GetPlayer(_player.UserId).MoveSpeed = int.MaxValue;
        }


        [TearDown]
        public void Stop()
        {
            _server.Stop();
        }

        [Test]
        public void TestTargettingTriggeringAttackIfClose()
        {
            var initialMonsterHp = _monster.HP;

            _client.RecievedPackets.Clear();
            _client.SendToServer(new EntityTargetPacket()
            {
                WhoUuid = _player.UserId,
                TargetUuid = _monster.UID
            });

            var attackPackets = _client.RecievedPackets.Where(p => p.GetType() == typeof(EntityAttackPacket))
                .ToList();

            Assert.That(attackPackets.Count > 0,
                "Player did not recieve any attack packets");

            Assert.That(_monster.HP < initialMonsterHp,
                "Targeting a monster close to you didnt triggered an attack");
        }

        [Test]
        public void TestAttackCooldown()
        {
            _client.SendToServer(new EntityTargetPacket()
            {
                WhoUuid = _player.UserId,
                TargetUuid = _monster.UID
            });

            _client.SendToServer(new EntityTargetPacket()
            {
                WhoUuid = _player.UserId,
                TargetUuid = _monster.UID
            });

            _client.RecievedPackets.Clear();

            _client.SendToServer(new EntityTargetPacket()
            {
                WhoUuid = _player.UserId,
                TargetUuid = _monster.UID
            });

            var attackPackets = _client.RecievedPackets
                .Where(p => p.GetType() == typeof(EntityAttackPacket))
                .Select(p => (EntityAttackPacket)p)
                .Where(p => p.AttackerUID == _player.UserId)
               .ToList();

            Assert.That(attackPackets.Count == 0,
                "Player could not perform 2 attacks while attack cooldown is active");
        }

        [Test]
        public void TestAttackWhenMovingClose()
        {
            _client.SendToServer(new EntityMovePacket()
            {
                UID = _player.UserId,
                To = new Position(-1, 0) // 1 tile far from the monster
            });

            _client.SendToServer(new EntityTargetPacket()
            {
                WhoUuid = _player.UserId,
                TargetUuid = _monster.UID
            });

            var attackPackets = _client.RecievedPackets
                .Where(p => p.GetType() == typeof(EntityAttackPacket))
                .Select(p => (EntityAttackPacket)p)
                .Where(p => p.AttackerUID == _player.UserId)
               .ToList();

            Assert.That(attackPackets.Count == 0,
                "Player is too far away to perform the attacks");

            _client.RecievedPackets.Clear();

            _client.SendToServer(new EntityMovePacket()
            {
                UID = _player.UserId,
                To = new Position(0, 0) // 1 tile closer from the monster
            });

            attackPackets = _client.RecievedPackets
            .Where(p => p.GetType() == typeof(EntityAttackPacket))
            .Select(p => (EntityAttackPacket)p)
            .Where(p => p.AttackerUID == _player.UserId)
           .ToList();

            Assert.That(attackPackets.Count == 1,
                "Player did not attack when moving close to his target");
        }

        [Test]
        public void TestEntityContinuousAttackBasedOnAttackSpeed()
        {
            var now = GameThread.TIME_MS_NOW;

            _client.RecievedPackets.Clear();

            var player = Server.GetPlayer(_player.UserId);
            var playerAtkSpeed = player.AtkSpeed;
            var playerAtkSpeedDelay = Formulas.GetTimeBetweenAttacks(playerAtkSpeed);

            _client.SendToServer(new EntityTargetPacket()
            {
                WhoUuid = _player.UserId,
                TargetUuid = _monster.UID
            });

            Assert.AreEqual(player.NextAttackAt, now + playerAtkSpeedDelay);

            GameThread.TIME_MS_NOW = now + playerAtkSpeedDelay;
            GameScheduler.RunTasks(now + playerAtkSpeedDelay);

            GameThread.TIME_MS_NOW = now + (playerAtkSpeedDelay * 2);
            GameScheduler.RunTasks(now + (playerAtkSpeedDelay * 2));

            GameThread.TIME_MS_NOW = now + (playerAtkSpeedDelay*3);
            GameScheduler.RunTasks(now + (playerAtkSpeedDelay*3));

            var attackPackets = _client.RecievedPackets
            .Where(p => p.GetType() == typeof(EntityAttackPacket))
            .Select(p => (EntityAttackPacket)p)
            .Where(p => p.AttackerUID == _player.UserId)
           .ToList();

            Assert.AreEqual(attackPackets.Count, 4);
        }

        [Test]
        public void TestAttackingWhenTargetIsMovingClose()
        {
            _client.SendToServer(new EntityMovePacket()
            {
                UID = _player.UserId,
                To = new Position(-1, 0) // 1 tile far from the monster
            });

            _client.SendToServer(new EntityTargetPacket()
            {
                WhoUuid = _player.UserId,
                TargetUuid = _monster.UID
            });

            var attackPackets = _client.RecievedPackets
                .Where(p => p.GetType() == typeof(EntityAttackPacket))
                .Select(p => (EntityAttackPacket)p)
                .Where(p => p.AttackerUID == _player.UserId)
               .ToList();

            Assert.That(attackPackets.Count == 0,
                "Player is too far away to perform the attacks");

            _client.RecievedPackets.Clear();

            _client.SendToServer(new EntityMovePacket()
            {
                UID = _monster.UID,
                To = new Position(0, 0) // 1 tile close to the monster
            });

            attackPackets = _client.RecievedPackets
            .Where(p => p.GetType() == typeof(EntityAttackPacket))
            .Select(p => (EntityAttackPacket)p)
            .Where(p => p.AttackerUID == _player.UserId)
           .ToList();

            Assert.That(attackPackets.Count == 1,
                "Player did not attack when his target moved close to him");

        }

    }
}

