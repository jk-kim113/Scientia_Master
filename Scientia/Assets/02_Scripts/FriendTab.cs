using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FriendTab : MonoBehaviour, IPointerClickHandler
{
#pragma warning disable 0649
    [SerializeField]
    Color _nonSelectColor;
    [SerializeField]
    Color _selectColor;
#pragma warning restore

    Image _myImg;
    bool _isSelect;
    int _myIndex;

    CommunityUI _parent;

    private void Awake()
    {
        _myImg = GetComponent<Image>();
        _myImg.color = _nonSelectColor;
    }

    public void InitTab(CommunityUI parent, int index, bool isSelect)
    {
        _parent = parent;
        _myIndex = index;

        if(isSelect)
        {
            _isSelect = true;
            _myImg.color = _selectColor;
        }
    }

    public void OffMode()
    {
        _isSelect = false;
        _myImg.color = _nonSelectColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isSelect)
        {
            _isSelect = true;
            _myImg.color = _selectColor;
            _parent.SelectTab(_myIndex);
        }
    }
}
