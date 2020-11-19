using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public enum eGameState
    {
        GameWait,
        GamePlaying,

        max
    }

#pragma warning disable 0649
    [SerializeField]
    BattleInfo[] _userInfoArr;
    [SerializeField]
    GameObject[] _stateObj;
    [SerializeField]
    Button _actionBtn;
#pragma warning restore

    eGameState _currentGameState;

    int _roomNumber;
    public int _RoomNumber { set { _roomNumber = value; } }

    private void Awake()
    {
        _currentGameState = eGameState.GameWait;
    }

    private void Start()
    {
        OpenStateObj();
    }

    void OpenStateObj()
    {
        for (int n = 0; n < _stateObj.Length; n++)
        {
            if ((eGameState)n == _currentGameState)
                _stateObj[n].SetActive(true);
            else
                _stateObj[n].SetActive(false);
        }
    }

    public void ShowUserInfo(int index, string nickName, int level, bool isReady)
    {
        if(ClientManager._instance._NowNickName.CompareTo(nickName) == 0)
        {
            _userInfoArr[0].ShowInfo(index, nickName, level, isReady);
        }
        else
        {
            for(int n = 1; n < 4; n++)
            {
                if(!_userInfoArr[n]._IsEmpty)
                {
                    _userInfoArr[n].ShowInfo(index, nickName, level, isReady);
                    return;
                }
            }
        }
    }

    public void ShowMaster(bool isMaster)
    {
        _actionBtn.GetComponentInChildren<Text>().text = isMaster ? "시작하기" : "준비하기";
        _userInfoArr[0].ShowMaster(isMaster);

        if (isMaster)
            _actionBtn.onClick.AddListener(() => { InformGameStart(); });
        else
            _actionBtn.onClick.AddListener(() => { InformGameReady(); });
    }

    void InformGameStart()
    {
        ClientManager._instance.InformGameStart(_roomNumber);
    }

    void InformGameReady()
    {
        ClientManager._instance.InformReady(_roomNumber);
    }

    public void ShowReadyUser(int index, bool isReady)
    {
        for(int n = 0; n < _userInfoArr.Length; n++)
        {
            if(_userInfoArr[n]._MyIndex == index)
            {
                _userInfoArr[n].ReadyState(isReady);

                break;
            }
        }
    }

    public void ExitButton()
    {
        SceneControlManager._instance.StartLoadLobbyScene();
    }
}
