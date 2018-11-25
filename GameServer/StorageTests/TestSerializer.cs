using NUnit.Framework;
using StackExchange.Redis;
using Storage;
using Storage.Players;
using Storage.TestDataBuilder;
using System.Linq;
using System.Net.Sockets;

[TestFixture]
public class TestSerializer
{

    private HashEntry? FindEntry(string key, HashEntry [] list) {
        foreach(var entry in list)
        {
            if (entry.Name == key)
                return entry;
        }
        return null;
    }

    [Test]
    public void TestSimpleSerialization ()
    {
        var player = new Player();
        player.Email = "wololo";
        player.Login = "walala";
        player.MoveSpeed = 15;
        var serializedPlayer = DataSerializer.ToRedisHash(player);

        var emailEntry = FindEntry("e", serializedPlayer);
        var loginEntry = FindEntry("l", serializedPlayer);
        var speedEntry = FindEntry("s", serializedPlayer);

        Assert.That(emailEntry.HasValue);
        Assert.That(loginEntry.HasValue);
        Assert.That(speedEntry.HasValue);

        Assert.That(emailEntry.Value.Value == player.Email);
        Assert.That(loginEntry.Value.Value == player.Login);
        Assert.That(speedEntry.Value.Value == player.MoveSpeed);
    }

    [Test]
    public void TestSimpleDeSerialization()
    {
        var player = new Player();
        player.Email = "wololo";
        player.Login = "walala";
        player.MoveSpeed = 15;
        var serializedPlayer = DataSerializer.ToRedisHash(player);

        var player2 = DataSerializer.FromRedisHash<Player>(serializedPlayer);

        Assert.AreEqual(player.Login, player2.Login);
        Assert.AreEqual(player.Email, player2.Email);
        Assert.AreEqual(player.MoveSpeed, player2.MoveSpeed);

    }
}

