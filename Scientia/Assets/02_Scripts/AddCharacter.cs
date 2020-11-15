using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCharacter : MonoBehaviour
{
    int _myIndex;

    public void Init(int index)
    {
        _myIndex = index;
    }

    public void AddButton()
    {
        CreateCharacterUI createCharacUI = UIManager._instance.OpenWnd<CreateCharacterUI>(UIManager.eKindWindow.CreateCharacterUI);
        createCharacUI.InitUI(_myIndex);
    }
}
