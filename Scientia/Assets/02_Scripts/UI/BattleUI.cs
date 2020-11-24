using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public enum eActionKind
    {
        GetCard,
        RotateCard
    }

#pragma warning disable 0649
    [SerializeField]
    BattleInfo[] _userInfoArr;
    [SerializeField]
    GameObject[] _stateObj;
    [SerializeField]
    Button _actionBtn;
    [SerializeField]
    ProjectBoard _projectBoard;
    [SerializeField]
    CardSlot[] _cardSlotArr;
#pragma warning restore

    Text _reaCardTimeText;

    private void Start()
    {
        _reaCardTimeText = _stateObj[(int)BattleManager.eReadyState.ReadCard].GetComponentInChildren<Text>();
    }

    public void StateChange(BattleManager.eReadyState state)
    {
        switch(state)
        {
            case BattleManager.eReadyState.GameWait:
            case BattleManager.eReadyState.ReadCard:
            case BattleManager.eReadyState.PickCard:
            case BattleManager.eReadyState.SelectionAction:

                for (int n = 0; n < _stateObj.Length; n++)
                    _stateObj[n].SetActive((int)state == n);

                break;

            case BattleManager.eReadyState.WaitServer:

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
                if(_userInfoArr[n]._IsEmpty)
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

    public void ShowReadCardTime(int time)
    {
        _reaCardTimeText.text = time.ToString();
    }

    public void PickCardTurn(int index)
    {
        for(int n = 0; n < _cardSlotArr.Length; n++)
            _cardSlotArr[n].ShowTurn(n == index);
    }

    public void ShowAddCard(int index, int slotIndex, int cardIndex)
    {
        _cardSlotArr[index].ShowAddCard(slotIndex, cardIndex);
    }

    public void GetCardAction()
    {
        ClientManager._instance.SelectAction((int)eActionKind.GetCard);
    }

    public void RotateCardAction()
    {
        ClientManager._instance.SelectAction((int)eActionKind.RotateCard);
    }

    public void ExitButton()
    {
        //TODO Send Packet To Server
        SceneControlManager._instance.StartLoadLobbyScene();
    }
}
