using UnityEngine;
using UnityEngine.UI;

public class EnrollUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    InputField _idField;
    [SerializeField]
    InputField _pwField;
#pragma warning restore

    bool _isCheckOverlap = false;
    public bool _IsCheckOverlap { set { _isCheckOverlap = value; } }

    public void Init()
    {
        _idField.text = string.Empty;
        _pwField.text = string.Empty;
    }

    public void OverlapCheck()
    {
        if(!string.IsNullOrEmpty(_idField.text))
        {
            if(CheckRule.IsValidID(_idField.text))
            {
                ClientManager._instance.OverlapCheck_ID(_idField.text);
            }
            else
                SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.ID_Rule);
        }
        else
            SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.ID_NonEnter);
    }

    public void OkButton()
    {
        if(!string.IsNullOrEmpty(_idField.text))
        {
            if(!string.IsNullOrEmpty(_pwField.text))
            {
                if (_isCheckOverlap)
                {
                    if(CheckRule.IsValidPassword(_pwField.text))
                    {
                        ClientManager._instance.EnrollTry(_idField.text, _pwField.text);
                    }
                    else
                        SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.Pw_Rule);
                }
                else
                    SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.ID_Overlap_NonCheck);
            }
            else
                SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.Pw_NonEnter);
        }
        else
            SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.ID_NonEnter);
    }

    public void CancelButton()
    {
        UIManager._instance.Close(UIManager.eKindWindow.EnrollUI);
    }
}
