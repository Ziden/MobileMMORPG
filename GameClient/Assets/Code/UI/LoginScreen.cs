using Client.Net;
using Common.Networking.Packets;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour {

    public GameObject LoginCanvas;
    public Button LoginButton;
    public Text Username;
    public Text Password;

    private static GameObject Canvas;

    public static void Kill()
    {
        Destroy(Canvas);
    }

    void Start() {
        LoginButton.onClick.AddListener(OnLoginClick);
        Canvas = LoginCanvas;
    }

    public void OnLoginClick()
    {
       // Login("admin", "wololo");
        Login(Username.text, Password.text);
    }

    private void Login(string username, string password)
    {
        var client = UnityClient.TcpClient;
        client.Send(new LoginPacket()
        {
            Login = username,
            Password = password
        });
    }

}
