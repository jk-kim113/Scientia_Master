using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefinedProtocol
{
    public enum eFromClient
    {
        LogInTry,
        OverlapCheck_ID,
        EnrollTry,
        GetMyCharacterInfo,

        max
    }

    public enum eToClient
    {
        LogInResult,
        ResultOverlap_ID,
        EnrollResult,
        CharacterInfo,
        EndCharacterInfo,

        max
    }
}
