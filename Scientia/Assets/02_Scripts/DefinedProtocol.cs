﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefinedProtocol
{
    public enum eFromClient
    {
        LogInTry,
        OverlapCheck_ID,
        OverlapCheck_NickName,
        EnrollTry,
        GetMyCharacterInfo,
        CreateCharacter,
        GetMyInfoData,
        AddReleaseCard,
        CreateRoom,
        GetRoomList,

        ConnectionTerminate,

        max
    }

    public enum eToClient
    {
        LogInResult,
        ResultOverlap_ID,
        ResultOverlap_NickName,
        EnrollResult,
        CharacterInfo,
        EndCharacterInfo,
        EndCreateCharacter,
        ShowMyInfo,
        CompleteAddReleaseCard,
        CompleteCreateRoom,
        ShowRoomList,

        max
    }
}
