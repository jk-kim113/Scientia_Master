﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemMessageUI : MonoBehaviour
{
    public enum eSystemMessageType
    {
        LogIn_Fail,
        ID_Overlap_NonCheck,
        ID_Overlap,
        NickName_Overlap,
        NickName_Overlap_NonCheck,
        Enroll_Success,
        Enroll_Fail,
        Character_Fail,
        ID_NonEnter,
        Pw_NonEnter,
        NickName_NonEnter,
        Character_NonSelect,
        ID_Rule,
        Pw_Rule,
        NickName_Rule,
    }

#pragma warning disable 0649
    [SerializeField]
    Text _systemMessageText;
#pragma warning restore

    void ShowMessage(string msg)
    {
        _systemMessageText.text = msg;
    }

    public void OkButton()
    {
        UIManager._instance.Close(UIManager.eKindWindow.SystemMessageUI);
    }

    public static void Open(eSystemMessageType type)
    {
        SystemMessageUI systemMsgUI = UIManager._instance.OpenWnd<SystemMessageUI>(UIManager.eKindWindow.SystemMessageUI);
        systemMsgUI.ShowMessage(TableManager._instance.Get(eTableType.SystemMessageData).ToS((int)type + 1, "Message"));
    }
}
