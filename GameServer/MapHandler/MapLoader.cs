using MapParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MapHandler
{
    public class MapLoader
    {

        public static List<TmxMap> GetAll()
        {
            List<TmxMap> maps = new List<TmxMap>();

            var mapAssembly = AppDomain.CurrentDomain.GetAssemblies().Last(a => a.FullName.Contains("MapHandler,"));
            var mapNames = mapAssembly.GetManifestResourceNames().Where(resourceName => resourceName.EndsWith(".tmx"));

            foreach (var mapName in mapNames)
            {
                using (var stream = mapAssembly.GetManifestResourceStream(mapName))
                {
                    var map = new TmxMap(stream);
                    maps.Add(map);
                }
            }
            return maps;
        }

        public static XDocument GetXDocument(string name)
        {
            var mapAssembly = AppDomain.CurrentDomain.GetAssemblies().Last(a => a.FullName.Contains("MapHandler,"));

            var test = mapAssembly.GetManifestResourceNames();

            var resource = mapAssembly.GetManifestResourceNames().First(resourceName => resourceName.EndsWith(name));
            using (var stream = mapAssembly.GetManifestResourceStream(resource))
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    return XDocument.Load(reader);
                }
            }

        }

    }
}
