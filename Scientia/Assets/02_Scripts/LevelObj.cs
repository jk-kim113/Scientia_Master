using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelObj : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Image _expBar;
    [SerializeField]
    Text _levelTxt;
#pragma warning restore

    public void InitLevelObj(int level, int exp)
    {
        _levelTxt.text = string.Format("Lv.{0} ({1:N})", level, exp * 100);
        _expBar.fillAmount = exp;
    }
}
