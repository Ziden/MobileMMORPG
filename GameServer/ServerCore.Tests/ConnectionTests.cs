using NUnit.Framework;
using System.Threading;
using System;
using ServerCore.Networking;
using System.Net.Sockets;

[TestFixture]
public class ConnectionTests
{


    [Test]
    public void TestSimpleConnections()
    {
        // Starting the Server
        var ServerTcpHandler = new ServerTcpHandler();

        ServerTcpHandler.StartListening(8888);

        int numberOfClients = 3;

        // Connecting a client to the server
        for(var x = 0; x < numberOfClients; x++)
        {
            var client = new ConnectedClientTcpHandler()
            {
                TcpClient = new TcpClient("localhost", 8888),
                ConnectionId = Guid.NewGuid().ToString()
            };
        }
        
        // Wait for all the connections
        Thread.Sleep(1000);

        Assert.AreEqual(numberOfClients, ServerTcpHandler.ConnectedSockets());
    }
}

