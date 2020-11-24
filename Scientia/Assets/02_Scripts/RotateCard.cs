using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RotateCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum eCardType
    {
        Lock,
        Empty,
        Rotatable
    }

#pragma warning disable 0649
    [SerializeField]
    Image _cardImg;
#pragma warning restore

    bool _isPointerDown;
    RectTransform _myTr;

    float _downMouseX;
    float _currentZ;

    private void Start()
    {
        _myTr = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(_isPointerDown)
        {
            float interval = Input.mousePosition.x - _downMouseX;

            if (interval > 0)
                interval = 0;

            _myTr.localEulerAngles = new Vector3(_myTr.localEulerAngles.x, _myTr.localEulerAngles.y, _currentZ - interval);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
        _downMouseX = Input.mousePosition.x;
        _currentZ = _myTr.localEulerAngles.z;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;

        if (_myTr.localEulerAngles.z - _currentZ < 45)
            _myTr.localEulerAngles = new Vector3(_myTr.localEulerAngles.x, _myTr.localEulerAngles.y, _currentZ);
        else if (_myTr.localEulerAngles.z - _currentZ < 90)
            _myTr.localEulerAngles = new Vector3(_myTr.localEulerAngles.x, _myTr.localEulerAngles.y, _currentZ + 90);
    }
}
