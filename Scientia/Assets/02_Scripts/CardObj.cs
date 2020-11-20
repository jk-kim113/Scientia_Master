using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardObj : MonoBehaviour, IPointerClickHandler
{
#pragma warning disable 0649
    [SerializeField]
    Image _cardImg;
#pragma warning restore

    protected bool _isUnlock;
    int _myIndex;

    public virtual void InitCard(Sprite img, int index)
    {
        _cardImg.sprite = img;
        _myIndex = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CardInfoUI cardInfoUI = UIManager._instance.OpenWnd<CardInfoUI>(UIManager.eKindWindow.CardInfoUI);
        cardInfoUI.InitCardInfo(
            ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, TableManager._instance.Get(eTableType.CardData).ToS(_myIndex + 1, "Name")),
            TableManager._instance.Get(eTableType.CardData).ToS(_myIndex + 1, "KoreanName"),
            TableManager._instance.Get(eTableType.CardData).ToS(_myIndex + 1, "Effect"),
            _isUnlock,
            _myIndex);
    }
}
