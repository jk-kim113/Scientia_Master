using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Image _itemIconImg;
    [SerializeField]
    Text _itemNameTxt;
    [SerializeField]
    Text _coinTxt;
#pragma warning restore

    int _myItemIndex;

    public void InitShopItem(int itemIndex)
    {
        _myItemIndex = itemIndex;

        _itemIconImg.sprite = ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image,
            TableManager._instance.Get(eTableType.ItemData).ToS(_myItemIndex, ItemData.Index.Name.ToString()));

        _itemNameTxt.text = TableManager._instance.Get(eTableType.ItemData).ToS(_myItemIndex, ItemData.Index.KoreanName.ToString());
        _coinTxt.text = TableManager._instance.Get(eTableType.ItemData).ToS(_myItemIndex, ItemData.Index.CoinKind.ToString())
            + "x " + TableManager._instance.Get(eTableType.ItemData).ToS(_myItemIndex, ItemData.Index.Coin.ToString());
    }

    public void BuyButton()
    {
        ClientManager._instance.BuyItem(_myItemIndex);
    }
}
