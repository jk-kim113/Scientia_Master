using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInfoUI : MonoBehaviour
{
    public enum eTabField
    {
        Physics = 1,
        Chemistry,
        Biology,
        Astronomy,
        MyDeck
    }

#pragma warning disable 0649
    [SerializeField]
    CardInvenTab[] _tabArr;
    [SerializeField]
    Transform _cardInvenTr;
#pragma warning restore

    GameObject _prefabCardObj;

    List<CardObj> _cardList = new List<CardObj>();
    List<int> _myDeckList = new List<int>();

    int _selectTabIndex;
    public eTabField _nowTabField { get { return (eTabField)_selectTabIndex; } }

    bool _isReady;
    public bool _IsReady { get { return _isReady; } }

    private void Start()
    {
        _isReady = false;
        for (int n = 0; n < _tabArr.Length; n++)
        {
            _tabArr[n].InitTab(this,
                TableManager._instance.Get(eTableType.CardTabData).ToS(n + 1, "KoreanName"), n + 1);
        }

        _prefabCardObj = ResourcePoolManager._instance.GetObj<GameObject>(ResourcePoolManager.eResourceKind.Prefab, "CardObj");

        for (int n = 0; n < TableManager._instance.Get(eTableType.CardData)._datas.Count; n++)
        {
            CardObj card = Instantiate(_prefabCardObj, _cardInvenTr).GetComponent<CardObj>();
            card.InitCard(
                ResourcePoolManager._instance.GetObj<Sprite>(
                    ResourcePoolManager.eResourceKind.Image, 
                    TableManager._instance.Get(eTableType.CardData).ToS(n + 1, "Name")),
                n);

            _cardList.Add(card);
            card.gameObject.SetActive(false);
        }

        _isReady = true;
    }

    public void SelectTab(int index)
    {
        _cardInvenTr.position = new Vector3(_cardInvenTr.position.x, 0, _cardInvenTr.position.z);
        if (_selectTabIndex != 0)
            _tabArr[_selectTabIndex - 1].OffMode();

        switch((eTabField)index)
        {
            case eTabField.Physics:
            case eTabField.Chemistry:
            case eTabField.Astronomy:
            case eTabField.Biology:

                if(_selectTabIndex == (int)eTabField.MyDeck)
                    MyDeckOnOff(false);
                else if (_selectTabIndex != 0)
                    CardOnOff(false);

                _selectTabIndex = index;

                CardOnOff(true);

                break;

            case eTabField.MyDeck:

                if (_selectTabIndex != 0)
                    CardOnOff(false);

                _selectTabIndex = index;
                MyDeckOnOff(true);

                break;
        }
    }

    public void Unlock(int[] cardIndex)
    {
        for (int n = 0; n < cardIndex.Length; n++)
        {
            if (cardIndex[n] == 0)
                break;

            _cardList[cardIndex[n] - 1].Unlock();
        }
    }

    public void Unlock(int cardIndex)
    {
        if(_cardList[cardIndex] != null)
            _cardList[cardIndex - 1].Unlock();
    }

    public void AddMyDeck(int index)
    {
        _myDeckList.Add(index);
    }

    public void DeleteMyDeck(int index)
    {
        _myDeckList.Remove(index);
        _cardList[index].gameObject.SetActive(false);
    }

    void CardOnOff(bool isOn)
    {
        for (int n = TableManager._instance.Get(eTableType.CardTabData).ToI(_selectTabIndex, "StartIndex");
                n <= TableManager._instance.Get(eTableType.CardTabData).ToI(_selectTabIndex, "EndIndex"); n++)
        {
            _cardList[n - 1].gameObject.SetActive(isOn);
        }
    }

    void MyDeckOnOff(bool isOn)
    {
        for (int n = 0; n < _myDeckList.Count; n++)
            _cardList[_myDeckList[n]].gameObject.SetActive(isOn);
    }

    public void BackButton()
    {
        UIManager._instance.Close(UIManager.eKindWindow.MyInfoUI);
    }
}
