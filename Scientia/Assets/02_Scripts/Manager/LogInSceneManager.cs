using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogInSceneManager : MonoBehaviour
{
    private void Start()
    {
        UIManager._instance.OpenWnd<LogInUI>(UIManager.eKindWindow.LogInUI);
    }
}
