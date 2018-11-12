using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {

    public Button Settings;

	// Use this for initialization
	void Start () {
        Settings.onClick.AddListener(() =>
        {
            // Testing
            ClientDialogsBehaviour.Toggle("ColorSelect");

        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
