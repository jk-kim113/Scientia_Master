using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public enum eQuickMode
    {
        MyDeck,
        RandomDeck,
        AllDeck,

        max
    }

#pragma warning disable 0649
    [SerializeField]
    Transform _roomTr;
    [SerializeField]
    Dropdown _quickEnterMode;
#pragma warning restore

    List<RoomInfo> _roomInfoList = new List<RoomInfo>();

    private void Start()
    {
        List<string> temp = new List<string>();

        for (int n = 0; n < (int)eQuickMode.max; n++)
            temp.Add(((eQuickMode)n).ToString());

        _quickEnterMode.ClearOptions();
        _quickEnterMode.AddOptions(temp);
    }

    public void ShowRoomInfo(int roomNumber, string name, bool isLock, int currentMemberCnt, int maxMemberCnt, string mode, string rule, string isPlay)
    {
        for(int n = 0; n < _roomInfoList.Count; n++)
        {
            if(_roomInfoList[n]._MyRoomNumber == roomNumber)
            {
                _roomInfoList[n].RenewRoomInfo(currentMemberCnt, maxMemberCnt, isPlay);
                return;
            }
        }

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

    public void QuickEnterButton()
    {
        ClientManager._instance.QuickEnter(_quickEnterMode.options[_quickEnterMode.value].text);
    }

    public void RefreshButton()
    {
        ClientManager._instance.GetRoomList();
    }
}
