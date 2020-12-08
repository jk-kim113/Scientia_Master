using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    CoinObj[] _coinObjArr;
    [SerializeField]
    Transform _shopTr;
    [SerializeField]
    Transform _invenTr;
#pragma warning restore

    List<MyItem> _myItemList = new List<MyItem>();

    private void Start()
    {
        GameObject prefabShopItem = ResourcePoolManager._instance.GetObj<GameObject>(ResourcePoolManager.eResourceKind.Prefab, "ShopItem") as GameObject;
        for(int n = 0; n < TableManager._instance.Get(eTableType.ItemData)._datas.Count; n++)
        {
            ShopItem shopItem = Instantiate(prefabShopItem, _shopTr).GetComponent<ShopItem>();
            shopItem.InitShopItem(n + 1);
        }
    }

    public void InitCoin(int[] coinArr)
    {
        for (int n = 0; n < _coinObjArr.Length; n++)
            _coinObjArr[n].InitCoin(coinArr[n]);
    }

    public void InitCoin(int coinIndex, int coinValue)
    {
        _coinObjArr[coinIndex].InitCoin(coinValue);
    }

    public void AddMyItmeToInven(int itemIndex, int itemCount)
    {
        for(int n = 0; n < _myItemList.Count; n++)
        {
            if (_myItemList[n]._MyItemIndex == itemIndex)
            {
                _myItemList[n].RenewItem(itemCount);
                return;
            }
        }

        GameObject prefabMyItem = ResourcePoolManager._instance.GetObj<GameObject>(ResourcePoolManager.eResourceKind.Prefab, "MyItem") as GameObject;
        MyItem myItem = Instantiate(prefabMyItem, _invenTr).GetComponent<MyItem>();
        myItem.InitItem(itemIndex, itemCount);
        _myItemList.Add(myItem);
    }
}
