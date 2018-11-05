using Client.Net;
using System;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBehaviour : MonoBehaviour
{
    public static LoadingBehaviour Loading;

    public RawImage LoadingRotatingImage;
    public GameObject LoadingPanel;
    public Text Text;

    private string _message;
    private Vector3 _rotationEuler;

    public void StartLoading(string msg)
    {
        Text.text = msg;
        LoadingPanel.SetActive(true);
    }

    public void StopLoading()
    {
        LoadingPanel.SetActive(false);
    }

    public bool IsLoading()
    {
        return LoadingPanel != null;
    }

    public void Update() { 
        if(LoadingPanel != null)
            _rotationEuler += Vector3.forward * 90 * Time.deltaTime;
            if(LoadingRotatingImage != null)
                LoadingRotatingImage.transform.rotation = Quaternion.Euler(_rotationEuler);
      
    }
    
    public void Start()
    {
        LoadingBehaviour.Loading = this;
    }
}
