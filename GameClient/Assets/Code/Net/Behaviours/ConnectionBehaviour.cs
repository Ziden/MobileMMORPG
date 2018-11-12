using Assets.Code.Game;
using Client.Net;
using System;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionBehaviour : MonoBehaviour
{
    public static ConnectionBehaviour connector;

    public static bool CloseOnQuit = true;

    public Image LoadingRotatingImage;
    public GameObject LoadingPanel;

    private string _message;
    private Thread _connectingThread;
    private Vector3 _rotationEuler;

    private State ConnectorState;

    private enum State
    {
        CONNECTED = 1,
        CONNECTING = 2,
        IDDLE = 3,
        LISTENING = 4,
        ERROR = 5
    }

    public void Update()
    {
        if(ConnectorState==State.CONNECTED)
        {
            UnityClient.TcpClient.StartListening();
            ConnectorState = State.LISTENING;
            if(LoadingPanel != null)
                LoadingPanel.SetActive(false);
        } else if(ConnectorState == State.ERROR && LoadingPanel.activeSelf)
        {
            LoadingPanel.SetActive(false);
            GenericDialog.Get().Show("Error",_message, Connect);
        } else if(ConnectorState == State.CONNECTING)
        {
            _rotationEuler += Vector3.forward * 90 * Time.deltaTime;
            if(LoadingRotatingImage != null)
                LoadingRotatingImage.transform.rotation = Quaternion.Euler(_rotationEuler);
        } else if(ConnectorState == State.LISTENING)
        {
            if(UnityClient.TcpClient == null || !UnityClient.TcpClient.IsListening())
            {
                Debug.Log("RECONNECTING");
                Connect();
            }
        }
    }

    public bool Connect()
    {
        if(LoadingPanel != null)
            LoadingPanel.SetActive(true);

        _connectingThread = new Thread(new ThreadStart(ClientConnectAndListen));
        _connectingThread.Start();
        ConnectorState = State.CONNECTING;
        return true;
    }

    public void Start()
    {
        CloseOnQuit = true;
        Debug.Log("START CONNECT");
        Connect();
    }

    private void OnDestroy()
    {
        if(CloseOnQuit && UnityClient.TcpClient != null)
        {
            Debug.Log("Closing Socket");
            UnityClient.TcpClient.Stop();
        }
    }

    public void ClientConnectAndListen()
    {
        try
        {
            Debug.Log("Creating TCP Client");
            ConnectorState = State.CONNECTING;
            UnityClient.TcpClient = new ClientTcpHandler("localhost", 8888);
            ConnectorState = State.CONNECTED;
            Debug.Log("TCP Client Created");
        }
        catch (SocketException e)
        {
            _message = "Error connecting to the server";
            if (e.ErrorCode == (int)SocketError.ConnectionRefused)
            {
                _message = "Server is Offline :(";
            }
            ConnectorState = State.ERROR;
        }
    }
}
