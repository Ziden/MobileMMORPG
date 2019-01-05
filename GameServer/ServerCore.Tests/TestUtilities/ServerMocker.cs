using Common.Networking.Packets;
using Moq;
using ServerCore.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ServerCore.Tests.TestUtilities
{
    // For Testing and exploding code purposes
    public static class ServerMocker
    {
        public static MockedClient GetClient()
        {
            var mockedClient = new Mock<MockedClient>();
            // Whenever the sever wants to send something we will just add to a "mocked sent list"
            mockedClient.Setup(x => x.Send(It.IsAny<BasePacket>()))
            .Returns((BasePacket packet) => 
                mockedClient.Object.MockRecieve(packet));

            return mockedClient.Object;
        }
    }

    public class MockedClient : ConnectedClientTcpHandler
    {
        public MockedClient()
        {
            this.ConnectionId = Guid.NewGuid().ToString();
        }

        public List<BasePacket> RecievedPackets = new List<BasePacket>();

        public bool MockRecieve(BasePacket packet)
        {
            RecievedPackets.Add(packet);
            return true;
        }

        public void SendToServer(BasePacket packet)
        {
            packet.ClientId = this.ConnectionId;
            Server.Events.Call(packet);
        }

    }
}
