using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    public void MyInfoButton()
    {
        UIManager._instance.OpenWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI);
    }

    public void CreateRoomButton()
    {
        UIManager._instance.OpenWnd<CreateRoomUI>(UIManager.eKindWindow.CreateRoomUI);
    }
}
