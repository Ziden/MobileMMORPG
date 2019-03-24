using NUnit.Framework;
using System;
using ServerCore;
using Common.Networking.Packets;
using ServerCore.Networking;
using Storage;
using Storage.TestDataBuilder;
using ServerCore.Tests.TestUtilities;
using System.Net.Sockets;
using static ServerCore.Server;
using System.Collections.Generic;
using Storage.Login;
using Storage.Players;

[TestFixture]
public class PacketTests
{
    private ConnectedClientTcpHandler _client;
    private Server _server;

    [SetUp]
    public void Prepare()
    {
        _server = new Server(new ServerStartConfig() { Port = 1212 });
        _server.StartListeningForPackets();

        Redis redis = new Redis();
        redis.Start();

        TestDb.Create();

        _client = new ConnectedClientTcpHandler()
        {
            TcpClient = new TcpClient("localhost", 1212),
            ConnectionId = Guid.NewGuid().ToString()
        };
    }

    [TearDown]
    public void TearDown()
    {
        _client.Stop();
        _server.Stop();
    }

    [Test]
    // Test to ensure server is recieving the packets and keeping the recieve order
    public void Test2LoginProcesses()
    {
        var tcpH = Server.TcpHandler;

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

    [Test]
    public void TestMultipleClients()
    {
        var clients = new List<ConnectedClientTcpHandler>();
        for (var x = 0 ; x < 5; x++) {
            var client = new ConnectedClientTcpHandler()
            {
                TcpClient = new TcpClient("localhost", 1234),
                ConnectionId = Guid.NewGuid().ToString()
            };
            clients.Add(client);
        }

        foreach(var client in clients)
        {

        }
    }

    private void FullLoginSequence(ConnectedClientTcpHandler client)
    {
        var loginpass = Guid.NewGuid().ToString();
        var player = new StoredPlayer()
        {
            UserId = Guid.NewGuid().ToString(),
            Login = loginpass,
            Password = loginpass
        };
        AccountService.RegisterAccount(loginpass, loginpass, "", player);
        client.Send(new LoginPacket()
        {
            Login = loginpass,
            Password = loginpass
        });
    }

}

