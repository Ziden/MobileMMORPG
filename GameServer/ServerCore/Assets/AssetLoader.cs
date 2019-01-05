using CommonCode.Networking.Packets;
using ServerCore.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ServerCore.Game.GameMap
{
    // TODO: Rework this when we have sound assets or other types
    public class LoadedAssets : Dictionary<AssetType, List<ImageAsset>>
    {
        public void AddAsset(AssetType type, ImageAsset asset)
        {
            if (!this.ContainsKey(type))
            {
                this[type] = new List<ImageAsset>();
            }
            this[type].Add(asset);
        }

        public ImageAsset GetAsset(AssetType type, string name)
        {
            if (!this.ContainsKey(type))
                return null;
            var list = this[type];
            return list.Where(asset => asset.ImageName.ToLower() == name.ToLower()).First();
        }
    }

    public class AssetLoader
    {
        public static LoadedAssets LoadedAssets = new LoadedAssets();

        private static void LoadAssetType(AssetType type, string assetFolderName)
        {
            var mapAssembly = AppDomain.CurrentDomain.GetAssemblies().Last(a => a.FullName.Contains("ServerCore,"));
            var assets = mapAssembly.GetManifestResourceNames()
                .Where(resourceName => resourceName.Contains(assetFolderName))
                .Where(resourceName => resourceName.EndsWith(".png")).ToList();
            foreach (var asset in assets)
            {
                var splitName = asset.Split(".");
                var assetName = $"{splitName[splitName.Length - 2]}.{splitName[splitName.Length - 1]}";

                using (var stream = mapAssembly.GetManifestResourceStream(asset))
                {
                    var gameAsset = new ImageAsset()
                    {
                        ImageName = assetName,
                        ImageData = ReadStreamGetBytes(stream)
                    };
                    LoadedAssets.AddAsset(type, gameAsset);
                }
            }
        }

        public static void LoadServerAssets()
        {
            LoadAssetType(AssetType.ITEMS, "items");
            LoadAssetType(AssetType.ANIMATION, "animations");
            LoadAssetType(AssetType.SPRITE, "characters");
            LoadAssetType(AssetType.TILESET, "tilesets");
        }


        public static void Clear()
        {
            LoadedAssets.Clear();
        }

        private static byte[] ReadStreamGetBytes(Stream input)
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
            if (searchMapName != null)
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

        private static XDocument GetXDocument(string name)
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
