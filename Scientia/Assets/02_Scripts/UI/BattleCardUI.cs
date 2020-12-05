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

        if (UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI)._IsMyTurn)
        {   
            _actionButton.GetComponentInChildren<Text>().text = "슬롯에 추가";
            if(BattleManager._instance._nowBattleState != BattleManager.eBattleState.Progress)
                _actionButton.onClick.AddListener(() => AddToCardSlot());
            else
                _actionButton.onClick.AddListener(() => AddCardInProgress());
            _actionButton.gameObject.SetActive(true);
        }
        else
            _actionButton.gameObject.SetActive(false);
    }

    void AddToCardSlot()
    {
        ClientManager._instance.PickCard(_cardIndex);
        UIManager._instance.Close(UIManager.eKindWindow.BattleCardUI);
    }

    void AddCardInProgress()
    {
        ClientManager._instance.PickCardInProgress(_cardIndex);
        UIManager._instance.Close(UIManager.eKindWindow.BattleCardUI);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager._instance.Close(UIManager.eKindWindow.BattleCardUI);
    }
}
