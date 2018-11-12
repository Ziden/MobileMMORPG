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
            var radius = MapUtils.GetRadius(0, 0, 1);

            Assert.That(radius.Count == 5);
        }

        [Test]
        public void ASD()
        {

        }

        [Test]
        public void TestMapRadius3()
        {
            var radius = MapUtils.GetRadius(0, 0, 3);

            Assert.That(radius.Count == 25);
        }
    }
}

