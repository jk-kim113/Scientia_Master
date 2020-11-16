using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private void Awake()
    {
        UIManager._instance.OpenWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI);
        UIManager._instance.Close(UIManager.eKindWindow.MyInfoUI);

        ClientManager._instance.RequestMyCardReleaseInfo();
    }
}
