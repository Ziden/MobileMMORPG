using NUnit.Framework;
using MapHandler;
using ServerCore.Game.GameMap;
using CommonCode.Pathfinder;

namespace MapTests
{
    [TestFixture]
    public class MapTests
    {
        [Test]
        public void Test()
        {
            WorldMap<Chunk> map = MapLoader.LoadMapFromFile<Chunk>("world");

            var iniChunk = map.GetChunk(0, 0);

            var passableArray = PathfinderHelper.GetPassableByteArray(new Position(0, 0), new Position(15, 15), map.Chunks);

           
        }
    }
}

