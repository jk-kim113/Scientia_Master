using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectOtherCard : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Transform _spawnTr;
    [SerializeField]
    Text _infoTxt;
#pragma warning restore

    List<GameCardObj> _cardList = new List<GameCardObj>();

    public void OpenSelectOtherCard(int[] cardIndex, string info)
    {
        _infoTxt.text = info;

        GameObject prefabGameCardObj = ResourcePoolManager._instance.GetObj<GameObject>(ResourcePoolManager.eResourceKind.Prefab, "GameCardObj");

        for (int n = 0; n < _cardList.Count; n++)
            _cardList[n].gameObject.SetActive(false);

        for(int n = 0; n < cardIndex.Length; n++)
        {
            if (cardIndex[n] == 0)
                break;

            if(n < _cardList.Count)
            {
                _cardList[n].InitCard(ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image,
                TableManager._instance.Get(eTableType.CardData).ToS(cardIndex[n], CardData.Index.Name.ToString())),
                cardIndex[n]);
            }
            else
            {
                GameCardObj card = Instantiate(prefabGameCardObj, _spawnTr).GetComponent<GameCardObj>();
                card.InitCard(ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image,
                    TableManager._instance.Get(eTableType.CardData).ToS(cardIndex[n], CardData.Index.Name.ToString())),
                    cardIndex[n]);
                _cardList.Add(card);
            }
        }
    }
}
