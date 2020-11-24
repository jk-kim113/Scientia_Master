using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyCardUI : CardInfoUI, IPointerClickHandler, IPointerDownHandler
{
    public enum eCardState
    {
        Normal,
        UnLocking,
        UnLocked,
        WaitServer,
        End
    }

#pragma warning disable 0649
    [SerializeField]
    Image _cardGaugeBar;
    [SerializeField]
    GameObject _lockObj;
    [SerializeField]
    Image _lockImg;
#pragma warning restore

    eCardState _currnetCardState = eCardState.Normal;

    private void OnEnable()
    {
        _cardGaugeBar.fillAmount = 0;
    }

    private void Update()
    {
        if (_currnetCardState == eCardState.UnLocking)
        {
            _cardGaugeBar.fillAmount += 0.3f * Time.deltaTime;
            if (_cardGaugeBar.fillAmount > 0.95f)
            {
                _cardGaugeBar.fillAmount = 1.0f;
                _currnetCardState = eCardState.UnLocked;
                StartUnLock();
            }
        }
        else if (_currnetCardState == eCardState.Normal)
        {
            _cardGaugeBar.fillAmount -= 0.3f * Time.deltaTime;
            if (_cardGaugeBar.fillAmount < 0.01f)
            {
                _cardGaugeBar.fillAmount = 0.0f;
            }
        }
    }

    public void InitCardInfo(Sprite cardImg, string cardName, string cardInfo, bool isUnLock, int cardIndex)
    {
        base.InitCardInfo(cardImg, cardName, cardInfo, cardIndex);

        _currnetCardState = eCardState.Normal;
        _lockObj.SetActive(!isUnLock);

        if (UIManager._instance.GetWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI)._nowTabField != MyInfoUI.eTabField.MyDeck)
        {
            _actionButton.GetComponentInChildren<Text>().text = "마이덱에 추가";
            _actionButton.onClick.AddListener(() => AddMyDeck(_cardIndex));
        }
        else
        {
            _actionButton.GetComponentInChildren<Text>().text = "마이덱에서 제거";
            _actionButton.onClick.AddListener(() => DeleteMyDeck(_cardIndex));
        }
    }

    void AddMyDeck(int index)
    {
        if (!_lockObj.activeSelf)
            UIManager._instance.GetWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI).AddMyDeck(index);
        else
            SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.UnLockCard);
    }

    void DeleteMyDeck(int index)
    {
        UIManager._instance.GetWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI).DeleteMyDeck(index);
    }

    void StartUnLock()
    {
        ClientManager._instance.MyCardRelease(_cardIndex + 1);
        _currnetCardState = eCardState.WaitServer;
        StartCoroutine(WaitEffect());
    }

    public void EndUnlock()
    {
        UIManager._instance.GetWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI).Unlock(_cardIndex + 1);
        _currnetCardState = eCardState.End;

        StartCoroutine(UnLockEffect());
    }

    IEnumerator WaitEffect()
    {
        WaitForSeconds term = new WaitForSeconds(0.05f);

        while (_currnetCardState == eCardState.WaitServer)
        {
            yield return term;

            _lockImg.gameObject.GetComponent<RectTransform>().anchoredPosition = Random.insideUnitCircle * 8;
        }
    }

    IEnumerator UnLockEffect()
    {
        WaitForSeconds term = new WaitForSeconds(0.1f);

        while (_lockImg.color.a > 0.01)
        {
            yield return term;

            _lockImg.color -= new Color(0, 0, 0, 0.08f);
        }

        _lockImg.color = new Color(_lockImg.color.r, _lockImg.color.g, _lockImg.color.b, 0);
        _lockObj.SetActive(false);
        _cardGaugeBar.fillAmount = 0;

        _currnetCardState = eCardState.Normal;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currnetCardState == eCardState.Normal)
        {
            _cardGaugeBar.fillAmount = 0;
            UIManager._instance.Close(UIManager.eKindWindow.MyCardUI);
        }
        else if (_currnetCardState == eCardState.UnLocking)
        {
            _currnetCardState = eCardState.Normal;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //TODO Item Check

        if (eventData.pointerCurrentRaycast.gameObject.CompareTag("CardUnLockPos"))
        {
            _currnetCardState = eCardState.UnLocking;
        }
    }
}
