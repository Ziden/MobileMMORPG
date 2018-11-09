using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    private GameObject MenuButton;

    private GameObject ChooseColorDialog;

    // Use this for initialization
    void Start () {
        MenuButton = GameObject.Find("MenuButton");
        ChooseColorDialog = GameObject.Find("ChooseColorDialog");

        MenuButton.GetComponent<Button>().onClick.AddListener(() =>
        {

        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
