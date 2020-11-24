﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameCardObj : CardObj, IPointerClickHandler
{
#pragma warning disable 0649
    [SerializeField]
    Text _cardCntText;
#pragma warning restore

    int _cardCount;

    public void InitCard(Sprite img, int index, int cardCnt)
    {
        base.InitCard(img, index);

        _cardCount = cardCnt;

        _cardCntText.text = cardCnt.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BattleCardUI battleCardUI = UIManager._instance.OpenWnd<BattleCardUI>(UIManager.eKindWindow.BattleCardUI);
        battleCardUI.InitCardInfo(
            ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, TableManager._instance.Get(eTableType.CardData).ToS(_myIndex + 1, "Name")),
            TableManager._instance.Get(eTableType.CardData).ToS(_myIndex + 1, "KoreanName"),
            TableManager._instance.Get(eTableType.CardData).ToS(_myIndex + 1, "Effect"),
            _myIndex);
    }
}
