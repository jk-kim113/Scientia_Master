using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectBoard : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Transform _cardSpawnTr;
#pragma warning restore

    List<GameCardObj> _cardList = new List<GameCardObj>();
    GameObject _prefabGameCardObj;

    private void Start()
    {
        //TODO Add Table
        _prefabGameCardObj = ResourcePoolManager._instance.GetObj<GameObject>(ResourcePoolManager.eResourceKind.Prefab, "GameCardObj") as GameObject;
    }

    public void ShowPickedCard(int[] pickedCardArr)
    {
        if (pickedCardArr.Length > _cardList.Count)
            _cardList.Capacity = pickedCardArr.Length;

        for (int n = 0; n < pickedCardArr.Length; n++)
        {
            if(_cardList[n] == null)
            {
                GameCardObj card = Instantiate(_prefabGameCardObj, _cardSpawnTr).GetComponent<GameCardObj>();
                _cardList[n] = card;
            }

            _cardList[n].InitCard(
                    ResourcePoolManager._instance.GetObj<Sprite>(
                    ResourcePoolManager.eResourceKind.Image,
                    TableManager._instance.Get(eTableType.CardData).ToS(pickedCardArr[n], "Name")),
                    n,
                    2);
            _cardList[n].gameObject.SetActive(true);
        }
    }
}
