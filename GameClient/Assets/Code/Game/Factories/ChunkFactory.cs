using MapHandler;
using System.Collections.Generic;
using UnityEngine;

public class ChunkFactory : MonoBehaviour
{
    private byte[,] tileMap;
    private Mesh mesh;
    private int tileCount = 0;

    [SerializeField]
    private float tileSize = 32.0f;

    [SerializeField]
    private float tileScale = 1.0f;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvCoordinates = new List<Vector2>();

    [SerializeField]
    private int textureTileWidth = 1;

    [SerializeField]
    private int textureTileHeight = 1;

    [SerializeField]
    private int order = 0;

    void Awake()
    {
        this.mesh = this.GetComponent<MeshFilter>().mesh;

        // Scale to match desired unit to pixel ratio.

        float scale = this.tileSize * this.tileScale;

        this.transform.localScale = new Vector3(scale, scale, 1.0f);
    }

    void Start()
    {
        this.GenerateTileMap();
        this.UpdateModel();
    }

    void Update()
    {
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //    Vector3 localPos = this.transform.InverseTransformPoint(mousePos);

        //    int tx = Mathf.FloorToInt(Mathf.Abs(localPos.x));
        //    int ty = Mathf.FloorToInt(Mathf.Abs(localPos.y));

        //    this.tileMap[tx, ty] = (byte)3;

        //    this.UpdateModel();

        //}
    }

    private void UpdateModel()
    {
        this.GenerateMesh();
    }

    //    private void GenerateTileMap()
    //{
    //    this.tileMap = new byte[99, 99];

    //    for (int ty = 0; ty < this.tileMap.GetLength(1); ty++)
    //    {
    //        for (int tx = 0; tx < this.tileMap.GetLength(0); tx++) { int tileIndex = tx + (ty * this.tileMap.GetLength(0)); bool isEven = (tileIndex % 2 == 0); if (this.order > 0)
    //            {
    //                this.tileMap[tx, ty] = (byte)((isEven) ? 255 : 2);
    //            }
    //            else
    //            {
    //                this.tileMap[tx, ty] = (byte)1;
    //            }
    //        }
    //    }
    //}

    private void GenerateTileMap()
    {
        this.tileMap = new byte[99, 99];

        for (int ty = 0; ty < this.tileMap.GetLength(1); ty++)
        {
            for (int tx = 0; tx < this.tileMap.GetLength(0); tx++)
            {
                int tileIndex = tx + (ty * this.tileMap.GetLength(0)); bool isEven = (tileIndex % 2 == 0); if (this.order > 0)
                {
                    this.tileMap[tx, ty] = (byte)((isEven) ? 255 : 2);
                }
                else
                {
                    this.tileMap[tx, ty] = (byte)1;
                }
            }
        }
    }

    private void GenerateMesh()
    {
        //for (int ty = 0; ty < this.tileMap.GetLength(1); ty++)
        //{
        //    for (int tx = 0; tx < this.tileMap.GetLength(0); tx++)
        //    {
        //        this.ConstructTile(tx, -ty, tileMap[tx, ty]);
        //    }
        //}

        for (int ty = 0; ty < this.tileMap.GetLength(1); ty++)
        {
            for (int tx = 0; tx < this.tileMap.GetLength(0); tx++)
            {
                int tileValue = (int)tileMap[tx, ty];

                if (tileValue != 255)
                {
                    this.ConstructTile(tx, -ty, tileValue);
                }
            }
        }

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

    private float tUnit = 0f;
    
    private Vector2 tTileSprite = new Vector2(0, 0);

    private void ConstructTile(int tx, int ty, int texture)
    {
        Vector3 v0 = new Vector3(tx, ty, 0);
        Vector3 v1 = new Vector3(tx + 1, ty, 0);
        Vector3 v2 = new Vector3(tx + 1, ty - 1, 0);
        Vector3 v3 = new Vector3(tx, ty - 1, 0);

        vertices.Add(v0);
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);

        int tileIndex = tx + (Mathf.Abs(ty) * this.tileMap.GetLength(0));

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

        float textureX = texture % this.textureTileWidth;
        float textureY = Mathf.Floor(texture / this.textureTileHeight);

        float uPerPixel = 1.0f / ((this.tileSize + 4.0f) * (float)this.textureTileWidth);
        float vPerPixel = 1.0f / ((this.tileSize + 4.0f) * (float)this.textureTileHeight);

        float uPerTile = this.tileSize * uPerPixel;
        float uPerTilePadded = (this.tileSize + 4.0f) * uPerPixel;

        float vPerTile = this.tileSize * vPerPixel;
        float vPerTilePadded = (this.tileSize + 4.0f) * vPerPixel;

        float left = (textureX * uPerTilePadded) + (2.0f * uPerPixel);
        float right = left + uPerTile;
        float top = (textureY * vPerTilePadded) + (2.0f * vPerPixel);
        float bottom = top + vPerTile;

        //uvCoordinates.Add(new Vector2(left, bottom));
        //uvCoordinates.Add(new Vector2(right, bottom));
        //uvCoordinates.Add(new Vector2(right, top));
        //uvCoordinates.Add(new Vector2(left, top));

        uvCoordinates.Add(new Vector2(tUnit * tTileSprite.x, tUnit * tTileSprite.y + tUnit));
        uvCoordinates.Add(new Vector2(tUnit * tTileSprite.x + tUnit, tUnit * tTileSprite.y + tUnit));
        uvCoordinates.Add(new Vector2(tUnit * tTileSprite.x + tUnit, tUnit * tTileSprite.y));
        uvCoordinates.Add(new Vector2(tUnit * tTileSprite.x, tUnit * tTileSprite.y));

        tileCount++;
    }

    //public Vector2 GetCoordinate(int index, int colums, int rows)
    //{
    //    for (int i = 0; i < rows; i++)
    //        if(index < (colums * i) + colums && index >= colums * i)
    //            return new Vector2(index = colums * i, i);

    //    return new Vector2(-1, -1);
    //}

    public Vector2 PixelSpaceToUVSpace(Vector2 xy, Texture2D t)
    {
        return new Vector2(xy.x / ((float)t.width), xy.y / ((float)t.height));
    }

    //public void SetTile(Position chunkCoord, Position tileCoord, Sprite sprite)
    //{
    //    Vector2[] uv = mesh.uv;
    //    Rect spriteRect = sprite.textureRect;

    //    int chunkIndex = SearchForChunk(chunkCoord);

    //    if (chunkIndex == -1)
    //    {
    //        Debug.LogError("Chunk not found");
    //        return;
    //    }

    //    int ui = chunkIndex * (16 * 16) + (tileCoord.Y * 16 + tileCoord.X);
    //    ui *= 4;

    //    uv[ui] = ToUV(new Vector2(spriteRect.xMin, spriteRect.yMax), sprite.texture);
    //    uv[ui + 1] = ToUV(new Vector2(spriteRect.xMin, spriteRect.yMin), sprite.texture);
    //    uv[ui + 2] = ToUV(new Vector2(spriteRect.xMax, spriteRect.yMin), sprite.texture);
    //    uv[ui + 3] = ToUV(new Vector2(spriteRect.xMax, spriteRect.yMax), sprite.texture);

    //    mesh.uv = uv;
    //}
}
