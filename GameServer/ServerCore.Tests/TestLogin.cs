using NUnit.Framework;
using Storage;
using Storage.TestDataBuilder;
using ServerCore;
using ServerCore.Tests.TestUtilities;
using Storage.Players;
using Common.Networking.Packets;
using Storage.Login;
using System.Linq;
using CommonCode.Networking.Packets;

namespace MapTests
{
    [TestFixture]
    public class TestLogin
    {
        private Player _player;
        private Server _server;

        [SetUp]
        public void Start()
        {
            Redis redis = new Redis();
            redis.Start();
            TestDb.Create();
            _server = new Server(1234);
            _server.StartGameThread();

            _player = new Player()
            {
                UserId = "wololo",
                Login = "login",
                Password = "password"
            };
        }

        [TearDown]
        public void stop()
        {
            _server.Stop();
        }

        [Test]
        public void TestChunksRecieved()
        {
            var client = ServerMocker.GetClient();

            client.FullLoginSequence(_player);

            Assert.That(client.RecievedPackets.Where(p => p.GetType() == typeof(ChunkPacket)).ToList().Count == 9,
                "Player should have recieved 9 chunk packets (3x3)");
        }

        [Test]
        public void TestPlayerPacketBeingRecieved()
        {

            var client = ServerMocker.GetClient();

            client.FullLoginSequence(_player);

            Assert.That(client.RecievedPackets.Any(p => p.GetType() == typeof(PlayerPacket)),
                "Player didnt got his own player packet with his info");
        }
        


        [Test]
        public void TestDownloadAssetsSequence()
        {
            var client = ServerMocker.GetClient();
            AccountService.RegisterAccount(_player.Login, _player.Password, _player.Email, _player);
            client.Login(_player.Login, _player.Password);

            var assetsRequiredPackets = client.RecievedPackets.
                Where(p => p.GetType() == typeof(AssetPacket))
                .ToList();

            // sending to server that i dont own this assets client-side, please send me mister server
            client.RecievedPackets.Clear();
            foreach (var assetPacket in assetsRequiredPackets)
            {
                client.SendToServer(assetPacket);
            }

            Assert.That(client.RecievedPackets.Count == assetsRequiredPackets.Count,
                "Client did not recieve all assets he asked for");
        }

    }
}

