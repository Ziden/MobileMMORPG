using NUnit.Framework;
using MapHandler;
using ServerCore.Game.GameMap;
using ServerCore.Game.Monsters;
using CommonCode.Networking.Packets;

namespace MapTests
{
    [TestFixture]
    public class MapLoadingTests
    {
        [Test]
        public void TestSimpleLoading()
        {
            ServerMap map = AssetLoader.LoadMapFromFile();

            Assert.That(map.Tilesets.Count > 0);

            Assert.That(map.Chunks.Count >= 2);
        }

        [Test]
        public void TestTileIndexesCorrect()
        {
            ServerMap map = AssetLoader.LoadMapFromFile();

            var chunk0 = map.GetChunkByChunkPosition(0, 0);
            var chunk1 = map.GetChunkByChunkPosition(1, 0);

            Assert.That(chunk0.Tiles[0, 0].Position.X == 0);
            Assert.That(chunk1.Tiles[0, 0].Position.X == 16);
        }


        [Test]
        public void TestLoadingSpawners()
        {
            ServerMap map = AssetLoader.LoadMapFromFile("test");

            Assert.That(map.Spawners.Count > 0);

            var spawner = map.Spawners[0];

            Assert.That(spawner.SpawnerMobs.Count == 1);

            var spawnerMob = spawner.SpawnerMobs[0];

            Assert.AreEqual(typeof(Skeleton), spawnerMob.MonsterClassType);
            Assert.AreEqual(1, spawnerMob.Amount);

        }

        [Test]
        public void TestGeneralAssetLoading()
        {
            AssetLoader.LoadServerAssets();

            Assert.That(AssetLoader.LoadedAssets[AssetType.ANIMATION].Count > 0);
            Assert.That(AssetLoader.LoadedAssets[AssetType.TILESET].Count > 0);
            Assert.That(AssetLoader.LoadedAssets[AssetType.SPRITE].Count > 0);
        }

        [Test]
        public void TestMapLoadTestMap()
        {
            ServerMap map = AssetLoader.LoadMapFromFile("test_map");

            Assert.That(map.Chunks.Count >= 2);

            Chunk chunk = map.GetChunkByChunkPosition(0, 0);

            Assert.AreEqual(chunk.Tiles[0, 0].TileId, 1);
            Assert.AreEqual(chunk.Tiles[1, 0].TileId, 1);
            Assert.AreEqual(chunk.Tiles[2, 0].TileId, 1);

            chunk = map.GetChunkByChunkPosition(0, 0);
        }
    }
}

