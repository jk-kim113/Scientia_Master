﻿using System.Collections;
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
    [SerializeField]
    Text _informText;
#pragma warning restore

    Text _reaCardTimeText;

    public bool _IsMyTurn { get; set; }

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

            case BattleManager.eReadyState.GameStart:

                AllReadyCancel();

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

    public void ShowMaster(int masterIndex)
    {
        for(int n = 0; n < _userInfoArr.Length; n++)
        {
            if (_userInfoArr[n]._MyIndex == masterIndex)
                _userInfoArr[n].ShowMaster(true);
            else
                _userInfoArr[n].ShowMaster(false);
        }

        _actionBtn.GetComponentInChildren<Text>().text = _userInfoArr[0]._MyIndex == masterIndex ? "시작하기" : "준비하기";

        if (_userInfoArr[0]._MyIndex == masterIndex)
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

    void AllReadyCancel()
    {
        for(int n = 0; n < _userInfoArr.Length; n++)
        {
            _userInfoArr[n].ReadyState(false);
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
        _IsMyTurn = _userInfoArr[0]._MyIndex == index;

        for(int n = 0; n < _cardSlotArr.Length; n++)
            _cardSlotArr[n].ShowTurn(n == index);

        for(int n = 0; n < _userInfoArr.Length; n++)
        {
            if (_userInfoArr[n]._MyIndex == index)
                _userInfoArr[n].ShowTurn(true);
            else
                _userInfoArr[n].ShowTurn(false);
        }
    }

    public void ShowAddCard(int index, int slotIndex, int cardIndex)
    {
        _cardSlotArr[index].ShowAddCard(slotIndex, cardIndex);
    }

    public void ChooseAction(int index)
    {
        _projectBoard.gameObject.SetActive(false);

        _stateObj[(int)BattleManager.eReadyState.SelectionAction].gameObject.SetActive(_userInfoArr[0]._MyIndex == index);
        _informText.gameObject.SetActive(_userInfoArr[0]._MyIndex != index);
        _informText.text = "행동 선택중...";
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
