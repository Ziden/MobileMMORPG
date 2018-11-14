using CommonCode;
using MapHandler;
using ServerCore.Game.GameMap.MobSpawners;
using ServerCore.Utils;
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

        // LAYERS
        public static string LAYER_SPAWNERS = "Spawners";
        public static string LAYER_FLOOR = "FloorLayer";

        // SPAWNER PROPS
        public static string SPAWNER_PROPERTY_MONSTER = "monster";
        public static string SPAWNER_PROPERTY_QTD = "qtd";


        /*   
            BIG TODO: REWRITE THIS PARSER - MAKE IT MODULAR SO ITS EASY TO ADD NEW STUFF...
            ELSE THIS IS GONNA BE HUGE...
        */

        /// <summary>
        /// Parses a byte stream into a Map, reading from as a TILED .tmx in csv format 
        /// </summary>
        public static ServerMap Parse(Stream mapStream)
        {
            ServerMap map = new ServerMap();
            using (XmlReader reader = XmlReader.Create(mapStream))
            {
                var doc = XDocument.Load(reader);
                var xMap = doc.Element("map");

                // Map Tilesets
                foreach (var tileset in xMap.Elements("tileset"))
                {
                    var imgSource = tileset.Element("image").Attribute("source").Value;
                    var tilesetName = imgSource.Split('/').Last();
                    byte[] image = MapLoader.LoadImageData(tilesetName);
                    map.Tilesets.Add(tilesetName, image);
                }

                // Map Objects
                foreach (var xObjectGroup in xMap.Elements("objectgroup"))
                {
                    var layerType = xObjectGroup.Attribute("name").Value;
                    if (layerType == LAYER_SPAWNERS)
                    {
                        ParseSpawner(xObjectGroup, map);
                    }
                }

                // Map Tiles
                ParseTiles(xMap, map); 
            }
            return map;
        }

        private static void ParseSpawner(XElement xObjectGroup, ServerMap map)
        {
            foreach (var xObject in xObjectGroup.Elements("object"))
            {
                var x = Int32.Parse(xObject.Attribute("x").Value) / GameCfg.TILE_SIZE_PIXELS;
                var y = Int32.Parse(xObject.Attribute("y").Value) / GameCfg.TILE_SIZE_PIXELS;
                var w = Int32.Parse(xObject.Attribute("width").Value) / GameCfg.TILE_SIZE_PIXELS;
                var h = Int32.Parse(xObject.Attribute("height").Value) / GameCfg.TILE_SIZE_PIXELS;
                var maxX = x + w - 1;
                var maxY = y + h - 1;

                var spawnerName = xObject.Attribute("name")?.Value;

                var spawner = new MonsterSpawner()
                {
                    minX = x,
                    maxX = maxX,
                    minY = y,
                    maxY = maxY
                };

                var xProperties = xObject.Element("properties");

                var xMonster = xProperties.Descendants("property")
                    .FirstOrDefault(el => el.Attribute("name")?.Value == SPAWNER_PROPERTY_MONSTER);

                var xQtd = xProperties.Descendants("property")
                    .FirstOrDefault(el => el.Attribute("name")?.Value == SPAWNER_PROPERTY_QTD);

                var monsterType = xMonster.Attribute("value").Value.FirstCharToUpper();
                var monsterQtd = Int32.Parse(xQtd.Attribute("value").Value);

                var monsterClass = Type.GetType($"ServerCore.Game.Monsters.{monsterType}");

                if (monsterClass == null)
                {
                    throw new InvalidDataException($"Monster {monsterType} in spawner {spawnerName}");
                }

                spawner.SpawnerMobs.Add(new SpawnerMob()
                {
                    Amount = monsterQtd,
                    MonsterClassType = monsterClass
                });
                map.Spawners.Add(spawner);
            }
        }

        private static void ParseTiles(XElement xMap, ServerMap map)
        {
            foreach (var xLayer in xMap.Elements("layer"))
            {
                var layerType = xLayer.Attribute("name").Value;

                foreach (var xChunk in xLayer.Element("data").Elements("chunk"))
                {
                    var csvData = Regex.Replace((string)xChunk.Value, @"\t|\n|\r", "");
                    var chunkX = Int16.Parse(xChunk.Attribute("x").Value) / Chunk.SIZE;
                    var chunkY = Int16.Parse(xChunk.Attribute("y").Value) / Chunk.SIZE;

                    if (layerType == LAYER_FLOOR)
                    {
                        var chunk = new ServerChunk()
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
    }
}
