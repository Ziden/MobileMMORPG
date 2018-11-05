using Common.Networking;
using Common.Networking.Packets;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Tests
{
    [TestFixture]
    class TestPacketSerialization
    {
        [Test]
        public void TestLoginPacketSerialization()
        {
            LoginPacket loginPacket = new LoginPacket()
            {
                Login = "Wololo",
                Password = "Walala"
            };

            byte[] serialized = PacketSerializer.Serialize(loginPacket);

            object deserialized = PacketSerializer.Deserialize(serialized);

            Assert.That(deserialized is LoginPacket);

            LoginPacket deserializedLoginPacket = (LoginPacket)deserialized;

            Assert.AreEqual(deserializedLoginPacket.Login, loginPacket.Login);
            Assert.AreEqual(deserializedLoginPacket.Password, loginPacket.Password);
        }
    }
}
