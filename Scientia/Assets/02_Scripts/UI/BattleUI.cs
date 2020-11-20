using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    BattleInfo[] _userInfoArr;
    [SerializeField]
    GameObject[] _stateObj;
    [SerializeField]
    Button _actionBtn;
    [SerializeField]
    ProjectBoard _projectBoard;
#pragma warning restore

    public void StateChange(BattleManager.eBattleState state)
    {
        switch(state)
        {
            case BattleManager.eBattleState.GameWait:
            case BattleManager.eBattleState.ReadCard:
            case BattleManager.eBattleState.GamePlaying:

                for (int n = 0; n < _stateObj.Length; n++)
                    _stateObj[n].SetActive((int)state == n);

                break;

            case BattleManager.eBattleState.WaitServer:

                for (int n = 0; n < _stateObj.Length; n++)
                {
                    if(_stateObj[n].activeSelf)
                        _stateObj[n].SetActive(false);
                }

                break;
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
        ClientManager._instance.InformGameStart();
    }

    void InformGameReady()
    {
        ClientManager._instance.InformReady();
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

    public void ShowPickedCard(int[] pickedCardArr)
    {
        _projectBoard.ShowPickedCard(pickedCardArr);
    }

    public void ExitButton()
    {
        SceneControlManager._instance.StartLoadLobbyScene();
    }
}
