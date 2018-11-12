using CommonCode.Pathfinder;
using MapHandler;
using NUnit.Framework;
using System.Collections.Generic;

namespace Common.Tests
{
    [TestFixture]
    public class TestPathfinder
    {

        public static short BLOCK = 2;
        public static short PASSABLE = 1;

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
                    chunk1.SetTile(xx, yy, PASSABLE);
                }
            }
            return chunk1;
        }


        [Test]
        public void Test1()
        {

            var chunk1 = CreateChunk(0, 0);
            chunk1.SetTile(1, 0, BLOCK);
            chunk1.SetTile(1, 1, BLOCK);
            chunk1.SetTile(1, 2, BLOCK);

            var chunks = new Dictionary<string, Chunk>();
            chunks.Add("0_0", chunk1);

            var path = WorldMap<Chunk>.FindPath(new Position(0, 0), new Position(2, 0), chunks);
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
            chunk1.SetTile(0, 0, BLOCK);

            var chunk2 = CreateChunk(-1, 0);

            var chunks = new Dictionary<string, Chunk>();
            chunks.Add("0_0", chunk1);
            chunks.Add("-1_0", chunk2);

            var path = WorldMap<Chunk>.FindPath(new Position(1, 0), new Position(-1, 0), chunks);
            Assert.That(path.Count == 5);   
        }

        [Test]
        public void TestNegativeMultiChunkY()
        {

            var chunk1 = CreateChunk(0, 0);
            chunk1.SetTile(0, 0, BLOCK);

            var chunk2 = CreateChunk(0, -1);

            var chunks = new Dictionary<string, Chunk>();
            chunks.Add("0_0", chunk1);
            chunks.Add("0_-1", chunk2);

            var path = WorldMap<Chunk>.FindPath(new Position(0, 1), new Position(0, -1), chunks);
            Assert.That(path.Count == 5);
        }

        [Test]
        public void TestCrossingUnecessaryChunkStillGeneratesPassableArrayForHim()
        {

            var chunk1 = CreateChunk(0, 0);
            chunk1.SetTile(0, 1, BLOCK);
            chunk1.SetTile(1, 0, BLOCK);

            var chunk2 = CreateChunk(0, -1);

            var chunks = new Dictionary<string, Chunk>();
            chunks.Add("0_0", chunk1);
            chunks.Add("0_-1", chunk2);

            var grid = PathfinderHelper.GetPassableByteArray(new Position(0, 0), new Position(0, 2), chunks);

            Assert.AreEqual(48, grid.PassableMap.GetLength(0));
            Assert.AreEqual(48, grid.PassableMap.GetLength(1));

        }

        [Test]
        public void TestMultiChunkX()
        {

            var chunk1 = CreateChunk(0, 0);
            var chunk2 = CreateChunk(1, 0);

            var chunks = new Dictionary<string, Chunk>();
            chunks.Add("0_0", chunk1);
            chunks.Add("1_0", chunk2);

            var path = WorldMap<Chunk>.FindPath(new Position(0, 0), new Position(19, 0), chunks);
            Assert.That(path.Count == 20);
        }

        [Test]
        public void TestBiggerMap()
        {

            var map = new WorldMap<Chunk>();

            var chunk1 = CreateChunk(0, 0);
            var chunk2 = CreateChunk(1, 0);
            var chunk3 = CreateChunk(-1, 0);
            //chunk3.SetTile(15, 0, 2);
            var chunk4 = CreateChunk(0, 1);
            var chunk5 = CreateChunk(0, -1);
            var chunk6 = CreateChunk(1, 1);
            var chunk7 = CreateChunk(-1, -1);
            var chunk8 = CreateChunk(-1, 1);
            var chunk9 = CreateChunk(1, -1);

            map.AddChunk(chunk1);
            map.AddChunk(chunk2);
            map.AddChunk(chunk3);
            map.AddChunk(chunk4);
            map.AddChunk(chunk5);
            map.AddChunk(chunk6);
            map.AddChunk(chunk7);
            map.AddChunk(chunk8);
            map.AddChunk(chunk9);

            var passableMapArray = PathfinderHelper.GetPassableByteArray(new Position(0, 0), new Position(-1, 0), map.Chunks);

            var path = WorldMap<Chunk>.FindPath(new Position(0, 0), new Position(-1, 0), map.Chunks);
            Assert.That(path.Count == 2);

            Assert.That(path[0].X == 0 && path[0].Y == 0);
            Assert.That(path[1].X == -1 && path[1].Y == 0);
        }
    }
}
