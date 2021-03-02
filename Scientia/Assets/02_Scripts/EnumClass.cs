using System.Collections;
using System.Collections.Generic;

public class EnumClass
{
    public enum eBattleState
    {
        BattleWait,
        ScanCard,
        FirstPickCard,
        SelectAction,
        SelectProjectCard,
        RotateMyCard,
        WaitAction,
        SelectMyCard,
        SelectField
    }

    public enum eActionKind
    {
        GetCard,
        RotateCard
    }
}
