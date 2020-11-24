using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyInfoCardObj : CardObj, IPointerClickHandler
{
#pragma warning disable 0649
    [SerializeField]
    GameObject _lockPanel;
#pragma warning restore

    public override void InitCard(Sprite img, int index)
    {
        base.InitCard(img, index);

        _isUnlock = false;
    }

    public void Unlock()
    {
        _isUnlock = true;
        _lockPanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MyCardUI myCardUI = UIManager._instance.OpenWnd<MyCardUI>(UIManager.eKindWindow.MyCardUI);
        myCardUI.InitCardInfo(
            ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, TableManager._instance.Get(eTableType.CardData).ToS(_myIndex + 1, "Name")),
            TableManager._instance.Get(eTableType.CardData).ToS(_myIndex + 1, "KoreanName"),
            TableManager._instance.Get(eTableType.CardData).ToS(_myIndex + 1, "Effect"),
            _isUnlock,
            _myIndex);
    }
}
