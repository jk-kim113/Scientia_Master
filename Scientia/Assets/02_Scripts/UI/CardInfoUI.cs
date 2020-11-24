using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Image _cardImg;
    [SerializeField]
    Text _infoText;
    [SerializeField]
    protected Button _actionButton;
#pragma warning restore

    protected int _cardIndex;

    public virtual void InitCardInfo(Sprite cardImg, string cardName, string cardInfo, int cardIndex)
    {
        _cardImg.sprite = cardImg;
        _infoText.text = cardName + "\n\n" + cardInfo;
        _cardIndex = cardIndex;

        _actionButton.onClick.RemoveAllListeners();
    }
}
