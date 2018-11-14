using MapHandler;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ServerCore.Game.GameMap
{
    public class MapLoader
    {
        public static byte[] LoadImageData(string tilesetName)
        {
            var mapAssembly = AppDomain.CurrentDomain.GetAssemblies().Last(a => a.FullName.Contains("ServerCore,"));
            var tileset = mapAssembly.GetManifestResourceNames()
                .Where(resourceName => resourceName.Contains(tilesetName))
                .Where(resourceName => resourceName.EndsWith(".png")).FirstOrDefault();

            using (var stream = mapAssembly.GetManifestResourceStream(tileset))
            {
                return ReadFully(stream);
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static ServerMap LoadMapFromFile(string searchMapName = null)
        {
            Console.WriteLine("Loading Map");
            var mapAssembly = AppDomain.CurrentDomain.GetAssemblies().Last(a => a.FullName.Contains("ServerCore,"));
            var mapNames = mapAssembly.GetManifestResourceNames().Where(resourceName => resourceName.EndsWith(".tmx"));
            if(searchMapName != null)
            {
                mapNames = mapNames.Where(mapName => mapName.Contains(searchMapName));
            }
            foreach (var mapName in mapNames)
            {
                using (var stream = mapAssembly.GetManifestResourceStream(mapName))
                {
                    var map = MapParser.Parse(stream);
                    return map;
                }
            }
            return null;
        }

        public static XDocument GetXDocument(string name)
        {
            var mapAssembly = AppDomain.CurrentDomain.GetAssemblies().Last(a => a.FullName.Contains("ServerCore,"));

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
