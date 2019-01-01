using MapHandler;
using NUnit.Framework;
using System;

[TestFixture]
public class TestPosition
{
    [Test]
    public void TestPositionBasicAngleCoordinates()
    {
        var angle90 = new Position(0, 0).GetAngleRadiansTo(new Position(0, 1));
        var angle180 = new Position(0, 0).GetAngleRadiansTo(new Position(-1, 0));
        var angle270 = new Position(0, 0).GetAngleRadiansTo( new Position(0, -1));
        var angle0 = new Position(0, 0).GetAngleRadiansTo(new Position(1, 0));

        Assert.AreEqual(angle90, Math.PI / 2);
        Assert.AreEqual(angle180, Math.PI);
        Assert.AreEqual(angle270, -Math.PI / 2);
        Assert.AreEqual(angle0, 0);
    }

    [Test]
    public void TestPositionDirections()
    {
        var direction = new Position(0, 0).GetDirection(new Position(10, 11));
        Assert.AreEqual(direction, Direction.NORTH);

        direction = new Position(0, 0).GetDirection(new Position(10, 9));
        Assert.AreEqual(direction, Direction.RIGHT);

        direction = new Position(0, 0).GetDirection(new Position(-1, 0));
        Assert.AreEqual(direction, Direction.LEFT);

        direction = new Position(0, 0).GetDirection(new Position(0, -1));
        Assert.AreEqual(direction, Direction.SOUTH);
    }
}

