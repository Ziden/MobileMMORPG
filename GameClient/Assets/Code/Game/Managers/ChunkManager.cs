using Assets.Code.Game;
using Assets.Code.Net;
using Client.Net;
using Common.Networking.Packets;
using CommonCode.EntityShared;
using MapHandler;
using System.IO;
using System.Linq;
using UnityEngine;

public class ChunkManager : MonoBehaviour {

    public Material material;
    public GameObject chunkPrefab;

    private static ChunkManager _instance;

    public static ChunkManager Instance() => _instance;
    
    void Start()
    {
        _instance = this;

        var texture = AssetHandler.LoadTexture(Path.Combine(Application.persistentDataPath, DefaultAssets.TLE_SET1));

        texture.wrapMode = TextureWrapMode.Repeat;
        texture.filterMode = FilterMode.Point;

        material.mainTexture = texture;

        chunkPrefab.GetComponent<Renderer>().material = material;
        chunkPrefab.transform.position = transform.position;
    }

    public void LoadChunk(ChunkPacket packet)
    {
        var chunkX = packet.X;
        var chunkY = packet.Y;
        var data = packet.ChunkData;

        if (!UnityClient.Map.Chunks.Any(cp => cp.Key.Equals($"{chunkX}_{chunkY}")))
        {
            var chunkParent = chunkPrefab;
            chunkParent.name = $"chunk_{chunkX}_{chunkY}";
            
            var bb = Instantiate(chunkParent);
            bb.GetComponent<ChunkFactory>().StartChunk(chunkX, chunkY, data);
            
            var tileId = data[0, 0];
            var tilePosition = new Position(chunkX * 16, chunkY * 16);
            bb.transform.position = tilePosition.ToUnityPosition();
        }
    }
}
