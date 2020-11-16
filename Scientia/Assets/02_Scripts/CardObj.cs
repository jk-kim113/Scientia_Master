using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObj : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Image _cardImg;
    [SerializeField]
    GameObject _lockPanel;
#pragma warning restore

    bool _isUnlock;

    public void InitCard(Sprite img)
    {
        _cardImg.sprite = img;
    }

    public void Unlock()
    {
        _isUnlock = true;
        _lockPanel.SetActive(false);
    }
}
