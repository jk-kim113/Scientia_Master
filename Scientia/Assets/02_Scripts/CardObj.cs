using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CardObj : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Image _cardImg;
#pragma warning restore

    protected bool _isUnlock;
    protected int _myIndex;

    public virtual void InitCard(Sprite img, int index)
    {
        _cardImg.sprite = img;
        _myIndex = index;
    }
}
