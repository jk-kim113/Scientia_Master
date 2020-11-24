using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class BattleCardUI : CardInfoUI, IPointerClickHandler
{
    public override void InitCardInfo(Sprite cardImg, string cardName, string cardInfo, int cardIndex)
    {
        base.InitCardInfo(cardImg, cardName, cardInfo, cardIndex);

        if (BattleManager._instance._nowReadyState == BattleManager.eReadyState.PickCard)
        {
            _actionButton.GetComponentInChildren<Text>().text = "슬롯에 추가";
            _actionButton.onClick.AddListener(() => AddToCardSlot());
            _actionButton.gameObject.SetActive(true);
        }
        else
            _actionButton.gameObject.SetActive(false);
    }

    void AddToCardSlot()
    {
        ClientManager._instance.PickCard(_cardIndex);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager._instance.Close(UIManager.eKindWindow.BattleCardUI);
    }
}
