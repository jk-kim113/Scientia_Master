using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateWait : FSMSingleton<BattleStateWait>, IFSMState<BattleStateManager>
{
    public void Enter(BattleStateManager e)
    {
        for (int n = 0; n < e._BattleInfoArr.Length; n++)
            e._BattleInfoArr[n].InitInfo();
    }

    public void Execute(BattleStateManager e)
    {

    }

    public void Exit(BattleStateManager e)
    {

    }
}
