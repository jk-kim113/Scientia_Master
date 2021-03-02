using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    GameObject _gameWaitObj;
    [SerializeField]
    GameObject _rotateCardObj;
    [SerializeField]
    BattleInfo[] _userInfoArr;
    [SerializeField]
    ProjectBoard _projectBoard;
    [SerializeField]
    Text _totalSkillcubeCntTxt;
    [SerializeField]
    Text _totalFlaskcubeTxt;
    [SerializeField]
    Text _reaCardTimeText;
    [SerializeField]
    GameObject _playerCardSlot;
    [SerializeField]
    GameObject _cubeSlot;
    [SerializeField]
    GameObject _selectActionObj;
    [SerializeField]
    Button _rotateOkButton;
    [SerializeField]
    CardSlot[] _cardSlotArr;
    [SerializeField]
    Text _informText;
    [SerializeField]
    Text _turnCntText;
    [SerializeField]
    GameObject _selectFieldObj;
    [SerializeField]
    SelectCard _selectCard;
    [SerializeField]
    SelectOtherCard _selectOtherCard;
#pragma warning restore

    RotateCard[] _rotateCardArr;

    int _physicsEffectCount;
    bool _isGameOver;

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
            Debug.Log(_RestTurnCnt);
        }
    }
    
    public int _RestTurnCnt { get { return _turnCount - _currentTurn; } }

    int _selectComplete = -1;

    Button _actionBtn;
    Text _actionText;
    EnumClass.eBattleState _currentBattleState;
    public EnumClass.eBattleState _NowBattleState { get { return _currentBattleState; } }
    float _readTimeCount;

    private void Start()
    {
        _rotateCardArr = _rotateCardObj.GetComponentsInChildren<RotateCard>();

        _actionBtn = _gameWaitObj.GetComponentsInChildren<Button>()[0];
        _actionText = _actionBtn.GetComponentInChildren<Text>();
        InitState(EnumClass.eBattleState.BattleWait);
    }

    private void Update()
    {
        if(_currentBattleState == EnumClass.eBattleState.ScanCard)
        {
            _readTimeCount -= Time.deltaTime;

            if (_readTimeCount < 0)
            {
                ChangeState(EnumClass.eBattleState.FirstPickCard);
                ClientManager._instance.FinishReadCard(_userInfoArr[0]._MyIndex);
            }

            _reaCardTimeText.text = _readTimeCount.ToString("N0");
        }
    }

    void InitState(EnumClass.eBattleState battlestate)
    {
        _currentBattleState = battlestate;

        EnterState(_currentBattleState);
    }

    public void ChangeState(EnumClass.eBattleState battlestate)
    {
        ExitState(_currentBattleState);

        _currentBattleState = battlestate;

        EnterState(_currentBattleState);
    }

    void EnterState(EnumClass.eBattleState battlestate)
    {
        Debug.Log(string.Format("{0} 상태 입장", battlestate.ToString()));
        switch (battlestate)
        {
            case EnumClass.eBattleState.BattleWait:

                _gameWaitObj.SetActive(true);

                break;

            case EnumClass.eBattleState.ScanCard:

                InitBattleInfo();
                _readTimeCount = 5;
                _projectBoard.gameObject.SetActive(true);
                _reaCardTimeText.gameObject.SetActive(true);

                break;

            case EnumClass.eBattleState.FirstPickCard:

                _projectBoard.gameObject.SetActive(true);
                _playerCardSlot.SetActive(true);
                _cubeSlot.SetActive(true);

                break;

            case EnumClass.eBattleState.SelectAction:

                _playerCardSlot.SetActive(true);
                _cubeSlot.SetActive(true);

                break;

            case EnumClass.eBattleState.SelectProjectCard:

                _projectBoard.gameObject.SetActive(true);
                _playerCardSlot.SetActive(true);
                _cubeSlot.SetActive(true);

                break;

            case EnumClass.eBattleState.RotateMyCard:

                _rotateCardObj.SetActive(true);
                _playerCardSlot.SetActive(true);
                _cubeSlot.SetActive(true);

                break;

            case EnumClass.eBattleState.SelectMyCard:

                _projectBoard.gameObject.SetActive(true);
                _playerCardSlot.SetActive(true);
                _cubeSlot.SetActive(true);

                break;

            case EnumClass.eBattleState.SelectField:

                break;
        }
    }

    void ExitState(EnumClass.eBattleState battlestate)
    {
        Debug.Log(string.Format("{0} 상태 퇴장", battlestate.ToString()));
        switch (battlestate)
        {
            case EnumClass.eBattleState.BattleWait:

                _gameWaitObj.SetActive(false);

                break;

            case EnumClass.eBattleState.ScanCard:

                _projectBoard.gameObject.SetActive(false);
                _reaCardTimeText.gameObject.SetActive(false);

                break;

            case EnumClass.eBattleState.FirstPickCard:

                _projectBoard.gameObject.SetActive(false);
                _playerCardSlot.SetActive(false);
                _cubeSlot.SetActive(false);

                break;

            case EnumClass.eBattleState.SelectAction:

                _selectActionObj.SetActive(false);
                _playerCardSlot.SetActive(false);
                _cubeSlot.SetActive(false);
                _informText.gameObject.SetActive(false);

                break;

            case EnumClass.eBattleState.SelectProjectCard:

                _projectBoard.gameObject.SetActive(false);
                _playerCardSlot.SetActive(false);
                _cubeSlot.SetActive(false);

                break;

            case EnumClass.eBattleState.RotateMyCard:

                _rotateCardObj.SetActive(false);
                _playerCardSlot.SetActive(false);
                _cubeSlot.SetActive(false);

                break;

            case EnumClass.eBattleState.SelectMyCard:

                _projectBoard.gameObject.SetActive(false);
                _playerCardSlot.SetActive(false);
                _cubeSlot.SetActive(false);

                break;

            case EnumClass.eBattleState.SelectField:

                _informText.gameObject.SetActive(false);
                _selectFieldObj.SetActive(false);

                break;
        }
    }

    void InformGameStart()
    {
        ClientManager._instance.InformGameStart(_userInfoArr[0]._MyIndex);
    }

    void InformGameReady()
    {
        ClientManager._instance.InformReady(_userInfoArr[0]._MyIndex);
    }

    void InitBattleInfo()
    {
        for (int n = 0; n < _userInfoArr.Length; n++)
        {
            if (!_userInfoArr[n]._IsEmpty)
            {
                _userInfoArr[n].InitInfo();
                _userInfoArr[n].ReadyState(false);
            }
                
        }
    }

    public void ShowMyInfo(int index, string nickName, int level, bool isReady)
    {
        _userInfoArr[0].ShowInfo(index, nickName, level, isReady);
    }

    public void ShowOtherInfo(int index, string nickName, int level, bool isReady)
    {
        for (int n = 1; n < 4; n++)
        {
            if (_userInfoArr[n]._IsEmpty)
            {
                _userInfoArr[n].ShowInfo(index, nickName, level, isReady);
                return;
            }
        }
    }

    public void ShowReadyUser(int index, bool isReady)
    {
        for (int n = 0; n < _userInfoArr.Length; n++)
        {
            if (_userInfoArr[n]._MyIndex == index)
            {
                _userInfoArr[n].ReadyState(isReady);

                break;
            }
        }
    }

    public void ShowMaster(int masterIndex)
    {
        for (int n = 0; n < _userInfoArr.Length; n++)
        {
            if (_userInfoArr[n]._MyIndex == masterIndex)
                _userInfoArr[n].ShowMaster(true);
            else
                _userInfoArr[n].ShowMaster(false);
        }

        _actionText.text = _userInfoArr[0]._MyIndex == masterIndex ? "시작하기" : "준비하기";

        _actionBtn.onClick.RemoveAllListeners();
        if (_userInfoArr[0]._MyIndex == masterIndex)
            _actionBtn.onClick.AddListener(() => { InformGameStart(); });
        else
            _actionBtn.onClick.AddListener(() => { InformGameReady(); });
    }

    public void ShowTotalSkillCubeCount(int skillcube)
    {
        _totalSkillcubeCntTxt.text = "x " + skillcube.ToString();
    }

    public void ShowTotalFlaskCubeCount(int flaskcube)
    {
        _totalFlaskcubeTxt.text = "x " + flaskcube.ToString();
    }

    public void ShowPickedCard(int[] pickedCardArr)
    {
        _projectBoard.ShowPickedCard(pickedCardArr);
    }

    public void PickCardTurn(int index)
    {
        _IsMyTurn = _userInfoArr[0]._MyIndex == index;

        for (int n = 0; n < _cardSlotArr.Length; n++)
            _cardSlotArr[n].ShowTurn(n == index);

        for (int n = 0; n < _userInfoArr.Length; n++)
            _userInfoArr[n].ShowTurn(_userInfoArr[n]._MyIndex == index);
    }

    public void ShowAddCard(int index, int slotIndex, int cardIndex)
    {
        _cardSlotArr[index].ShowAddCard(slotIndex, cardIndex);
    }

    public void ChooseAction(int index)
    {
        _selectActionObj.gameObject.SetActive(_userInfoArr[0]._MyIndex == index);
        _informText.gameObject.SetActive(_userInfoArr[0]._MyIndex != index);
        _informText.text = "행동 선택중...";

        PickCardTurn(index);
    }

    public void GetCardAction()
    {
        ClientManager._instance.SelectAction((int)EnumClass.eActionKind.GetCard);
    }

    public void RotateCardAction()
    {
        ClientManager._instance.SelectAction((int)EnumClass.eActionKind.RotateCard);
    }

    public void RenewProjectBoard(int cardIndex, int cardCount)
    {
        _projectBoard.RenewCard(cardIndex, cardCount);
    }

    public void GetCardState(int index)
    {
        _IsMyTurn = _userInfoArr[0]._MyIndex == index;

        _projectBoard.gameObject.SetActive(_userInfoArr[0]._MyIndex == index);

        _informText.gameObject.SetActive(_userInfoArr[0]._MyIndex != index);
        _informText.text = "카드 고르는중...";
    }

    public void RotateCardState(int index, int[] cardState, int[] cardRotateInfo, int turnCount)
    {
        _rotateOkButton.gameObject.SetActive(_userInfoArr[0]._MyIndex == index);
        _rotateOkButton.onClick.RemoveAllListeners();
        _rotateOkButton.onClick.AddListener(() => { FinishRotateCard(); });

        _IsMyTurn = _userInfoArr[0]._MyIndex == index;
        _turnCount = turnCount;
        _NowTurn = 0;
        _currentTurn = 0;

        for (int n = 0; n < _rotateCardArr.Length; n++)
        {
            switch (cardState[n])
            {
                case 0:

                    _rotateCardArr[n].InitCard(ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, "EmptyCardSlot"),
                        RotateCard.eCardType.Empty, n, cardRotateInfo[n]);

                    break;
                case -1:

                    _rotateCardArr[n].InitCard(ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, "CloseCardSlot"),
                        RotateCard.eCardType.Lock, n, cardRotateInfo[n]);

                    break;
                default:

                    _rotateCardArr[n].InitCard(
                        ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, TableManager._instance.Get(eTableType.CardData).ToS(cardState[n], "Name")),
                        RotateCard.eCardType.Rotatable, n, cardRotateInfo[n]);

                    break;
            }
        }
    }

    public void FinishRotateCard()
    {
        if (_turnCount == 0)
        {
            //TODO System Message Rotate at least one more

            return;
        }

        int[] rotateCnt = new int[4];
        for (int n = 0; n < _rotateCardArr.Length; n++)
            rotateCnt[n] = _rotateCardArr[n]._MyRotateCount;

        ClientManager._instance.FinishRotateCard(rotateCnt);
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

    public void ShowRotate(int index, float rotateValue, int restCnt)
    {
        if (!_IsMyTurn)
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
            for (int n = 0; n < _rotateCardArr.Length; n++)
            {
                if (completeCard[n] > 0)
                {
                    _rotateCardArr[n].gameObject.SetActive(true);
                    _rotateCardArr[n].InitCard(
                        ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, TableManager._instance.Get(eTableType.CardData).ToS(completeCard[n], "Name")),
                        RotateCard.eCardType.Selectable,
                        n, 0);
                }
                else
                    _rotateCardArr[n].gameObject.SetActive(false);

            }
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

    public void ShowUserSkillCubeCount(int index, int skillcube, int field, int[] skillPos)
    {
        if (_userInfoArr[index] != null)
            _userInfoArr[index].ShowSkillCube(skillcube, field, skillPos);
    }

    public void ShowUserFlaskCubeCount(int index, int flaskcube)
    {
        if (_userInfoArr[index] != null)
            _userInfoArr[index].ShowFlaskCube(flaskcube);
    }

    public void DeleteCard(int index, int slotIndex)
    {
        _cardSlotArr[index].DeleteCard(slotIndex);
    }

    public void OpenCardSlot(int index, int unLockSlot)
    {
        _cardSlotArr[index].Open(unLockSlot);
    }

    public void SelectCard(int index)
    {
        _IsMyTurn = _userInfoArr[0]._MyIndex == index;

        _projectBoard.gameObject.SetActive(_userInfoArr[0]._MyIndex == index);

        _informText.gameObject.SetActive(_userInfoArr[0]._MyIndex != index);
        _informText.text = "카드 고르는중...";
    }

    public void SelectField(int userIndex)
    {
        if (_userInfoArr[0]._MyIndex != userIndex)
        {
            _informText.text = "분야 선택하는 중...";
            _informText.gameObject.SetActive(true);
        }
        else
        {
            _selectFieldObj.SetActive(true);
        }
    }

    public void GameOver(int selectCount)
    {
        _isGameOver = true;
        _physicsEffectCount = selectCount;
        _selectFieldObj.SetActive(true);
    }

    public void SelectFieldButton(int field)
    {
        ClientManager._instance.SelectField(field);

        if (_isGameOver && --_physicsEffectCount <= 0)
            ClientManager._instance.FinishGameOver();
    }

    public void OpenSelectCard(int[] cardIndex)
    {
        _selectCard.gameObject.SetActive(true);
        _selectCard.OpenSelectCard(cardIndex, "플라스크를 올려둘 카드를 선택하세요.");
    }

    public void OpenSelectOtherCard(int[] cardIndex)
    {
        _selectOtherCard.gameObject.SetActive(true);
        _selectOtherCard.OpenSelectOtherCard(cardIndex, "플라스크를 올려둘 카드를 선택하세요.");
    }

    public void ShowInformation(string info)
    {
        _informText.text = info;
        _informText.gameObject.SetActive(true);
    }

    //public void ExitButton()
    //{
    //    //TODO Send Packet To Server
    //}
}
