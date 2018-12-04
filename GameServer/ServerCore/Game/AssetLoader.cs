using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ServerCore.Game.GameMap
{
    public class AssetLoader
    {
        private static Dictionary<string, byte[]> _imageCache = new Dictionary<string, byte[]>();

        private static Dictionary<string, byte[]> _animations = new Dictionary<string, byte[]>();

        // Image must be declared as "Embedded Resource" in the project to be read like this
        public static byte[] LoadImageData(string imageName)
        {
            if(_imageCache.ContainsKey(imageName))
            {
                return _imageCache[imageName];
            }

            var mapAssembly = AppDomain.CurrentDomain.GetAssemblies().Last(a => a.FullName.Contains("ServerCore,"));
            var tileset = mapAssembly.GetManifestResourceNames()
                .Where(resourceName => resourceName.Contains(imageName))
                .Where(resourceName => resourceName.EndsWith(".png")).FirstOrDefault();

            using (var stream = mapAssembly.GetManifestResourceStream(tileset))
            {
                _imageCache[imageName] = ReadFully(stream);
                return _imageCache[imageName];
            }
        }

        public static List<String> GetAnimations()
        {
            return _animations.Keys.ToList();
        }

        public static byte [] GetAnimation(string name)
        {
            return _animations[name];
        }

        public static List<string> LoadAnimations()
        {
            var mapAssembly = AppDomain.CurrentDomain.GetAssemblies().Last(a => a.FullName.Contains("ServerCore,"));
            var animations = mapAssembly.GetManifestResourceNames()
                .Where(resourceName => resourceName.Contains("animations"))
                .Where(resourceName => resourceName.EndsWith(".png")).ToList();

            foreach(var anim in animations)
            {
                var splitName = anim.Split(".");
                var animName = $"{splitName[splitName.Length - 2]}.{splitName[splitName.Length - 1]}";

                using (var stream = mapAssembly.GetManifestResourceStream(anim))
                {
                    _animations.Add(animName, ReadFully(stream));
                }         
            }

            return _animations.Keys.ToList();
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
            Log.Info("Loading Map");
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
