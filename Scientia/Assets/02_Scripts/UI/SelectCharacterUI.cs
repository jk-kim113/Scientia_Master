using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    AddCharacter[] _slotObjArr;
#pragma warning restore

    GameObject _prefabCharacterSlot;

    private void Start()
    {
        _prefabCharacterSlot = ResourcePoolManager._instance.GetObj<GameObject>(ResourcePoolManager.eResourceKind.Prefab, "CharacterSlot");

        for (int n = 0; n < _slotObjArr.Length; n++)
            _slotObjArr[n].Init(n);
    }

    public void SetSortOrder(int sort)
    {
        GetComponent<Canvas>().sortingOrder = sort;
    }

    public void ShowCharacter(string nickname, int avartarindex, int accountlevel, int slot)
    {
        GameObject obj = Instantiate(_prefabCharacterSlot, _slotObjArr[slot].gameObject.transform);
        CharacterSlot characSlot = obj.GetComponent<CharacterSlot>();
        characSlot.InitSlot(nickname, avartarindex, accountlevel);
    }
}
