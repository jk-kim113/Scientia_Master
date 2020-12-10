using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityUI : MonoBehaviour
{
    public enum eFriendTab
    {
        MyFriend,
        RequestFriend,
        AddFriend,

        max
    }

#pragma warning disable 0649
    [SerializeField]
    GameObject[] _friendGroupArr;
    [SerializeField]
    FriendTab[] _friendTabArr;
    [SerializeField]
    Transform[] _friendTrArr;
#pragma warning restore

    eFriendTab _currentTab;

    private void Start()
    {
        _currentTab = eFriendTab.MyFriend;

        for(int n = 0; n < _friendTabArr.Length; n++)
        {
            _friendTabArr[n].InitTab(this, n, n == (int)_currentTab);
            _friendGroupArr[n].SetActive(n == (int)_currentTab);
        }
    }

    public void InitMyFriend(string friendName, int[] friendLevel)
    {
        string[] nickname = friendName.Split(new char[1] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

        for(int n = 0; n < nickname.Length; n++)
        {
            GameObject prefamMyFriend = ResourcePoolManager._instance.GetObj<GameObject>(ResourcePoolManager.eResourceKind.Prefab, "MyFriendObj") as GameObject;
            MyFriendObj myFriend = Instantiate(prefamMyFriend, _friendTrArr[(int)eFriendTab.MyFriend]).GetComponent<MyFriendObj>();
            myFriend.InitMyFriend(n + 1, nickname[n], friendLevel[n]);
        }
    }

    public void InitReceiveFriend(string receiveName, int[] receiveLevel)
    {
        string[] nickname = receiveName.Split(new char[1] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int n = 0; n < nickname.Length; n++)
        {
            GameObject prefamMyFriend = ResourcePoolManager._instance.GetObj<GameObject>(ResourcePoolManager.eResourceKind.Prefab, "MyFriendObj") as GameObject;
            MyFriendObj myFriend = Instantiate(prefamMyFriend, _friendTrArr[(int)eFriendTab.MyFriend]).GetComponent<MyFriendObj>();
            myFriend.InitMyFriend(n + 1, nickname[n], receiveLevel[n]);
        }
    }

    public void SelectTab(int index)
    {
        _currentTab = (eFriendTab)index;

        for(int n = 0; n < _friendTabArr.Length; n++)
        {
            if (n != (int)_currentTab)
                _friendTabArr[n].OffMode();

            _friendGroupArr[n].SetActive(n == (int)_currentTab);
        }
    }

    public void ExitButton()
    {
        UIManager._instance.Close(UIManager.eKindWindow.CommunityUI);
    }
}
