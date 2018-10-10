using System.IO;
using System.Reflection;
using MapParser;
using NUnit.Framework;
using System;
using System.Linq;
using MapHandler;
using System.Collections.Generic;

namespace TiledSharpTesting
{
    [TestFixture]
    public class MapLoaderTests
    {

        [Test]
        public void TestSimpleLoading()
        {
            List<TmxMap> mapNames = MapLoader.GetAll();

            Assert.That(mapNames.Count > 0);
        }

 
    }

}

