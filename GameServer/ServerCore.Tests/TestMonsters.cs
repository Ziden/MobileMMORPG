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
    public class TestMonsters
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
        public void TestMonsterMovingUpdatingClients()
        {
            var client = ServerMocker.GetClient();
            var player = client.FullLoginSequence(_player);

            var skeleton = new Skeleton()
            {
                Position = new Position(0, 1)
            };
            var chunk = player.GetChunk();

            chunk.EntitiesInChunk[EntityType.MONSTER].Add(skeleton);

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
                Position = new Position(45, 0)
            };

            Server.Events.Call(new EntitySpawnEvent()
            {
                Entity = skeleton,
                Position = skeleton.Position
            });

            var monsterChunk = Server.Map.GetChunkByChunkPosition(2,0);
            var playerChunki = Server.Map.GetChunkByChunkPosition(0, 0);

            var chunks = Server.Map.Chunks;

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
        public void TestMonsterMovingOnDifferentChunk()
        {
            var client = ServerMocker.GetClient();
            var player = client.FullLoginSequence(_player);

            var skeleton = new Skeleton()
            {
                Position = new Position(0, 22)
            };

            var monsterChunk = Server.Map.GetChunkByChunkPosition(0, 1);
            var playerChunki = Server.Map.GetChunkByChunkPosition(0, 0);

            monsterChunk.EntitiesInChunk[EntityType.MONSTER].Add(skeleton);

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
        public void TestMonsterDieCleansMonsterData()
        {
            var client = ServerMocker.GetClient();
            var player = client.FullLoginSequence(_player);

            var skeleton = new Skeleton()
            {
                Position = new Position(0, 1)
            };
            Server.Events.Call(new EntitySpawnEvent()
            {
                Entity = skeleton,
                Position = skeleton.Position
            });
            skeleton.MovementBehaviour = null;
            skeleton.MovementTick();
            
            var skeletonMoveTask = GameScheduler.GetTask(skeleton.MovementTaskId);

            Assert.That(skeletonMoveTask != null,
                "Monster movement task should be created upon spawning");

            player.Atk = 5;
            player.AtkSpeed = 99999999;
            skeleton.Def = 0;

            player.TryAttacking(skeleton, singleHit:true);

            Assert.That(skeleton.HP == skeleton.MAXHP - 5, 
                "5 atk on 0 def should deal 5 damage");

            player.Atk = 5000;

            player.TryAttacking(skeleton, singleHit:true);

            var moveTask = GameScheduler.GetTask(skeleton.MovementTaskId);
            var attackTask = GameScheduler.GetTask(skeleton.AttackTaskId);

            Assert.That(skeleton.HP < 0,
                "Skeleton should have < 0 hp as he took 5k damage");

            Assert.That(!Server.Map.Monsters.ContainsKey(skeleton.UID),
                "Server should not contain the skeleton as its dead.");

            Assert.That(moveTask == null,
                "Skeleton is dead and his move task should have been cancelled");

            Assert.That(attackTask == null,
             "Skeleton is dead and his move task should have been cancelled");

        }

        [Test]
        public void TestMonsterAttackingPlayer()
        {
            var client = ServerMocker.GetClient();
            var player = client.FullLoginSequence(_player);

            var skeleton = new Skeleton()
            {
                Position = new Position(0, 1)
            };
            Server.Events.Call(new EntitySpawnEvent()
            {
                Entity = skeleton,
                Position = skeleton.Position
            });

            var playerHpBefore = player.HP;

            client.RecievedPackets.Clear();

            Thread.Sleep(100); 

            Server.Events.Call(new EntityTargetEvent()
            {
                Entity = skeleton,
                TargetedEntity = player
            });

            var playerHpAfter = player.HP;

            var recievedDamagePackets = client.RecievedPackets
                .Where(p => p.GetType() == typeof(EntityAttackPacket))
                .Any();

            Assert.That(playerHpAfter < playerHpBefore,
                "Player should have taken a hit from monster");

           Assert.True(recievedDamagePackets,
                "Client did not recieved the skeleton attack packet");
        }

    }
}

