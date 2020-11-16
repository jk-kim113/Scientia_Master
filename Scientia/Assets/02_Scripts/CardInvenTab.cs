using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardInvenTab : MonoBehaviour, IPointerClickHandler
{
#pragma warning disable 0649
    [SerializeField]
    Color _nonSelectColor;
    [SerializeField]
    Color _selectColor;
    [SerializeField]
    Text _myFieldText;
#pragma warning restore

    Image _myImg;
    bool _isSelect;
    int _myIndex;

    MyInfoUI _parent;

    private void Awake()
    {
        _myImg = GetComponent<Image>();
    }

    private void Start()
    {
        _myImg.color = _nonSelectColor;
    }

    public void InitTab(MyInfoUI parent, string field, int index)
    {
        _parent = parent;
        _myFieldText.text = field;
        _myIndex = index;
    }

    public void OffMode()
    {
        _isSelect = false;
        _myImg.color = _nonSelectColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!_isSelect)
        {
            _isSelect = true;
            _myImg.color = _selectColor;
            _parent.SelectTab(_myIndex);
        }
    }
}
