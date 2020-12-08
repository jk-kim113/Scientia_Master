using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateManager : FSM<BattleStateManager>
{
#pragma warning disable 0649
    [SerializeField]
    BattleInfo[] _battleInfoArr;
#pragma warning restore

    public BattleInfo[] _BattleInfoArr { get { return _battleInfoArr; } }

    const int _myIndex = 0;

    public void InitState()
    {
        InitState(this, BattleStateWait.Instance);
    }

    void Update()
    {
        FSMUpdate();
    }
}
