using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    InputField _idField;
    [SerializeField]
    InputField _pwField;
#pragma warning restore

    private void Update()
    {
        if (_idField.isFocused)
        {
            if(TouchScreenKeyboard.isSupported && !TouchScreenKeyboard.visible)
                TouchScreenKeyboard.Open("", TouchScreenKeyboardType.ASCIICapable);
        }
            
    }

    public void LogInButton()
    {
        if (!string.IsNullOrEmpty(_idField.text))
        {
            if (!string.IsNullOrEmpty(_pwField.text))
                ClientManager._instance.LogIn(_idField.text, _pwField.text);
            else
            {
                SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.Pw_NonEnter);
            }
        }
        else
        {
            SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.ID_NonEnter);
        }
    }

    public void EnrollButton()
    {
        EnrollUI enrolUI = UIManager._instance.OpenWnd<EnrollUI>(UIManager.eKindWindow.EnrollUI);
        enrolUI.Init();
    }
}
