using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    static LobbyManager _uniqueInstance;
    public static LobbyManager _instance { get { return _uniqueInstance; } }

    public enum eLoadType
    {
        none,

        MyInfoStart,
        MyInfoWait,
        MyInfoEnd,

        LobbyInfoStart,
        LobbyInfoWait,
        LobbyInfoEnd,

        ShopInfoStart,
        ShopInfoWait,
        ShopInfoEnd,

        FriendInfoStart,
        FriendInfoWait,
        FriendInfoEnd,

        LoadEnd
    }

    eLoadType _currentLoadType;
    public eLoadType _nowLoadType { get { return _currentLoadType; } }

    private void Awake()
    {
        _uniqueInstance = this;
        _currentLoadType = eLoadType.none;

        StartLoad();
    }

    void StartLoad()
    {
        _currentLoadType = eLoadType.MyInfoStart;

        UIManager._instance.OpenWnd<LobbyUI>(UIManager.eKindWindow.LobbyUI);
        UIManager._instance.OpenWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI);
        UIManager._instance.OpenWnd<ShopUI>(UIManager.eKindWindow.ShopUI);
        UIManager._instance.OpenWnd<CommunityUI>(UIManager.eKindWindow.CommunityUI);

        ClientManager._instance.RequestMyInfoData();
        _currentLoadType = eLoadType.MyInfoWait;
    }

    public void LoadFinish()
    {
        switch(_currentLoadType)
        {
            case eLoadType.MyInfoWait:

                _currentLoadType = eLoadType.MyInfoEnd;
                UIManager._instance.Close(UIManager.eKindWindow.MyInfoUI);
                _currentLoadType = eLoadType.LobbyInfoStart;
                ClientManager._instance.GetRoomList();
                _currentLoadType = eLoadType.LobbyInfoWait;

                break;

            case eLoadType.LobbyInfoWait:

                _currentLoadType = eLoadType.LobbyInfoEnd;
                _currentLoadType = eLoadType.ShopInfoStart;
                ClientManager._instance.GetShopInfo();
                _currentLoadType = eLoadType.ShopInfoWait;

                break;

            case eLoadType.ShopInfoWait:

                _currentLoadType = eLoadType.ShopInfoEnd;
                UIManager._instance.Close(UIManager.eKindWindow.ShopUI);
                _currentLoadType = eLoadType.FriendInfoStart;
                ClientManager._instance.RequestFriendList();
                _currentLoadType = eLoadType.FriendInfoWait;

                break;

            case eLoadType.FriendInfoWait:

                _currentLoadType = eLoadType.FriendInfoEnd;
                UIManager._instance.Close(UIManager.eKindWindow.CommunityUI);
                _currentLoadType = eLoadType.LoadEnd;

                break;
        }
    }
}
