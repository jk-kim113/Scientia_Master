using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Image[] _cardImgArr;
    [SerializeField]
    Image _panelImg;
#pragma warning restore

    int _initOpenCnt = 2;

    bool[] _isOpenCardslot;

    private void Start()
    {
        _isOpenCardslot = new bool[_cardImgArr.Length];

        for (int n = 0; n < _cardImgArr.Length; n++)
        {
            if (n < _initOpenCnt)
                _cardImgArr[n].sprite = ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, "EmptyCardSlot");
            else
                _cardImgArr[n].sprite = ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, "CloseCardSlot");
        }
    }

    public void Open(int unLockSlot)
    {
        _cardImgArr[unLockSlot - 1].sprite = ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, "EmptyCardSlot");
    }

    public void ShowTurn(bool isTurn)
    {
        if(isTurn)
            _panelImg.color = Color.blue;
        else
            _panelImg.color = Color.white;
    }

    public void ShowAddCard(int slotIndex, int cardIndex)
    {
        _cardImgArr[slotIndex].sprite = ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image,
            TableManager._instance.Get(eTableType.CardData).ToS(cardIndex, "Name"));
    }

    public void DeleteCard(int slotIndex)
    {
        _cardImgArr[slotIndex].sprite = ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, "EmptyCardSlot");
    }
}
