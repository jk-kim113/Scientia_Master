using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private void Awake()
    {
        UIManager._instance.Close(UIManager.eKindWindow.CreateRoomUI);
        UIManager._instance.Close(UIManager.eKindWindow.LobbyUI);

        UIManager._instance.OpenWnd<BattleUI>(UIManager.eKindWindow.BattleUI);
    }
}
