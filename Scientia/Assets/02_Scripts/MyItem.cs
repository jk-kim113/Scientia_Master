using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyItem : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Image _itemIconImg;
    [SerializeField]
    Text _itemNameTxt;
    [SerializeField]
    Text _itemNumTxt;
#pragma warning restore

    int _myItemIndex;
    public int _MyItemIndex { get { return _myItemIndex; } }

    public void InitItem(int itemIndex, int itemCount)
    {
        _myItemIndex = itemIndex;
        _itemIconImg.sprite = ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image,
            TableManager._instance.Get(eTableType.ItemData).ToS(itemIndex, ItemData.Index.Name.ToString()));

        _itemNameTxt.text = TableManager._instance.Get(eTableType.ItemData).ToS(itemIndex, ItemData.Index.KoreanName.ToString());
        RenewItem(itemCount);
    }

    public void RenewItem(int itemCount)
    {
        _itemNumTxt.text = "x " + itemCount.ToString();
    }
}
