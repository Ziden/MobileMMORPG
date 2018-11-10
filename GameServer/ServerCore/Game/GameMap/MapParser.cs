using MapHandler;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace ServerCore.Game.GameMap
{
    public class MapParser
    {
        public enum LayerType
        {
            TILES = 1,
            OBJECTS = 2,
            DYNAMIC = 3
        }

        private static LayerType GetLayer(string layerId)
        {
            return (LayerType)Enum.Parse(typeof(LayerType), layerId);
        }

        /// <summary>
        /// Parses a byte stream into a Map, reading from as a TILED .tmx in csv format
        /// </summary>
        public static WorldMap<ChunkType> Parse<ChunkType>(Stream mapStream) where ChunkType : Chunk , new()
        {
            WorldMap<ChunkType> map = new WorldMap<ChunkType>();
            using (XmlReader reader = XmlReader.Create(mapStream))
            {
                var doc = XDocument.Load(reader);
                var xMap = doc.Element("map");

                foreach(var tileset in xMap.Elements("tileset"))
                {
                    var imgSource = tileset.Element("image").Attribute("source").Value;
                    var tilesetName = imgSource.Split('/').Last();
                    byte[] image = MapLoader.LoadImageData(tilesetName);
                    map.Tilesets.Add(tilesetName, image);
                }

                foreach (var xLayer in xMap.Elements("layer"))
                {
                    var layerType = GetLayer(xLayer.Attribute("id").Value);

                    foreach (var xChunk in xLayer.Element("data").Elements("chunk"))
                    {
                        var csvData = Regex.Replace((string)xChunk.Value, @"\t|\n|\r", "");
                        var chunkX = Int16.Parse(xChunk.Attribute("x").Value) / Chunk.SIZE;
                        var chunkY = Int16.Parse(xChunk.Attribute("y").Value) / Chunk.SIZE;

                        if (layerType == LayerType.TILES)
                        {
                            var chunk = new ChunkType()
                            {
                                x = chunkX,
                                y = chunkY
                            };
                            var tileArray = csvData.Split(',');
                            for (int y = 0; y < Chunk.SIZE; y++)
                            {
                                for (int x = 0; x < Chunk.SIZE; x++)
                                {
                                    var tile = Int16.Parse(tileArray[x + y * Chunk.SIZE]);
                                    chunk.SetTile(x, y, tile);
                                }
                            }
                            map.AddChunk(chunk);
                        }
                    }
                }
            }
            return map;
        }
    }
}
