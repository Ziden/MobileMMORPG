using Assets.Code.Game.ClientMap;
using Assets.Code.Net;
using Client.Net;
using CommonCode.EntityShared;
using MapHandler;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChunkFactory : MonoBehaviour
{
    private Mesh mesh;
    private int tileCount = 0;
    private float tUnit = 0.25f;
    
    private Vector2 tile;

    [SerializeField]
    private float tileSize = 16.0f;

    [SerializeField]
    private float tileScale = 1.0f;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvCoordinates = new List<Vector2>();

    [SerializeField]
    private int order = 0;

    private int chunkX;
    private int chunkY;
    private short[,] data;

    void Awake()
    {
        this.mesh = this.GetComponent<MeshFilter>().mesh;
        float scale = this.tileSize * this.tileScale;
        this.transform.localScale = new Vector3(scale, scale, 1.0f);
    }
    
    public void StartChunk(int chunkX, int chunkY, short[,] data)
    {
        this.chunkX = chunkX;
        this.chunkY = chunkY;
        this.data = data;
        this.GenerateMesh();
    }
    
    private void GenerateMesh()
    {
        ClientChunk c = new ClientChunk()
        {
            x = chunkX,
            y = chunkY,
            GameObject = gameObject
        };

        var asset = AssetHandler.LoadedAssets[DefaultAssets.TLE_SET1];
        
        var flippedMatrix = HorizontalFlip(asset.Matrix);

        var tilesW = asset.Sprite.GetLength(0);
        var tilesH = asset.Sprite.GetLength(1);

        Debug.Log("Rendering Chunk " + chunkX + " " + chunkY);
        for (var x = 0; x < 16; x++)
        {
            for (var y = 0; y < 16; y++)
            {
                var tileId = data[x, y];

                var tilePosition = new Position(chunkX * 16 + x, chunkY * 16 + y);

                c.Tiles[x, y] = new MapTile(tilePosition, tileId);
                
                if (tileId == 0)
                    return;
                
                tileId--;
                
                int tileIndexY = (int)Math.Floor((double)tileId / tilesW);
                var tileIndexX = tileId % tilesW;

                var tileCoordinate = GetCoordinate(tileId, tilesW, tilesH);

                for (int xx = 0; xx < tilesW; xx++)
                {
                    for (int yy = 0; yy < tilesH; yy++)
                    {
                        if (flippedMatrix[yy, xx] == tileCoordinate)
                        {
                            if(yy == 0)
                                tile = new Vector2(0, 3);
                            else
                                tile = new Vector2(xx, yy);
                            break;
                        }
                    }
                }
                
                this.ConstructTile(x, -y);
            }
        }
        
        UnityClient.Map.AddChunk(c);

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.uv = uvCoordinates.ToArray();

        mesh.RecalculateNormals();

        vertices.Clear();
        triangles.Clear();
        uvCoordinates.Clear();

        tileCount = 0;
    }

    private void ConstructTile(int tx, int ty)
    {
        Vector3 v0 = new Vector3(tx, ty, 0);
        Vector3 v1 = new Vector3(tx + 1, ty, 0);
        Vector3 v2 = new Vector3(tx + 1, ty - 1, 0);
        Vector3 v3 = new Vector3(tx, ty - 1, 0);

        vertices.Add(v0);
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);

        int tileIndex = tx + (Mathf.Abs(ty) * 16);

        int i0 = (tileCount * 4);
        int i1 = (tileCount * 4) + 1;
        int i2 = (tileCount * 4) + 2;
        int i3 = (tileCount * 4) + 3;

        triangles.Add(i0);
        triangles.Add(i1);
        triangles.Add(i3);

        triangles.Add(i1);
        triangles.Add(i2);
        triangles.Add(i3);

        uvCoordinates.Add(new Vector2(tUnit * tile.x, tUnit * tile.y + tUnit));
        uvCoordinates.Add(new Vector2(tUnit * tile.x + tUnit, tUnit * tile.y + tUnit));
        uvCoordinates.Add(new Vector2(tUnit * tile.x + tUnit, tUnit * tile.y));
        uvCoordinates.Add(new Vector2(tUnit * tile.x, tUnit * tile.y));

        tileCount++;
    }

    public static float GetCoordinate(int index, int colums, int rows)
    {
        for (int i = 0; i < rows; i++)
            if (index < (colums * i) + colums && index >= colums * i)
                return float.Parse($"{i}.{index - colums * i}");

        return 0;
    }

    public static T[,] HorizontalFlip<T>(T[,] source)
    {
        for (var i = 0; i < source.GetLength(1); i++)
        {
            int t = 0, b = source.GetLength(0) - 1;
            while (t < b)
            {
                var x = source[t, i];
                source[t, i] = source[b, i];
                source[b, i] = x;
                t++; b--;
            }
        }

        return source;
    }
}
