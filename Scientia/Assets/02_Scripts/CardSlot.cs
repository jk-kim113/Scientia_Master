using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Image[] _cardImgArr;
#pragma warning restore

    int _initOpenCnt = 2;

    bool[] _isOpenCardslot;

    private void Start()
    {
        _isOpenCardslot = new bool[_cardImgArr.Length];

        for (int n = 0; n < _cardImgArr.Length; n++)
        {
            //TODO All Close State
        }

        for(int n = 0; n < _initOpenCnt; n++)
        {
            //TODO InitOpen
        }
    }

    public void Open()
    {
        int emptySlot = 0;

        if(IsEmptySlot(out emptySlot))
        {
            //TODO Open
        }
        else
        {
            //TODO System Message no empty slot
        }
    }

    bool IsEmptySlot(out int emptySlot)
    {
        emptySlot = -1;
        for (int n = 0; n < _isOpenCardslot.Length; n++)
        {
            if (_isOpenCardslot[n])
            {
                emptySlot = n;
                return true;
            }   
        }

        return false;
    }
}
