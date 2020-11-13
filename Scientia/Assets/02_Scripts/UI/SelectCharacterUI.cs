using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    GameObject[] _slotObjArr;
#pragma warning restore

    public void SetSortOrder(int sort)
    {
        GetComponent<Canvas>().sortingOrder = sort;
    }

    public void ShowCharacter(string nickname, int avartarindex, int accountlevel, int slot)
    {
        GameObject obj = ResourcePoolManager._instance.GetObj<GameObject>(ResourcePoolManager.eResourceKind.Prefab, "CharacterSlotUI");
        Instantiate(obj, _slotObjArr[slot].transform);
        CharacterSlotUI characSlotUI = obj.GetComponent<CharacterSlotUI>();
    }
}
