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

        max
    }

    public enum eToClient
    {
        LogInResult,

        max
    }
}
