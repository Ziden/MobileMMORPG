using CommonCode.Pathfinder;
using MapHandler;
using NUnit.Framework;

namespace Common.Tests
{
    [TestFixture]
    public class TestPathfinder
    {
        public static short BLOCK = 2;
        public static short PASSABLE = 1;

        private WorldMap<Chunk> _map;

        [SetUp]
        public void Setup()
        {
            _map = new WorldMap<Chunk>();
        }

        private Chunk CreateChunk(int x, int y)
        {
            var chunk1 = new Chunk()
            {
                x = x,
                y = y
            };

            for(int xx = 0; xx < 16; xx++)
            {
                for (int yy = 0; yy < 16; yy++)
                {
                    chunk1.Tiles[xx, yy] = new MapTile(new Position(xx, yy), PASSABLE);
                }
            }
            return chunk1;
        }

        [Test]
        public void TestPathfinderDistanceLimit()
        {
            var chunk1 = CreateChunk(0, 0);
            chunk1.Tiles[1, 0].TileId = BLOCK;

            var path = _map.FindPath(new Position(10, 0), new Position(0, 0));

           // Assert.That(path.Count )
        }

        [Test]
        public void TestSomePathfinding()
        {
            var chunk1 = CreateChunk(0, 0);
            chunk1.Tiles[1, 0].TileId = BLOCK;
            chunk1.Tiles[1, 1].TileId = BLOCK;
            chunk1.Tiles[1, 2].TileId = BLOCK;
            _map.AddChunk(chunk1);

            var path = _map.FindPath(new Position(0, 0), new Position(2, 0));
            Assert.That(path.Count == 9);

            Assert.That(path[0].X == 0 && path[0].Y==0);
            Assert.That(path[1].X == 0 && path[1].Y == 1);
            Assert.That(path[2].X == 0 && path[2].Y == 2);
            Assert.That(path[3].X == 0 && path[3].Y == 3);
            Assert.That(path[4].X == 1 && path[4].Y == 3);
            Assert.That(path[5].X == 2 && path[5].Y == 3);
            Assert.That(path[6].X == 2 && path[6].Y == 2);
            Assert.That(path[7].X == 2 && path[7].Y == 1);
            Assert.That(path[8].X == 2 && path[8].Y == 0);
        }

        [Test]
        public void TestNegativeMultiChunk()
        {

            var chunk1 = CreateChunk(0, 0);
            chunk1.Tiles[0, 0].TileId = BLOCK;

            var chunk2 = CreateChunk(-1, 0);

            _map.AddChunk(chunk1);
            _map.AddChunk(chunk2);

            var path = _map.FindPath(new Position(1, 0), new Position(-1, 0));
            Assert.That(path.Count == 5);   
        }

        [Test]
        public void TestGeneratePassableByteArrayNegativeChunks()
        {
            var chunk1 = CreateChunk(0, 0);
            chunk1.Tiles[0, 0].TileId = BLOCK;

            var chunk2 = CreateChunk(-1, 0);

            _map.AddChunk(chunk1);
            _map.AddChunk(chunk2);

            var passableArrayResponse = _map.GetPassableByteArray(new Position(1, 0), new Position(-1, 0));

            Assert.That(passableArrayResponse.OffsetY == 1,
                "Since we had a negative chunk, our offset Y should be added to still be able to make the 2d array");
        }

        [Test]
        public void TestNegativeMultiChunkY()
        {

            var chunk1 = CreateChunk(0, 0);
            chunk1.Tiles[0, 0].TileId = BLOCK;

            var chunk2 = CreateChunk(0, -1);

            _map.AddChunk(chunk1);
            _map.AddChunk(chunk2);

            var path = _map.FindPath(new Position(0, 1), new Position(0, -1));
            Assert.That(path.Count == 5);
        }

        [Test]
        public void TestCrossingUnecessaryChunkStillGeneratesPassableArrayForHim()
        {

            var chunk1 = CreateChunk(0, 0);
            chunk1.Tiles[0, 1].TileId = BLOCK;
            chunk1.Tiles[1, 0].TileId = BLOCK;

            var chunk2 = CreateChunk(0, -1);

            _map.AddChunk(chunk1);
            _map.AddChunk(chunk2);

            var grid = _map.GetPassableByteArray(new Position(0, 0), new Position(0, 2));

            Assert.AreEqual(48, grid.PassableMap.GetLength(0));
            Assert.AreEqual(48, grid.PassableMap.GetLength(1));
        }

        [Test]
        public void TestMultiChunkX()
        {

            var chunk1 = CreateChunk(0, 0);
            var chunk2 = CreateChunk(1, 0);

            _map.AddChunk(chunk1);
            _map.AddChunk(chunk2);

            var path = _map.FindPath(new Position(0, 0), new Position(19, 0));
            Assert.That(path.Count == 20);
        }

        [Test]
        public void TestBiggerMap()
        {

            var chunk1 = CreateChunk(0, 0);
            var chunk2 = CreateChunk(1, 0);
            var chunk3 = CreateChunk(-1, 0);
            var chunk4 = CreateChunk(0, 1);
            var chunk5 = CreateChunk(0, -1);
            var chunk6 = CreateChunk(1, 1);
            var chunk7 = CreateChunk(-1, -1);
            var chunk8 = CreateChunk(-1, 1);
            var chunk9 = CreateChunk(1, -1);

            _map.AddChunk(chunk1);
            _map.AddChunk(chunk2);
            _map.AddChunk(chunk3);
            _map.AddChunk(chunk4);
            _map.AddChunk(chunk5);
            _map.AddChunk(chunk6);
            _map.AddChunk(chunk7);
            _map.AddChunk(chunk8);
            _map.AddChunk(chunk9);

            var passableMapArray =_map.GetPassableByteArray(new Position(0, 0), new Position(-1, 0));

            var path = _map.FindPath(new Position(0, 0), new Position(-1, 0));
            Assert.That(path.Count == 2);

            Assert.That(path[0].X == 0 && path[0].Y == 0);
            Assert.That(path[1].X == -1 && path[1].Y == 0);
        }

        [Test]
        public void TestPassableBlockArrayRandomBug()
        {
            for(int x = 0; x < 10; x++)
            {
                for(int y = 0; y < 10; y++)
                {
                    _map.AddChunk(CreateChunk(x, y));
                }
            }
          
            var path = _map.FindPath(new Position(0, 0), new Position(2, 0));

            Assert.That(path.Count == 3);
        }
    }
}
