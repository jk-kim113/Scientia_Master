using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ClientManager : TSingleton<ClientManager>
{
    //const string _ip = "1.235.143.68";
    //const int _port = 100;

    const string _ip = "127.0.0.1";
    const int _port = 80;

    Socket _server;

    bool _isConnect = false;

    Queue<DefinedStructure.PacketInfo> _toClientQueue = new Queue<DefinedStructure.PacketInfo>();
    Queue<byte[]> _fromClientQueue = new Queue<byte[]>();

    protected override void Init()
    {
        base.Init();

        StartCoroutine(AddOrder());
        StartCoroutine(DoOrder());
        StartCoroutine(SendOrder());
    }

    public void ConnectServer()
    {
        _isConnect = Connect(_ip, _port);
    }

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

    IEnumerator AddOrder()
    {
        while (true)
        {
            if (_isConnect && _server != null && _server.Poll(0, SelectMode.SelectRead))
            {
                byte[] buffer = new byte[1032];
                int recvLen = _server.Receive(buffer);
                if (recvLen > 0)
                {
                    DefinedStructure.PacketInfo pToClient = new DefinedStructure.PacketInfo();
                    pToClient = (DefinedStructure.PacketInfo)ConvertPacket.ByteArrayToStructure(buffer, pToClient.GetType(), recvLen);

                    _toClientQueue.Enqueue(pToClient);
                }
            }

            yield return null;
        }
    }

    IEnumerator DoOrder()
    {
        while (true)
        {
            if (_toClientQueue.Count != 0)
            {
                DefinedStructure.PacketInfo pToClient = _toClientQueue.Dequeue();

                switch ((DefinedProtocol.eToClient)pToClient._id)
                {
                    case DefinedProtocol.eToClient.LogInResult:

                        break;
                }
            }

            yield return null;
        }
    }

    IEnumerator SendOrder()
    {
        while (true)
        {
            if (_fromClientQueue.Count != 0)
                _server.Send(_fromClientQueue.Dequeue());

            yield return null;
        }
    }

    public void LogIn(string id, string pw)
    {
        DefinedStructure.P_LogInTry pLogInTry;
        pLogInTry._id = id;
        pLogInTry._pw = pw;

        ToPacket(DefinedProtocol.eFromClient.LogInTry, pLogInTry);
    }

    void ToPacket(DefinedProtocol.eFromClient fromClientID, object str)
    {
        DefinedStructure.PacketInfo packetRecieve1;
        packetRecieve1._id = (int)fromClientID;
        packetRecieve1._data = ConvertPacket.StructureToByteArray(str);
        packetRecieve1._totalSize = packetRecieve1._data.Length;

        _fromClientQueue.Enqueue(ConvertPacket.StructureToByteArray(packetRecieve1));
    }
}
