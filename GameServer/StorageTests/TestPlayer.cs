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
        var player = new StoredPlayer();
        player.UserId = "123";
        player.Login = "wololo";
        player.Password = "walala";

        RedisHash<StoredPlayer>.Set(player);
        var user = RedisHash<StoredPlayer>.Get("123");

        Assert.AreEqual(user.Login, player.Login);
        Assert.AreEqual(user.Password, player.Password);
    }

    [Test]
    public void TestUpdatingPosition()
    {

        var player = new StoredPlayer();
        player.UserId = "123";
        player.Login = "wololo";
        player.Password = "walala";
        player.X = 1;
        player.Y = 2;

        RedisHash<StoredPlayer>.Set(player);

        PlayerService.UpdatePlayerPosition(player.UserId, 3, 4);

        var obtainedPlayer = RedisHash<StoredPlayer>.Get("123");

        Assert.AreEqual(obtainedPlayer.X, 3);
        Assert.AreEqual(obtainedPlayer.Y, 4);
    }
}

