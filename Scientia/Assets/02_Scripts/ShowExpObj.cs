using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowExpObj : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Image _expBar;
    [SerializeField]
    Text _levelExpTxt;
#pragma warning restore

    public void Init(int level, float exp)
    {
        _expBar.fillAmount = exp;
        _levelExpTxt.text = string.Format("Lv.{0} ({1:N}%)", level, exp * 100);
    }
}
