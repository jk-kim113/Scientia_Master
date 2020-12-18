using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCard : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Image[] _cardImgArr;
    [SerializeField]
    Text _infoTxt;
#pragma warning restore

    int[] _cardIndexArr;

    private void Start()
    {
        _cardIndexArr = new int[_cardImgArr.Length];
    }

    public void OpenSelectCard(int[] cardIndex, string info)
    {
        _infoTxt.text = info;

        for (int n = 0; n < _cardImgArr.Length; n++)
        {
            if(cardIndex[n] != 0)
            {
                _cardImgArr[n].sprite = ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image, 
                    TableManager._instance.Get(eTableType.CardData).ToS(cardIndex[n], CardData.Index.Name.ToString()));
                _cardImgArr[n].gameObject.SetActive(true);
            }
            else
                _cardImgArr[n].gameObject.SetActive(false);
        }
    }

    public void ClickSelectCard(int index)
    {
        if(gameObject.activeSelf)
        {
            ClientManager._instance.SelectCardResult(_cardIndexArr[index]);
        }
    }
}
