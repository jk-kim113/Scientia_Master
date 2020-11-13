using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlotUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Image _characImg;
    [SerializeField]
    Text _nameText;
    [SerializeField]
    Text _levelText;
#pragma warning restore

    public void InitSlot(string nickname, int avartarindex, int accountlevel)
    {
        Debug.Log("asd");
    }

    public void SelectCharacter()
    {

    }
}
