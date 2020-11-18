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
        
        ClientManager._instance.RequestMyInfoData();
        _currentLoadType = eLoadType.MyInfoWait;
    }

    public void LoadFinish()
    {
        switch(_currentLoadType)
        {
            case eLoadType.MyInfoWait:

                UIManager._instance.Close(UIManager.eKindWindow.MyInfoUI);
                _currentLoadType = eLoadType.LoadEnd;
                ClientManager._instance.GetRoomList();

                break;
        }
    }
}
