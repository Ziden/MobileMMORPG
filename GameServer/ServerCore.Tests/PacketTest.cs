using NUnit.Framework;
using System.Threading;
using System;
using ServerCore;
using Common.Networking.Packets;
using ServerCore.Networking;
using Storage;
using Storage.TestDataBuilder;
using ServerCore.Tests.TestUtilities;
using System.Linq;
using System.Net.Sockets;

[TestFixture]
public class PacketTests
{

    private ConnectedClientTcpHandler _client;
    private Server _server;

    [SetUp]
    public void Prepare()
    {
        _server = new Server(8886);
        _server.StartListeningForPackets();

        Redis redis = new Redis();
        redis.Start();

        TestDb.Create();

        _client = new ConnectedClientTcpHandler(new TcpClient("localhost", 8886));
    }

    [TearDown]
    public void TearDown()
    {
        _client.Stop();
    }

    [Test]
    // Test to ensure server is recieving the packets and keeping the recieve order
    public void TestLoginPacketQueue()
    {
        _client.Send(new LoginPacket()
        {
            Login = "admin",
            Password = "wololo"
        });

        _client.Send(new LoginPacket()
        {
            Login = "admin2",
            Password = "wololo2"
        });

        Waiter.WaitUntil(() => Server.PacketsToProccess.Count == 2);

        // Get the packets that the server recieved, in order and
        // check if its the correct packets
        BasePacket packet = null;
        Server.PacketsToProccess.TryDequeue(out packet);
        var loginPacket = (LoginPacket)packet;
        Assert.AreEqual("admin", loginPacket.Login);
        Assert.AreEqual("wololo", loginPacket.Password);

        Server.PacketsToProccess.TryDequeue(out packet);
        loginPacket = (LoginPacket)packet;
        Assert.AreEqual("admin2", loginPacket.Login);
        Assert.AreEqual("wololo2", loginPacket.Password);
    }
}

