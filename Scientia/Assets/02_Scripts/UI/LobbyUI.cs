using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Transform _roomTr;
#pragma warning restore

    public void ShowRoomInfo(int roomNumber, string name, bool isLock, int currentMemberCnt, int maxMemberCnt, string mode, string rule, string isPlay)
    {
        GameObject prefabRoomInfo = ResourcePoolManager._instance.GetObj<GameObject>(ResourcePoolManager.eResourceKind.Prefab, "RoomInfo");
        RoomInfo room = Instantiate(prefabRoomInfo, _roomTr).GetComponent<RoomInfo>();
        room.InitRoomInfo(roomNumber, name, isLock, currentMemberCnt, maxMemberCnt, mode, rule, isPlay);
    }

    public void MyInfoButton()
    {
        UIManager._instance.OpenWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI);
    }

    public void CreateRoomButton()
    {
        UIManager._instance.OpenWnd<CreateRoomUI>(UIManager.eKindWindow.CreateRoomUI);
    }

    public void ShopButton()
    {
        UIManager._instance.OpenWnd<ShopUI>(UIManager.eKindWindow.ShopUI);
    }

    public void CommunityButton()
    {
        UIManager._instance.OpenWnd<CommunityUI>(UIManager.eKindWindow.CommunityUI);
    }
}
