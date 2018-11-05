using NUnit.Framework;
using Storage;
using Storage.Players;
using System;
using System.Linq.Expressions;

[TestFixture]
public class TestPlayer
{
    [SetUp]
    public void MakeThisShitReady()
    {
        var redis = new Redis();
        redis.Start();
        Redis.Server.FlushAllDatabases();
    }

    [Test]
    public void TestPlayerDaoSaveRead ()
    {
        var player = new Player();
        player.UserId = "123";
        player.Login = "wololo";
        player.Password = "walala";

        RedisHash<Player>.Set(player);
        var user = RedisHash<Player>.Get("123");

        Assert.AreEqual(user.Login, player.Login);
        Assert.AreEqual(user.Password, player.Password);
    }
}

