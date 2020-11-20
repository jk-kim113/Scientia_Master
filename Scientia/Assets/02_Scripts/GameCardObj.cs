using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCardObj : CardObj
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
    }
}
