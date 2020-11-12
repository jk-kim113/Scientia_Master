using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ClientManager : MonoBehaviour
{
    const string _ip = "1.235.143.68";
    const int _port = 100;

    Socket _server;

    bool _isConnect = false;

    bool Connect(string ipAddress, int port)
    {
        try
        {
            if (!_isConnect)
            {
                // make socket
                _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _server.Connect(ipAddress, port);

                return true;
            }
        }
        catch (System.Exception ex)
        {
            // 메세지 창에 띄운다.
            Debug.Log(ex.Message);
        }

        return false;
    }

    public void ConnectServer()
    {
        _isConnect = Connect(_ip, _port);
    }

    private void Start()
    {
        ConnectServer();
    }
}
