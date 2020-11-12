using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWindow : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Image _loadingBar;
#pragma warning restore

    public void OpenLoadingWnd()
    {
        _loadingBar.fillAmount = 0;
    }

    public void SettingLoadRate(float rate)
    {
        _loadingBar.fillAmount = rate;
    }
}
