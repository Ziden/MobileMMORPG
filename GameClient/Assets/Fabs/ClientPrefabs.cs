using UnityEngine;

public class ClientPrefabs : MonoBehaviour {

    public GameObject TextPrefab;
    public GameObject HealthbarPrefab;

    private static ClientPrefabs instance;

	// Use this for initialization
	void Start () {
        instance = this;
	}

    public static ClientPrefabs Get()
    {
        return instance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
