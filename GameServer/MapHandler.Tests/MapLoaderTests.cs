using NUnit.Framework;
using MapHandler;

namespace MapTests
{
    [TestFixture]
    public class MapLoaderTests
    {
        [Test]
        public void TestSimpleLoading()
        {
            Map map = MapLoader.LoadMapFromFile();

            Assert.That(map.Tilesets.Count > 0);

            Assert.That(map.Chunks.Count >= 2);
        }

        [Test]
        public void TestMapLoadTestMap()
        {
            Map map = MapLoader.LoadMapFromFile("test_map");

            Assert.That(map.Chunks.Count == 2);

            Chunk chunk = map.GetChunk(0, 0);

            Assert.AreEqual(chunk.GetTile(0, 0), 1);
            Assert.AreEqual(chunk.GetTile(1, 0), 2);
            Assert.AreEqual(chunk.GetTile(2, 0), 3);
            Assert.AreEqual(chunk.GetTile(3, 0), 4);


            Assert.AreEqual(chunk.GetTile(0, 1), 4);
            Assert.AreEqual(chunk.GetTile(1, 1), 3);
            Assert.AreEqual(chunk.GetTile(2, 1), 2);
            Assert.AreEqual(chunk.GetTile(3, 1), 1);

            chunk = map.GetChunk(0, 0);
        }
    }
}

