using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
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
        _nameText.text = nickname;

        _characImg.sprite = ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image,
            TableManager._instance.Get(eTableType.CharacterData).ToS(avartarindex, "EnglishName"));

        _levelText.text = accountlevel.ToString();
    }

    public void SelectCharacter()
    {
        // TODO Enter Lobby Scene
    }
}
