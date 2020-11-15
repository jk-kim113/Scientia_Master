    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterKind : MonoBehaviour, IPointerClickHandler
{
#pragma warning disable 0649
    [SerializeField]
    Image _characterImg;
    [SerializeField]
    Image _edgeImg;
#pragma warning restore

    CreateCharacterUI _parent;
    int _myIndex;
    bool _isClicked;

    public void InitKind(Sprite img, CreateCharacterUI parent, int index)
    {
        _characterImg.sprite = img;
        _edgeImg.color = Color.white;
        _parent = parent;
        _myIndex = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!_isClicked)
            _edgeImg.color = Color.red;
        else
            _edgeImg.color = Color.white;

        _isClicked = !_isClicked;
        _parent.SelectInfo(_myIndex);
    }

    public void ClickCancel()
    {
        _edgeImg.color = Color.white;
        _isClicked = false;
    }
}
