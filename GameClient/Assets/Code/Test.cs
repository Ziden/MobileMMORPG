using Assets.Code.Net;
using System.IO;
using UnityEngine;

public class Test : MonoBehaviour {

    public Material aa;
    public GameObject chunk;

    void Start()
    {

        // Create a new 2x2 texture ARGB32 (32 bit with alpha) and no mipmaps
        var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);

        // set the pixel values
        texture.SetPixel(0, 0, new Color(1.0f, 1.0f, 1.0f, 0.5f));
        texture.SetPixel(1, 0, Color.clear);
        texture.SetPixel(0, 1, Color.white);
        texture.SetPixel(1, 1, Color.black);

        // Apply all SetPixel calls
        texture.Apply();

        var tx = AssetHandler.LoadTexture(Path.Combine(Application.persistentDataPath, "Set1.png"));

        // connect texture to material of GameObject this script is attached to

        aa.mainTexture = tx;

        chunk.GetComponent<Renderer>().material = aa;
        chunk.transform.position = transform.position;

        Instantiate(chunk);
        
    }
}
