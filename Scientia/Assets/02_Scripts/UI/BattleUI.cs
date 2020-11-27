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
    [SerializeField]
    GameObject _rotateCardObj;
    [SerializeField]
    Text _turnCntText;
    [SerializeField]
    Button _rotateOkButton;
    [SerializeField]
    GameObject _selectFieldObj;
#pragma warning restore

    Text _reaCardTimeText;
    RotateCard[] _rotateCardArr;

    public bool _IsMyTurn { get; set; }

    int _turnCount;
    public int _TurnCount { get { return _turnCount; } }

    int _currentTurn;
    public int _NowTurn 
    { 
        get
        {
            return _currentTurn;
        }
        set
        {
            _currentTurn = value;
            _turnCntText.text = "남은 회전 횟수 : " + _RestTurnCnt.ToString();
        }
    }

    public int _RestTurnCnt { get { return _turnCount - _currentTurn; } }

    int _selectComplete = -1;

    private void Start()
    {
        _reaCardTimeText = _stateObj[(int)BattleManager.eReadyState.ReadCard].GetComponentInChildren<Text>();
        _rotateCardArr = _rotateCardObj.GetComponentsInChildren<RotateCard>();
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

            case BattleManager.eReadyState.DoingAction:
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
        _rotateCardObj.SetActive(false);
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

    public void GetCardState(int index)
    {
        StateChange(BattleManager.eReadyState.PickCard);
        _IsMyTurn = _userInfoArr[0]._MyIndex == index;

        _projectBoard.gameObject.SetActive(_userInfoArr[0]._MyIndex == index);

        _informText.gameObject.SetActive(_userInfoArr[0]._MyIndex != index);
        _informText.text = "카드 고르는중...";
    }

    public void RotateCardState(int index, int[] cardState, int turnCount)
    {
        _rotateOkButton.gameObject.SetActive(_userInfoArr[0]._MyIndex == index);
        _rotateOkButton.onClick.RemoveAllListeners();
        _rotateOkButton.onClick.AddListener(() => { FinishRotateCard(); });

        StateChange(BattleManager.eReadyState.DoingAction);
        _informText.gameObject.SetActive(false);
        _IsMyTurn = _userInfoArr[0]._MyIndex == index;
        _turnCount = turnCount;
        _NowTurn = 0;

        _rotateCardObj.SetActive(true);

        for(int n = 0; n < _rotateCardArr.Length; n++)
        {
            switch(cardState[n])
            {
                case 0:

                    _rotateCardArr[n].InitCard(ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, "EmptyCardSlot"), RotateCard.eCardType.Empty, n);

                    break;
                case -1:

                    _rotateCardArr[n].InitCard(ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, "CloseCardSlot"), RotateCard.eCardType.Lock, n);

                    break;
                default:

                    _rotateCardArr[n].InitCard(
                        ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, TableManager._instance.Get(eTableType.CardData).ToS(cardState[n], "Name")), 
                        RotateCard.eCardType.Rotatable,
                        n);

                    break;
            }
        }
    }

    public void FinishRotateCard()
    {
        if(_turnCount == 0)
        {
            //TODO System Message Rotate at least one more

            return;
        }

        int[] rotateCnt = new int[4];
        for (int n = 0; n < _rotateCardArr.Length; n++)
            rotateCnt[n] = _rotateCardArr[n]._MyRotateCount;

        ClientManager._instance.FinishRotateCard(rotateCnt);
    }

    public void ShowRotate(int index, float rotateValue, int restCnt)
    {
        _rotateCardArr[index].SetRotation(rotateValue);
        _turnCntText.text = "남은 회전 횟수 : " + restCnt.ToString();
    }

    public void ShowCompleteCard(int index, int[] completeCard)
    {
        _selectComplete = -1;
        _rotateOkButton.gameObject.SetActive(_userInfoArr[0]._MyIndex == index);
        _rotateOkButton.onClick.RemoveAllListeners();
        _rotateOkButton.onClick.AddListener(() => { ChooseCompleteCard(); });

        _turnCntText.gameObject.SetActive(false);
        _informText.gameObject.SetActive(_userInfoArr[0]._MyIndex != index);
        _informText.text = "완료카드 고르는중...";

        if (_userInfoArr[0]._MyIndex == index)
        {
            for(int n = 0; n < _rotateCardArr.Length; n++)
            {
                if(completeCard[n] > 0)
                {
                    _rotateCardArr[n].gameObject.SetActive(true);
                    _rotateCardArr[n].InitCard(
                        ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, TableManager._instance.Get(eTableType.CardData).ToS(completeCard[n], "Name")),
                        RotateCard.eCardType.Selectable,
                        n);
                }
                else
                    _rotateCardArr[n].gameObject.SetActive(false);

            }
        }
    }

    public void ClickCompleteCard(int index)
    {
        if (_selectComplete == index)
            return;

        _selectComplete = index;

        for (int n = 0; n < _rotateCardArr.Length; n++)
        {
            _rotateCardArr[n].ShowClick(n == index);
        }
    }

    public void ChooseCompleteCard()
    {
        if (_selectComplete < 0)
        {
            //TODO System Message no select
        }
        else
            ClientManager._instance.ChooseCompleteCard(_selectComplete);
    }

    public void SelectField(int count)
    {
        if(count <= 0)
        {
            _informText.text = "점수 계산하는 중...";
            _informText.gameObject.SetActive(true);
        }
        else
        {
            _selectFieldObj.SetActive(true);
        }
    }

    public void ExitButton()
    {
        //TODO Send Packet To Server
    }
}
