using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericDialog : MonoBehaviour
{
    public Text Title;
    public Text Msg;
    public Button OkBtn;
    public GameObject DialogCanvas;

    private static GenericDialog _instance;

    void Start()
    {
        DialogCanvas.SetActive(false);
        _instance = this;
    }

    public static GenericDialog Get()
    {
        return _instance;
    }

    public void Close()
    {
        DialogCanvas.SetActive(false);
    }

    public void Show(string title, string msg, Func<bool> callback = null)
    {
        Msg.text = msg;
        Title.text = title;
        transform.SetAsLastSibling();

        OkBtn.onClick.RemoveAllListeners();
        if (callback == null)
        {
            OkBtn.onClick.AddListener(() =>
            {
                Close();
            });
        }
        else
        {
            OkBtn.onClick.AddListener(() =>
            {
                Close();
                callback.Invoke();
            });
        }

        DialogCanvas.SetActive(true);
    }
}
