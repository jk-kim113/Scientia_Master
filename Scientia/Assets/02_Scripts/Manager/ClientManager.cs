using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ClientManager : TSingleton<ClientManager>
{
    public struct ShowRoom
    {
        public int _roomNumber;
        public string _name;
        public int _isLock;
        public int _currentMemberCnt;
        public int _maxMemberCnt;
        public string _mode;
        public string _rule;
        public int _isPlay;
    }

    //const string _ip = "1.235.143.68";
    //const int _port = 100;

    const string _ip = "127.0.0.1";
    const int _port = 80;

    Socket _server;

    bool _isConnect = false;

    Queue<DefinedStructure.PacketInfo> _toClientQueue = new Queue<DefinedStructure.PacketInfo>();
    Queue<byte[]> _fromClientQueue = new Queue<byte[]>();

    string _currentNickName;
    public string _NowNickName { get { return _currentNickName; } set { _currentNickName = value; } }

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
                    #region LogIn / Character
                    case DefinedProtocol.eToClient.LogInResult:

                        DefinedStructure.P_ResultLogIn pResultLogIn = new DefinedStructure.P_ResultLogIn();
                        pResultLogIn = (DefinedStructure.P_ResultLogIn)ConvertPacket.ByteArrayToStructure(pToClient._data, pResultLogIn.GetType(), pToClient._totalSize);

                        if(pResultLogIn._isSuccess == 0)
                        {
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
                            SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.ID_Non_Overlap);
                        }

                        break;

                    case DefinedProtocol.eToClient.ResultOverlap_NickName:

                        DefinedStructure.P_ResultCheck pResultOverlap_NickName = new DefinedStructure.P_ResultCheck();
                        pResultOverlap_NickName = (DefinedStructure.P_ResultCheck)ConvertPacket.ByteArrayToStructure(pToClient._data, pResultOverlap_NickName.GetType(), pToClient._totalSize);

                        if(pResultOverlap_NickName._result == 0)
                        {
                            SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.NickName_Overlap);
                        }
                        else
                        {
                            CreateCharacterUI createCharacUI = UIManager._instance.GetWnd<CreateCharacterUI>(UIManager.eKindWindow.CreateCharacterUI);
                            createCharacUI.CreateCharacter();
                        }

                        break;

                    case DefinedProtocol.eToClient.EnrollResult:

                        DefinedStructure.P_ResultCheck pResultEnroll = new DefinedStructure.P_ResultCheck();
                        pResultEnroll = (DefinedStructure.P_ResultCheck)ConvertPacket.ByteArrayToStructure(pToClient._data, pResultEnroll.GetType(), pToClient._totalSize);

                        if(pResultEnroll._result == 0)
                        {
                            UIManager._instance.Close(UIManager.eKindWindow.EnrollUI);
                            SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.Enroll_Success);
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
                        UIManager._instance.Close(UIManager.eKindWindow.CreateCharacterUI);
                        UIManager._instance.GetWnd<SelectCharacterUI>(UIManager.eKindWindow.SelectCharacterUI).SetSortOrder(5);

                        break;

                    case DefinedProtocol.eToClient.EndCreateCharacter:

                        DefinedStructure.P_ResultCheck pResultCreateCharac = new DefinedStructure.P_ResultCheck();
                        pResultCreateCharac = (DefinedStructure.P_ResultCheck)ConvertPacket.ByteArrayToStructure(pToClient._data, pResultCreateCharac.GetType(), pToClient._totalSize);

                        if(pResultCreateCharac._result == 0)
                        {
                            GetMyCharacterInfo();
                        }
                        else
                        {
                            SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.Character_Fail);
                        }

                        break;
                    #endregion

                    case DefinedProtocol.eToClient.ShowMyInfo:
                        
                        DefinedStructure.P_MyInfoData pMyInfoData = new DefinedStructure.P_MyInfoData();
                        pMyInfoData = (DefinedStructure.P_MyInfoData)ConvertPacket.ByteArrayToStructure(pToClient._data, pMyInfoData.GetType(), pToClient._totalSize);

                        MyInfoUI myInfoUI = UIManager._instance.GetWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI);
                        while(!myInfoUI._IsReady)
                        {
                            yield return null;
                        }
                        myInfoUI.Unlock(pMyInfoData._cardReleaseArr);

                        LobbyManager._instance.LoadFinish();

                        break;

                    case DefinedProtocol.eToClient.CompleteAddReleaseCard:

                        CardInfoUI cardInfoUI = UIManager._instance.GetWnd<CardInfoUI>(UIManager.eKindWindow.CardInfoUI);
                        cardInfoUI.EndUnlock();

                        break;

                    case DefinedProtocol.eToClient.EnterRoom:

                        SceneControlManager._instance.StartLoadBattleScene();

                        DefinedStructure.P_UserInfo pUserInfo = new DefinedStructure.P_UserInfo();
                        pUserInfo = (DefinedStructure.P_UserInfo)ConvertPacket.ByteArrayToStructure(pToClient._data, pUserInfo.GetType(), pToClient._totalSize);

                        while(!UIManager._instance.isOpened(UIManager.eKindWindow.BattleUI))
                        {
                            yield return null;
                        }

                        BattleUI battleUI = UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI);
                        battleUI._RoomNumber = pUserInfo._roomNumber;

                        battleUI.ShowUserInfo(pUserInfo._index, pUserInfo._nickName, pUserInfo._accountLevel, pUserInfo._isReady == 0);

                        break;

                    case DefinedProtocol.eToClient.ShowRoomList:

                        DefinedStructure.P_RoomInfo pRoomInfo = new DefinedStructure.P_RoomInfo();
                        pRoomInfo = (DefinedStructure.P_RoomInfo)ConvertPacket.ByteArrayToStructure(pToClient._data, pRoomInfo.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<LobbyUI>(UIManager.eKindWindow.LobbyUI).ShowRoomInfo(
                            pRoomInfo._roomNumber,
                            pRoomInfo._name,
                            pRoomInfo._isLock == 0,
                            pRoomInfo._currentMemberCnt,
                            pRoomInfo._maxMemberCnt,
                            pRoomInfo._mode,
                            pRoomInfo._rule,
                            pRoomInfo._isPlay == 0?"게임중":"대기중");

                        break;

                    case DefinedProtocol.eToClient.ShowReady:

                        DefinedStructure.P_ShowReady pShowReady = new DefinedStructure.P_ShowReady();
                        pShowReady = (DefinedStructure.P_ShowReady)ConvertPacket.ByteArrayToStructure(pToClient._data, pShowReady.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowReadyUser(pShowReady._index, pShowReady._isReady == 0);

                        break;

                    case DefinedProtocol.eToClient.ShowMaster:

                        DefinedStructure.P_MasterInfo pMasterInfo = new DefinedStructure.P_MasterInfo();
                        pMasterInfo = (DefinedStructure.P_MasterInfo)ConvertPacket.ByteArrayToStructure(pToClient._data, pMasterInfo.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowMaster(pMasterInfo._masterName.CompareTo(_currentNickName) == 0);

                        break;

                    case DefinedProtocol.eToClient.FinishShowRoom:

                        LobbyManager._instance.LoadFinish();

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

    public void OverlapCheck_ID(string target)
    {
        DefinedStructure.P_OverlapCheck pOverlapCheck;
        pOverlapCheck._target = target;

        ToPacket(DefinedProtocol.eFromClient.OverlapCheck_ID, pOverlapCheck);
    }

    public void OverlapCheck_NickName(string target)
    {
        DefinedStructure.P_OverlapCheck pOverlapCheck;
        pOverlapCheck._target = target;

        ToPacket(DefinedProtocol.eFromClient.OverlapCheck_NickName, pOverlapCheck);
    }

    public void EnrollTry(string id, string pw)
    {
        DefinedStructure.P_Send_ID_Pw pEnrollTry;
        pEnrollTry._id = id;
        pEnrollTry._pw = pw;

        ToPacket(DefinedProtocol.eFromClient.EnrollTry, pEnrollTry);
    }

    public void CreateCharacter(string nickname, int characIndex, int slot)
    {
        DefinedStructure.P_CreateCharacter pCreateCharacter;
        pCreateCharacter._nickName = nickname;
        pCreateCharacter._characterIndex = characIndex;
        pCreateCharacter._slot = slot;

        ToPacket(DefinedProtocol.eFromClient.CreateCharacter, pCreateCharacter);
    }

    public void RequestMyInfoData()
    {
        DefinedStructure.P_GetMyInfoData pGetMyInfoData;
        pGetMyInfoData._nickName = _currentNickName;

        ToPacket(DefinedProtocol.eFromClient.GetMyInfoData, pGetMyInfoData);
    }

    public void MyCardRelease(int cardIndex)
    {
        DefinedStructure.P_ReleaseCard pReleaseCard;
        pReleaseCard._nickName = _currentNickName;
        pReleaseCard._cardIndex = cardIndex;

        ToPacket(DefinedProtocol.eFromClient.AddReleaseCard, pReleaseCard);
    }

    public void CreateRoom(string name, bool isLock, string pw, string mode, string rule)
    {
        DefinedStructure.P_CreateRoom pCreateRoom;
        pCreateRoom._nickNaame = _currentNickName;
        pCreateRoom._name = name;
        pCreateRoom._isLock = isLock ? 0 : 1;
        pCreateRoom._pw = pw;
        pCreateRoom._mode = mode;
        pCreateRoom._rule = rule;

        ToPacket(DefinedProtocol.eFromClient.CreateRoom, pCreateRoom);
    }

    public void GetRoomList()
    {
        DefinedStructure.P_Request pRequest;

        ToPacket(DefinedProtocol.eFromClient.GetRoomList, pRequest);
    }

    public void EnterRoom(int roomNum)
    {
        DefinedStructure.P_TryEnterRoom pTryEnterRoom;
        pTryEnterRoom._nickName = _currentNickName;
        pTryEnterRoom._roomNumber = roomNum;

        ToPacket(DefinedProtocol.eFromClient.TryEnterRoom, pTryEnterRoom);
    }

    public void InformReady(int roomNum)
    {
        DefinedStructure.P_InformRoomInfo pInformReady;
        pInformReady._roomNumber = roomNum;

        ToPacket(DefinedProtocol.eFromClient.InformReady, pInformReady);
    }

    public void InformGameStart(int roomNum)
    {
        DefinedStructure.P_InformRoomInfo pInformGameStart;
        pInformGameStart._roomNumber = roomNum;

        ToPacket(DefinedProtocol.eFromClient.InformGameStart, pInformGameStart);
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
        packetRecieve._data = new byte[1024];
        byte[] temp = ConvertPacket.StructureToByteArray(str);
        for (int n = 0; n < temp.Length; n++)
            packetRecieve._data[n] = temp[n];
        packetRecieve._totalSize = temp.Length;
        
        _fromClientQueue.Enqueue(ConvertPacket.StructureToByteArray(packetRecieve));
    }

    //private void OnApplicationQuit()
    //{
    //    if(_server != null)
    //    {
    //        DefinedStructure.P_Request pConnectionTerminate;

    //        ToPacket(DefinedProtocol.eFromClient.ConnectionTerminate, pConnectionTerminate);

    //        //_server.Shutdown(SocketShutdown.Both);
    //        _server.Disconnect(true);
    //        _server.Close();
    //        _server = null;
    //    }
    //}
}
