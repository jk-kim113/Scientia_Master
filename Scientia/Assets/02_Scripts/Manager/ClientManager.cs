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

    long _myUUID;

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

                        DefinedStructure.P_ResultLogIn pResultLogIn = new DefinedStructure.P_ResultLogIn();
                        pResultLogIn = (DefinedStructure.P_ResultLogIn)ConvertPacket.ByteArrayToStructure(pToClient._data, pResultLogIn.GetType(), pToClient._totalSize);

                        if(pResultLogIn._isSuccess == 0)
                        {
                            _myUUID = pResultLogIn._UUID;
                            GetMyCharacterInfo();
                            UIManager._instance.OpenWnd<SelectCharacterUI>(UIManager.eKindWindow.SelectCharacterUI).SetSortOrder(-1);
                        }
                        else
                        {
                            SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.LogIn_Fail);
                        }

                        break;

                    case DefinedProtocol.eToClient.ResultOverlap_ID:

                        DefinedStructure.P_ResultCheck pResultOverlap_ID = new DefinedStructure.P_ResultCheck();
                        pResultOverlap_ID = (DefinedStructure.P_ResultCheck)ConvertPacket.ByteArrayToStructure(pToClient._data, pResultOverlap_ID.GetType(), pToClient._totalSize);
                        
                        if(pResultOverlap_ID._result == 0)
                        {
                            SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.ID_Overlap);
                        }
                        else
                        {
                            EnrollUI enrollUI = UIManager._instance.GetWnd<EnrollUI>(UIManager.eKindWindow.EnrollUI);
                            enrollUI._IsCheckOverlap = true;
                        }

                        break;

                    case DefinedProtocol.eToClient.EnrollResult:

                        DefinedStructure.P_ResultCheck pResultEnroll = new DefinedStructure.P_ResultCheck();
                        pResultEnroll = (DefinedStructure.P_ResultCheck)ConvertPacket.ByteArrayToStructure(pToClient._data, pResultEnroll.GetType(), pToClient._totalSize);

                        if(pResultEnroll._result == 0)
                        {
                            UIManager._instance.Close(UIManager.eKindWindow.EnrollUI);
                        }
                        else
                        {
                            SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.Enroll_Fail);
                        }

                        break;

                    case DefinedProtocol.eToClient.CharacterInfo:

                        DefinedStructure.P_CharacterInfo pCharacterInfo = new DefinedStructure.P_CharacterInfo();
                        pCharacterInfo = (DefinedStructure.P_CharacterInfo)ConvertPacket.ByteArrayToStructure(pToClient._data, pCharacterInfo.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<SelectCharacterUI>(UIManager.eKindWindow.SelectCharacterUI).ShowCharacter(
                            pCharacterInfo._nickName, pCharacterInfo._chracIndex, pCharacterInfo._accountLevel, pCharacterInfo._slotIndex);

                        break;

                    case DefinedProtocol.eToClient.EndCharacterInfo:

                        UIManager._instance.Close(UIManager.eKindWindow.LogInUI);
                        UIManager._instance.GetWnd<SelectCharacterUI>(UIManager.eKindWindow.SelectCharacterUI).SetSortOrder(5);

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
        DefinedStructure.P_Send_ID_Pw pLogInTry;
        pLogInTry._id = id;
        pLogInTry._pw = pw;

        ToPacket(DefinedProtocol.eFromClient.LogInTry, pLogInTry);
    }

    public void OverlapCheck(string target)
    {
        DefinedStructure.P_OverlapCheck pOverlapCheck;
        pOverlapCheck._target = target;

        ToPacket(DefinedProtocol.eFromClient.OverlapCheck_ID, pOverlapCheck);
    }

    public void EnrollTry(string id, string pw)
    {
        DefinedStructure.P_Send_ID_Pw pEnrollTry;
        pEnrollTry._id = id;
        pEnrollTry._pw = pw;

        ToPacket(DefinedProtocol.eFromClient.EnrollTry, pEnrollTry);
    }

    void GetMyCharacterInfo()
    {
        DefinedStructure.P_Request pGetMyChatacInfo;

        ToPacket(DefinedProtocol.eFromClient.GetMyCharacterInfo, pGetMyChatacInfo);
    }

    void ToPacket(DefinedProtocol.eFromClient fromClientID, object str)
    {
        DefinedStructure.PacketInfo packetRecieve;
        packetRecieve._id = (int)fromClientID;
        packetRecieve._data = ConvertPacket.StructureToByteArray(str);
        packetRecieve._totalSize = packetRecieve._data.Length;

        _fromClientQueue.Enqueue(ConvertPacket.StructureToByteArray(packetRecieve));
    }
}
