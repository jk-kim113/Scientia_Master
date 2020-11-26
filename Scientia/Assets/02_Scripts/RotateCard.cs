using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RotateCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public enum eCardType
    {
        Lock,
        Empty,
        Rotatable,
        Selectable
    }

#pragma warning disable 0649
    [SerializeField]
    Image _cardImg;
    [SerializeField]
    Image _edgeImg;
#pragma warning restore

    eCardType _cardType = eCardType.Lock;

    bool _isPointerDown;
    RectTransform _myTr;

    float _downMouseX;
    float _currentZ;
    float _initZ;

    int _myRotateCount;
    public int _MyRotateCount { get { return _myRotateCount; } }

    int _myIndex;

    private void Start()
    {
        _myTr = GetComponent<RectTransform>();
        _initZ = _myTr.localEulerAngles.z;
    }

    private void Update()
    {
        if(_isPointerDown)
        {
            float interval = Input.mousePosition.x - _downMouseX;

            if(_currentZ - interval > _initZ)
                _myTr.localEulerAngles = new Vector3(_myTr.localEulerAngles.x, _myTr.localEulerAngles.y, _currentZ - interval);

            if(_myTr.localEulerAngles.z > 359 && _myTr.localEulerAngles.z < 360)
                _myTr.localEulerAngles = new Vector3(_myTr.localEulerAngles.x, _myTr.localEulerAngles.y, 359);

            ClientManager._instance.RotateInfo(_myIndex, _myTr.localEulerAngles.z);
        }
    }

    public void InitCard(Sprite cardImg, eCardType type, int index)
    {
        _cardType = type;

        _cardImg.sprite = cardImg;
        _myIndex = index;
    }

    public void SetRotation(float rotateValue)
    {
        _myTr.localEulerAngles = new Vector3(_myTr.localEulerAngles.x, _myTr.localEulerAngles.y, rotateValue);
    }

    public void ShowClick(bool isClick)
    {
        if (isClick)
            _edgeImg.color = new Color(_edgeImg.color.r, _edgeImg.color.g, _edgeImg.color.b, 1);
        else
            _edgeImg.color = new Color(_edgeImg.color.r, _edgeImg.color.g, _edgeImg.color.b, 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_cardType == eCardType.Rotatable && UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI)._IsMyTurn)
        {
            _isPointerDown = true;
            _downMouseX = Input.mousePosition.x;
            _currentZ = _myTr.localEulerAngles.z;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(_cardType == eCardType.Rotatable && UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI)._IsMyTurn)
        {
            _isPointerDown = false;

            float interval = _myTr.localEulerAngles.z - _currentZ;
            float rotateVal = 0;
            int turnCnt = 0;

            if (interval >= 0 && interval < 45)
            {
                rotateVal = 0;
                turnCnt = 0;
                _myRotateCount += 0;
            }   
            else if (interval >= 45 && interval < 135)
            {
                rotateVal = 90;
                turnCnt = 1;
                _myRotateCount += 1;
            }   
            else if (interval >= 135 && interval < 225)
            {
                rotateVal = 180;
                turnCnt = 2;
                _myRotateCount += 2;
            }   
            else if (interval >= 225 && interval < 315)
            {
                rotateVal = 270;
                turnCnt = 3;
                _myRotateCount += 3;
            }   
            else if (interval >= 315 && interval < 360)
            {
                rotateVal = 360;
                turnCnt = 4;
                _myRotateCount += 4;
            }   
            else if (interval <= 0 && interval > -45)
            {
                rotateVal = 0;
                turnCnt = 0;
                _myRotateCount += 0;
            }   
            else if (interval <= -45 && interval > -135)
            {
                rotateVal = -90;
                turnCnt = -1;
                _myRotateCount -= 1;
            }   
            else if (interval <= -135 && interval > -225)
            {
                rotateVal = -180;
                turnCnt = -2;
                _myRotateCount -= 2;
            }   
            else if (interval <= -225 && interval > -315)
            {
                rotateVal = -270;
                turnCnt = -3;
                _myRotateCount -= 3;
            }   
            else if (interval <= -315 && interval > 360)
            {
                rotateVal = -360;
                turnCnt = -4;
                _myRotateCount -= 4;
            }

            UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI)._NowTurn += turnCnt;

            int countCheck = UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI)._TurnCount
                                - UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI)._NowTurn;

            if (countCheck >= 0)
                _myTr.localEulerAngles = new Vector3(_myTr.localEulerAngles.x, _myTr.localEulerAngles.y, _currentZ + rotateVal);
            else
            {
                _myRotateCount += countCheck;
                UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI)._NowTurn += countCheck;
                _myTr.localEulerAngles = new Vector3(_myTr.localEulerAngles.x, _myTr.localEulerAngles.y, _currentZ + rotateVal + 90 * countCheck);
            }

            ClientManager._instance.RotateInfo(_myIndex, _myTr.localEulerAngles.z);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_cardType == eCardType.Selectable && UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI)._IsMyTurn)
        {
            UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ClickCompleteCard(_myIndex);
        }
    }
}
