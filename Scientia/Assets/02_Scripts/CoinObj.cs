using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinObj : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Text _coinTxt;
#pragma warning restore

    public void InitCoin(int coinCount)
    {
        _coinTxt.text = coinCount.ToString("N0");
    }
}
