using UnityEngine;

public class ClientDialogsBehaviour : MonoBehaviour {

    public GameObject RootNode;

    private static GameObject _rootNode;

    private static Transform _openedDialog;

    public static void Open(string name)
    {
        var dialog = _rootNode.transform.Find(name);
        if(dialog != null)
        {
            dialog.gameObject.SetActive(true);
            _openedDialog = dialog;
        }
    }

    public static void Toggle(string name)
    {
        var dialog = _rootNode.transform.Find(name);
        if (dialog != null)
        {

            var isSameDialog = _openedDialog == dialog;

            if (_openedDialog != null)
            {
                _openedDialog.gameObject.SetActive(false);
                _openedDialog = null;
            }

            if (!isSameDialog)
            {
                dialog.gameObject.SetActive(true);
                _openedDialog = dialog;
            }


        }
    }

	// Use this for initialization
	void Start () {
        _rootNode = RootNode;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
