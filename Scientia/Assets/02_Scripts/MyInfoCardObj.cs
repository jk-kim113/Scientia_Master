using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInfoCardObj : CardObj
{
#pragma warning disable 0649
    [SerializeField]
    GameObject _lockPanel;
#pragma warning restore

    public override void InitCard(Sprite img, int index)
    {
        base.InitCard(img, index);

        _isUnlock = false;
    }

    public void Unlock()
    {
        _isUnlock = true;
        _lockPanel.SetActive(false);
    }
}
