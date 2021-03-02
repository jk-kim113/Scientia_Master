using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    static BattleManager _uniqueInstance;
    public static BattleManager _instance { get { return _uniqueInstance; } }

    int _roomNumber;
    public int _RoomNumber { get { return _roomNumber; } set { _roomNumber = value; } }

    private void Awake()
    {
        _uniqueInstance = this;

        UIManager._instance.Close(UIManager.eKindWindow.CreateRoomUI);
        UIManager._instance.Close(UIManager.eKindWindow.LobbyUI);

        UIManager._instance.OpenWnd<BattleUI>(UIManager.eKindWindow.BattleUI);
    }
}
