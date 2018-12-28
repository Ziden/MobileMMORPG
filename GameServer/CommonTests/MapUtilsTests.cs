using NUnit.Framework;
using MapHandler;
using System;

namespace MapTests
{
    [TestFixture]
    public class MapUtilsTests
    {
        [Test]
        public void TestMapRadius1()
        {
            var pos = new Position(0, 0);
            var radius = pos.GetRadius(1);

            Assert.That(radius.Count == 5);
        }
        
        [Test]
        public void TestMapRadiusNoIncludeSelf()
        {
            var pos = new Position(0, 0);
            var radius = pos.GetRadius(1, false);

            Assert.That(radius.Count == 4);
        }

        [Test]
        public void TestMapRadius3()
        {
            var pos = new Position(0, 0);
            var radius = pos.GetRadius(3);

            Assert.That(radius.Count == 25);
        }
    }
}

