using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ClientManager : TSingleton<ClientManager>
{
    const string _ip = "1.235.143.68";
    const int _port = 100;

    //const string _ip = "127.0.0.1";
    //const int _port = 80;

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
                    try
                    {
                        DefinedStructure.PacketInfo pToClient = new DefinedStructure.PacketInfo();
                        pToClient = (DefinedStructure.PacketInfo)ConvertPacket.ByteArrayToStructure(buffer, pToClient.GetType(), recvLen);

                        _toClientQueue.Enqueue(pToClient);
                    }
                    catch(NullReferenceException ex)
                    {
                        Debug.LogWarning(ex.Message);
                        Debug.LogWarning(ex.StackTrace);
                    }
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

                    #region Card
                    case DefinedProtocol.eToClient.ShowMyInfo:
                        
                        DefinedStructure.P_MyInfoData pMyInfoData = new DefinedStructure.P_MyInfoData();
                        pMyInfoData = (DefinedStructure.P_MyInfoData)ConvertPacket.ByteArrayToStructure(pToClient._data, pMyInfoData.GetType(), pToClient._totalSize);

                        MyInfoUI myInfoUI = UIManager._instance.GetWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI);
                        while(!myInfoUI._IsReady)
                        {
                            yield return null;
                        }
                        myInfoUI.InitMyInfo(pMyInfoData._characIndex, pMyInfoData._levelArr, pMyInfoData._expArr);
                        myInfoUI.Unlock(pMyInfoData._cardReleaseArr);

                        LobbyManager._instance.LoadFinish();

                        break;

                    case DefinedProtocol.eToClient.CompleteAddReleaseCard:

                        MyCardUI myCardUI = UIManager._instance.GetWnd<MyCardUI>(UIManager.eKindWindow.MyCardUI);
                        myCardUI.EndUnlock();

                        break;
                    #endregion

                    #region Room
                    case DefinedProtocol.eToClient.EnterRoom:

                        if(SceneControlManager._instance._nowScene != SceneControlManager.eTypeScene.Battle)
                            SceneControlManager._instance.StartLoadBattleScene();

                        DefinedStructure.P_UserInfo pUserInfo = new DefinedStructure.P_UserInfo();
                        pUserInfo = (DefinedStructure.P_UserInfo)ConvertPacket.ByteArrayToStructure(pToClient._data, pUserInfo.GetType(), pToClient._totalSize);

                        while(!UIManager._instance.isOpened(UIManager.eKindWindow.BattleUI))
                        {
                            yield return null;
                        }

                        BattleManager._instance._RoomNumber = pUserInfo._roomNumber;

                        BattleUI battleUI = UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI);
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

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowMaster(pMasterInfo._masterIndex);

                        break;

                    case DefinedProtocol.eToClient.FinishShowRoom:

                        LobbyManager._instance.LoadFinish();

                        break;
                    #endregion

                    #region Game Ready
                    case DefinedProtocol.eToClient.CannotPlay:

                        SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.Error_GameStart);

                        break;

                    case DefinedProtocol.eToClient.GameStart:

                        DefinedStructure.P_GameStart pGameStart = new DefinedStructure.P_GameStart();
                        pGameStart = (DefinedStructure.P_GameStart)ConvertPacket.ByteArrayToStructure(pToClient._data, pGameStart.GetType(), pToClient._totalSize);

                        BattleManager._instance.ReadyStateChange(BattleManager.eReadyState.GameStart);
                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowTotalSkillCubeCount(pGameStart._skillcubeCnt);
                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowTotalFlaskCubeCount(pGameStart._flaskcubeCnt);

                        break;

                    case DefinedProtocol.eToClient.ShowPickedCard:

                        DefinedStructure.P_PickedCard pPickedCard = new DefinedStructure.P_PickedCard();
                        pPickedCard = (DefinedStructure.P_PickedCard)ConvertPacket.ByteArrayToStructure(pToClient._data, pPickedCard.GetType(), pToClient._totalSize);

                        BattleManager._instance.ReadyStateChange(BattleManager.eReadyState.ReadCard);
                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowPickedCard(pPickedCard._pickedCardArr);

                        break;

                    case DefinedProtocol.eToClient.PickCard:

                        DefinedStructure.P_ThisTurn pPickCard = new DefinedStructure.P_ThisTurn();
                        pPickCard = (DefinedStructure.P_ThisTurn)ConvertPacket.ByteArrayToStructure(pToClient._data, pPickCard.GetType(), pToClient._totalSize);

                        BattleManager._instance.ReadyStateChange(BattleManager.eReadyState.PickCard);
                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).PickCardTurn(pPickCard._index);

                        break;

                    case DefinedProtocol.eToClient.ShowPickCard:

                        DefinedStructure.P_ShowPickCard pShowPickCard = new DefinedStructure.P_ShowPickCard();
                        pShowPickCard = (DefinedStructure.P_ShowPickCard)ConvertPacket.ByteArrayToStructure(pToClient._data, pShowPickCard.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowAddCard(pShowPickCard._index, pShowPickCard._slotIndex, pShowPickCard._cardIndex);

                        break;
                    #endregion

                    #region Game Progress
                    case DefinedProtocol.eToClient.ChooseAction:

                        DefinedStructure.P_ThisTurn pChooseAction = new DefinedStructure.P_ThisTurn();
                        pChooseAction = (DefinedStructure.P_ThisTurn)ConvertPacket.ByteArrayToStructure(pToClient._data, pChooseAction.GetType(), pToClient._totalSize);

                        BattleManager._instance._nowBattleState = BattleManager.eBattleState.Progress;
                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ChooseAction(pChooseAction._index);

                        break;

                    case DefinedProtocol.eToClient.GetCard:

                        DefinedStructure.P_GetCard pGetCard = new DefinedStructure.P_GetCard();
                        pGetCard = (DefinedStructure.P_GetCard)ConvertPacket.ByteArrayToStructure(pToClient._data, pGetCard.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).GetCardState(pGetCard._index);

                        break;

                    case DefinedProtocol.eToClient.ShowProjectBoard:

                        DefinedStructure.P_ShowProjectBoard pShowProjectBoard = new DefinedStructure.P_ShowProjectBoard();
                        pShowProjectBoard = (DefinedStructure.P_ShowProjectBoard)ConvertPacket.ByteArrayToStructure(pToClient._data, pShowProjectBoard.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).RenewProjectBoard(pShowProjectBoard._cardIndex, pShowProjectBoard._cardCount);

                        break;

                    case DefinedProtocol.eToClient.RotateCard:

                        DefinedStructure.P_InformRotateCard pInformRotateCard = new DefinedStructure.P_InformRotateCard();
                        pInformRotateCard = (DefinedStructure.P_InformRotateCard)ConvertPacket.ByteArrayToStructure(pToClient._data, pInformRotateCard.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).RotateCardState(
                            pInformRotateCard._index, pInformRotateCard._cardArr,
                            pInformRotateCard._cardRotateInfo, pInformRotateCard._turnCount);

                        break;

                    case DefinedProtocol.eToClient.ShowRotateInfo:

                        DefinedStructure.P_ShowRotateInfo pShowRotateInfo = new DefinedStructure.P_ShowRotateInfo();
                        pShowRotateInfo = (DefinedStructure.P_ShowRotateInfo)ConvertPacket.ByteArrayToStructure(pToClient._data, pShowRotateInfo.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowRotate(pShowRotateInfo._index, pShowRotateInfo._rotateValue, pShowRotateInfo._restCount);

                        break;

                    case DefinedProtocol.eToClient.SelectCompleteCard:

                        DefinedStructure.P_SelectCompleteCard pSelectCompleteCard = new DefinedStructure.P_SelectCompleteCard();
                        pSelectCompleteCard = (DefinedStructure.P_SelectCompleteCard)ConvertPacket.ByteArrayToStructure(pToClient._data, pSelectCompleteCard.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowCompleteCard(pSelectCompleteCard._index, pSelectCompleteCard._cardArr);

                        break;

                    case DefinedProtocol.eToClient.ShowTotalFlask:

                        DefinedStructure.P_ShowTotalFlask pShowTotalFlask = new DefinedStructure.P_ShowTotalFlask();
                        pShowTotalFlask = (DefinedStructure.P_ShowTotalFlask)ConvertPacket.ByteArrayToStructure(pToClient._data, pShowTotalFlask.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowTotalFlaskCubeCount(pShowTotalFlask._totalFlask);

                        break;

                    case DefinedProtocol.eToClient.ShowUserFlask:

                        DefinedStructure.P_ShowUserFlask pShowUserFlask = new DefinedStructure.P_ShowUserFlask();
                        pShowUserFlask = (DefinedStructure.P_ShowUserFlask)ConvertPacket.ByteArrayToStructure(pToClient._data, pShowUserFlask.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowUserFlaskCubeCount(pShowUserFlask._index, pShowUserFlask._userFlask);

                        break;

                    case DefinedProtocol.eToClient.ShowTotalSkill:

                        DefinedStructure.P_ShowTotalSkill pShowTotalSkill = new DefinedStructure.P_ShowTotalSkill();
                        pShowTotalSkill = (DefinedStructure.P_ShowTotalSkill)ConvertPacket.ByteArrayToStructure(pToClient._data, pShowTotalSkill.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowTotalSkillCubeCount(pShowTotalSkill._totalSkill);

                        break;

                    case DefinedProtocol.eToClient.ShowUserSkill:

                        DefinedStructure.P_ShowUserSkill pShowUserSkill = new DefinedStructure.P_ShowUserSkill();
                        pShowUserSkill = (DefinedStructure.P_ShowUserSkill)ConvertPacket.ByteArrayToStructure(pToClient._data, pShowUserSkill.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowUserSkillCubeCount(pShowUserSkill._index, pShowUserSkill._userSkill, pShowUserSkill._field, pShowUserSkill._userSkillPos);

                        break;

                    case DefinedProtocol.eToClient.ShowUserSlot:

                        DefinedStructure.P_ShowUserSlot pShowUserSlot = new DefinedStructure.P_ShowUserSlot();
                        pShowUserSlot = (DefinedStructure.P_ShowUserSlot)ConvertPacket.ByteArrayToStructure(pToClient._data, pShowUserSlot.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).OpenCardSlot(pShowUserSlot._index, pShowUserSlot._unLockSlot);

                        break;

                    case DefinedProtocol.eToClient.DeletePickCard:

                        DefinedStructure.P_DeletePickCard pDeletePickCard = new DefinedStructure.P_DeletePickCard();
                        pDeletePickCard = (DefinedStructure.P_DeletePickCard)ConvertPacket.ByteArrayToStructure(pToClient._data, pDeletePickCard.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).DeleteCard(pDeletePickCard._index, pDeletePickCard._slotIndex);

                        break;

                    case DefinedProtocol.eToClient.SelectField:

                        DefinedStructure.P_SelectField pSelectField = new DefinedStructure.P_SelectField();
                        pSelectField = (DefinedStructure.P_SelectField)ConvertPacket.ByteArrayToStructure(pToClient._data, pSelectField.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).SelectField(pSelectField._userIndex);

                        break;

                    case DefinedProtocol.eToClient.GameOver:

                        DefinedStructure.P_GameOver pGameOver = new DefinedStructure.P_GameOver();
                        pGameOver = (DefinedStructure.P_GameOver)ConvertPacket.ByteArrayToStructure(pToClient._data, pGameOver.GetType(), pToClient._totalSize);

                        if (pGameOver._specificScore == 0)
                            UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowInformation("게임 종료를 처리하는 중");
                        else
                            UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).GameOver(pGameOver._specificScore);

                        break;

                    case DefinedProtocol.eToClient.SelectCard:

                        DefinedStructure.P_GetCard pSelectCard = new DefinedStructure.P_GetCard();
                        pSelectCard = (DefinedStructure.P_GetCard)ConvertPacket.ByteArrayToStructure(pToClient._data, pSelectCard.GetType(), pToClient._totalSize);

                        BattleManager._instance.ReadyStateChange(BattleManager.eReadyState.SelectCard);
                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).SelectCard(pSelectCard._index);

                        break;

                    case DefinedProtocol.eToClient.SelectMyCard:

                        DefinedStructure.P_SelectMyCard pSelectMyCard = new DefinedStructure.P_SelectMyCard();
                        pSelectMyCard = (DefinedStructure.P_SelectMyCard)ConvertPacket.ByteArrayToStructure(pToClient._data, pSelectMyCard.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).OpenSelectCard(pSelectMyCard._cardArr);

                        break;

                    case DefinedProtocol.eToClient.SelectOtherCard:

                        DefinedStructure.P_SelectOtherCard pSelectOtherCard = new DefinedStructure.P_SelectOtherCard();
                        pSelectOtherCard = (DefinedStructure.P_SelectOtherCard)ConvertPacket.ByteArrayToStructure(pToClient._data, pSelectOtherCard.GetType(), pToClient._totalSize);

                        BattleManager._instance.ReadyStateChange(BattleManager.eReadyState.SelectCard);
                        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).OpenSelectOtherCard(pSelectOtherCard._cardArr);

                        break;

                    case DefinedProtocol.eToClient.ShowGameResult:

                        DefinedStructure.P_ShowGameResult pShowGameResult = new DefinedStructure.P_ShowGameResult();
                        pShowGameResult = (DefinedStructure.P_ShowGameResult)ConvertPacket.ByteArrayToStructure(pToClient._data, pShowGameResult.GetType(), pToClient._totalSize);

                        GameResultUI gameresultUI = UIManager._instance.OpenWnd<GameResultUI>(UIManager.eKindWindow.GameResultUI);
                        

                        break;
                    #endregion

                    #region Shop
                    case DefinedProtocol.eToClient.ShowUserShopInfo:

                        DefinedStructure.P_ShowShopInfo pShowShopInfo = new DefinedStructure.P_ShowShopInfo();
                        pShowShopInfo = (DefinedStructure.P_ShowShopInfo)ConvertPacket.ByteArrayToStructure(pToClient._data, pShowShopInfo.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<ShopUI>(UIManager.eKindWindow.ShopUI).AddMyItmeToInven(pShowShopInfo._itemIndex, pShowShopInfo._itemCount);

                        break;

                    case DefinedProtocol.eToClient.EndUserShopInfo:

                        DefinedStructure.P_EndUserShopInfo pEndUserShopInfo = new DefinedStructure.P_EndUserShopInfo();
                        pEndUserShopInfo = (DefinedStructure.P_EndUserShopInfo)ConvertPacket.ByteArrayToStructure(pToClient._data, pEndUserShopInfo.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<ShopUI>(UIManager.eKindWindow.ShopUI).InitCoin(pEndUserShopInfo._coinArr);
                        LobbyManager._instance.LoadFinish();

                        break;

                    case DefinedProtocol.eToClient.ShowCoinInfo:

                        DefinedStructure.P_ShowCoinInfo pShowCoinInfo = new DefinedStructure.P_ShowCoinInfo();
                        pShowCoinInfo = (DefinedStructure.P_ShowCoinInfo)ConvertPacket.ByteArrayToStructure(pToClient._data, pShowCoinInfo.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<ShopUI>(UIManager.eKindWindow.ShopUI).InitCoin(pShowCoinInfo._coinIndex, pShowCoinInfo._coinValue);

                        break;
                    #endregion

                    case DefinedProtocol.eToClient.ShowFriendList:

                        DefinedStructure.P_ShowFriendList pShowFriendList = new DefinedStructure.P_ShowFriendList();
                        pShowFriendList = (DefinedStructure.P_ShowFriendList)ConvertPacket.ByteArrayToStructure(pToClient._data, pShowFriendList.GetType(), pToClient._totalSize);

                        UIManager._instance.GetWnd<CommunityUI>(UIManager.eKindWindow.CommunityUI).InitMyFriend(pShowFriendList._friendNickName, pShowFriendList._friendLevel);

                        LobbyManager._instance.LoadFinish();

                        break;

                    case DefinedProtocol.eToClient.SystemMessage:

                        DefinedStructure.P_SystemMessage pSystemMessage = new DefinedStructure.P_SystemMessage();
                        pSystemMessage = (DefinedStructure.P_SystemMessage)ConvertPacket.ByteArrayToStructure(pToClient._data, pSystemMessage.GetType(), pToClient._totalSize);

                        SystemMessageUI.Open((SystemMessageUI.eSystemMessageType)Enum.Parse(typeof(SystemMessageUI.eSystemMessageType), pSystemMessage._systemMsgType));
                        
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

    public void QuickEnter(string mode)
    {
        DefinedStructure.P_QuickEnter pQuickEnter;
        pQuickEnter._quickMode = mode;
        pQuickEnter._nickName = _currentNickName;

        ToPacket(DefinedProtocol.eFromClient.QuickEnter, pQuickEnter);
    }

    public void InformReady()
    {
        DefinedStructure.P_InformRoomInfo pInformReady;
        pInformReady._roomNumber = BattleManager._instance._RoomNumber;

        ToPacket(DefinedProtocol.eFromClient.InformReady, pInformReady);
    }

    public void InformGameStart()
    {
        DefinedStructure.P_InformRoomInfo pInformGameStart;
        pInformGameStart._roomNumber = BattleManager._instance._RoomNumber;

        ToPacket(DefinedProtocol.eFromClient.InformGameStart, pInformGameStart);
    }

    public void FinishReadCard()
    {
        DefinedStructure.P_InformRoomInfo pFinishReadCard;
        pFinishReadCard._roomNumber = BattleManager._instance._RoomNumber;

        ToPacket(DefinedProtocol.eFromClient.FinishReadCard, pFinishReadCard);
    }

    public void PickCard(int cardIndex)
    {
        DefinedStructure.P_PickCard pPickCard;
        pPickCard._roomNumber = BattleManager._instance._RoomNumber;
        pPickCard._cardIndex = cardIndex;

        ToPacket(DefinedProtocol.eFromClient.PickCard, pPickCard);
    }

    public void SelectAction(int action)
    {
        DefinedStructure.P_SelectAction pSelectAction;
        pSelectAction._roomNumber = BattleManager._instance._RoomNumber;
        pSelectAction._selectType = action;

        ToPacket(DefinedProtocol.eFromClient.SelectAction, pSelectAction);
    }

    public void PickCardInProgress(int cardIndex)
    {
        DefinedStructure.P_PickCard pPickCard;
        pPickCard._roomNumber = BattleManager._instance._RoomNumber;
        pPickCard._cardIndex = cardIndex;

        ToPacket(DefinedProtocol.eFromClient.PickCardInProgress, pPickCard);
    }

    public void RotateInfo(int index, float rotateValue)
    {
        DefinedStructure.P_RotateInfo pRotateInfo;
        pRotateInfo._roomNumber = BattleManager._instance._RoomNumber;
        pRotateInfo._index = index;
        pRotateInfo._rotateValue = rotateValue;
        pRotateInfo._restCount = UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI)._RestTurnCnt;

        ToPacket(DefinedProtocol.eFromClient.RotateInfo, pRotateInfo);
    }

    public void FinishRotateCard(int[] cardRotateCnt)
    {
        DefinedStructure.P_FinishRotate pFinishRotate;
        pFinishRotate._roomNumber = BattleManager._instance._RoomNumber;
        pFinishRotate._rotateCardInfoArr = new int[4];
        for (int n = 0; n < cardRotateCnt.Length; n++)
            pFinishRotate._rotateCardInfoArr[n] = cardRotateCnt[n];

        ToPacket(DefinedProtocol.eFromClient.FinishRotate, pFinishRotate);
    }

    public void ChooseCompleteCard(int completeCard)
    {
        DefinedStructure.P_ChooseCompleteCard pChooseCompleteCard;
        pChooseCompleteCard._roomNumber = BattleManager._instance._RoomNumber;
        pChooseCompleteCard._index = completeCard;

        ToPacket(DefinedProtocol.eFromClient.ChooseCompleteCard, pChooseCompleteCard);
    }

    public void SelectField(int field)
    {
        DefinedStructure.P_SelectFieldResult pSelectFieldResult;
        pSelectFieldResult._roomNumber = BattleManager._instance._RoomNumber;
        pSelectFieldResult._field = field;

        ToPacket(DefinedProtocol.eFromClient.SelectFieldResult, pSelectFieldResult);
    }

    public void GetShopInfo()
    {
        DefinedStructure.P_RequestShopInfo pRequestShopInfo;
        pRequestShopInfo._nickName = _currentNickName;

        ToPacket(DefinedProtocol.eFromClient.RequestShopInfo, pRequestShopInfo);
    }

    public void BuyItem(int itemIndex)
    {
        DefinedStructure.P_BuyItem pBuyItem;
        pBuyItem._nickName = _currentNickName;
        pBuyItem._itemIndex = itemIndex;

        ToPacket(DefinedProtocol.eFromClient.BuyItem, pBuyItem);
    }

    public void RequestFriendList()
    {
        DefinedStructure.P_RequestFriendList pRequestFriendList;
        pRequestFriendList._nickName = _currentNickName;

        ToPacket(DefinedProtocol.eFromClient.RequestFriendList, pRequestFriendList);
    }

    public void SelectCardResult(int cardIndex)
    {
        DefinedStructure.P_PickCard pPickCard;
        pPickCard._roomNumber = BattleManager._instance._RoomNumber;
        pPickCard._cardIndex = cardIndex;

        ToPacket(DefinedProtocol.eFromClient.SelectCardResult, pPickCard);
    }

    public void FinishGameOver()
    {
        DefinedStructure.P_InformRoomInfo pFinishGameOver;
        pFinishGameOver._roomNumber = BattleManager._instance._RoomNumber;

        ToPacket(DefinedProtocol.eFromClient.FinishGameOver, pFinishGameOver);
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

    private void OnApplicationQuit()
    {
        if (_server != null)
        {
            DefinedStructure.P_Request pConnectionTerminate;

            ToPacket(DefinedProtocol.eFromClient.ConnectionTerminate, pConnectionTerminate);

            DefinedStructure.PacketInfo packetRecieve;
            packetRecieve._id = (int)DefinedProtocol.eFromClient.ConnectionTerminate;
            packetRecieve._data = new byte[1024];
            byte[] temp = ConvertPacket.StructureToByteArray(pConnectionTerminate);
            for (int n = 0; n < temp.Length; n++)
                packetRecieve._data[n] = temp[n];
            packetRecieve._totalSize = temp.Length;

            _server.Send(ConvertPacket.StructureToByteArray(packetRecieve));

            try
            {
                _server.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            finally
            {
                _server.Close(0);
                _server = null;
            }
        }
    }
}
