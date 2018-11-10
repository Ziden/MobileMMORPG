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

    [Test]
    public void TestUpdatingPosition()
    {

        var player = new Player();
        player.UserId = "123";
        player.Login = "wololo";
        player.Password = "walala";
        player.X = 1;
        player.Y = 2;

        RedisHash<Player>.Set(player);

        PlayerService.UpdatePlayerPosition(player, 3, 4);

        var obtainedPlayer = RedisHash<Player>.Get("123");

        Assert.AreEqual(obtainedPlayer.X, 3);
        Assert.AreEqual(obtainedPlayer.Y, 4);
    }
}

