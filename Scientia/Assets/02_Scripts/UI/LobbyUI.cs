using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Transform _roomTr;
#pragma warning restore

    GameObject _prefabRoomInfo;

    private void Awake()
    {
        _prefabRoomInfo = ResourcePoolManager._instance.GetObj<GameObject>(ResourcePoolManager.eResourceKind.Prefab, "RoomInfo");
    }

    public void ShowRoomInfo(int roomNumber, string name, bool isLock, int currentMemberCnt, int maxMemberCnt, string mode, string rule, string isPlay)
    {
        RoomInfo room = Instantiate(_prefabRoomInfo, _roomTr).GetComponent<RoomInfo>();
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
}
