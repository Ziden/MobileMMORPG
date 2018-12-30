using CommonCode.Pathfinder;
using Pathfinder;
using System;
using System.Collections.Generic;
using Common.Entity;

namespace MapHandler
{
    public class WorldMap<ChunkType> where ChunkType : Chunk
    {
        public Dictionary<string, ChunkType> Chunks = new Dictionary<string, ChunkType>();

        public void AddChunk(ChunkType c)
        {
            Chunks.Add($"{c.x}_{c.y}", c);
        }

        public MapTile GetTile(int x, int y)
        {
            var chunk = GetChunkByTilePosition(x, y);
            if (chunk == null)
                return null;
            else
            {
                var chunkTileX = x - (chunk.x << 4);
                var chunkTileY = y - (chunk.y << 4);
                return chunk.Tiles[chunkTileX, chunkTileY];
            }
        }

        public void UpdateEntityPosition(Entity e, Position from, Position to)
        {
            if (from == null && to != null) // spawning
            {
                var spawnChunk = GetChunkByTilePosition(to.X, to.Y);
                spawnChunk.EntitiesInChunk[e.EntityType].Add(e);
                GetTile(to).Occupator = e;
            }
            else if (to == null && from != null) // despawning
            {
                var spawnChunk = GetChunkByTilePosition(from.X, from.Y);
                spawnChunk.EntitiesInChunk[e.EntityType].Remove(e);
                GetTile(from).Occupator = null;
            }
            else 
            {
                // moving
                var chunkFrom = GetChunkByTilePosition(from.X, from.Y);
                var chunkTo = GetChunkByTilePosition(to.X, to.Y);

                if (chunkFrom == null || chunkTo == null)
                {
                    throw new Exception("Chunk the entity came from is not stored +"+from.ToString()+" TO: "+to.ToString());
                } 
 
                if (chunkFrom != chunkTo)
                {
                    chunkFrom.EntitiesInChunk[e.EntityType].Remove(e);
                    chunkTo.EntitiesInChunk[e.EntityType].Add(e);
                }

                var oldTile = GetTile(from);
                var newTile = GetTile(to);
                oldTile.Occupator = null;
                newTile.Occupator = e;
            }
        }

        public MapTile GetTile(Position p)
        {
            return GetTile(p.X, p.Y);
        }

        public bool IsPassable(int x, int y)
        {
            var tile = GetTile(x, y);
            return (
                tile != null &&
                TileProperties.IsPassable(tile.TileId) &&
                tile.Occupator == null
            );
        }

        public ChunkType GetChunkByTilePosition(int x, int y)
        {
            var chunkX = x >> 4;
            var chunkY = y >> 4;
            return GetChunkByChunkPosition(chunkX, chunkY);
        }

        public ChunkType GetChunkByChunkPosition(int chunkX, int chunkY)
        {
            var key = $"{chunkX}_{chunkY}";
            if (!Chunks.ContainsKey(key))
                return null;
            return Chunks[key];
        }

        public List<Position> FindPath(Position start, Position goal)
        {
            var passableMapResult = this.GetPassableByteArray(start, goal);

            // Make the goal as passable to make sure we try to reach it if theres a path
            if (!IsPassable(goal.X, goal.Y) && start.GetDistance(goal) > 1)
            {
                passableMapResult
                    .PassableMap[
                    goal.X + passableMapResult.OffsetX * 16,
                    goal.Y + passableMapResult.OffsetY * 16
                    ] = 1;
            }

            var pathfinder = new PathFinder(passableMapResult.PassableMap);

            var offsetedStart = new Position(start.X + (passableMapResult.OffsetX * 16), start.Y + (passableMapResult.OffsetY * 16));
            var offsetedGoal = new Position(goal.X + (passableMapResult.OffsetX * 16), goal.Y + (passableMapResult.OffsetY * 16));

            var result = pathfinder.FindPath(offsetedStart, offsetedGoal);
            if (result == null)
            {
                return null;
            }

            List<Position> returned = new List<Position>();

            foreach (var node in result)
            {
                returned.Add(new Position(node.X - (passableMapResult.OffsetX * 16), node.Y - (passableMapResult.OffsetY * 16)));
            }

            return returned;
        }
    }
}
