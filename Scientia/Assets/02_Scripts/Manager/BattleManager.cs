using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    static BattleManager _uniqueInstance;
    public static BattleManager _instance { get { return _uniqueInstance; } }

    public enum eBattleState
    {
        GameWait,
        ReadCard,
        GamePlaying,

        
        WaitServer
    }

    int _roomNumber;
    public int _RoomNumber { get { return _roomNumber; } set { _roomNumber = value; } }


    float _timeGoal;
    eBattleState _currentBattleState;

    private void Awake()
    {
        _uniqueInstance = this;

        UIManager._instance.Close(UIManager.eKindWindow.CreateRoomUI);
        UIManager._instance.Close(UIManager.eKindWindow.LobbyUI);

        UIManager._instance.OpenWnd<BattleUI>(UIManager.eKindWindow.BattleUI);
    }

    private void Start()
    {
        StateChange(eBattleState.GameWait);
    }

    private void Update()
    {
        switch(_currentBattleState)
        {
            case eBattleState.ReadCard:

                _timeGoal -= Time.deltaTime;
                if(_timeGoal < 0)
                {
                    //TODO Inform Server
                    _currentBattleState = eBattleState.WaitServer;
                }

                break;
        }
    }

    void StateChange(eBattleState state)
    {
        _currentBattleState = state;
        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).StateChange(_currentBattleState);

        switch (state)
        {
            case eBattleState.GameWait:

                break;

            case eBattleState.ReadCard:

                _timeGoal = 30.0f;
                //TODO Show UI 

                break;
        }
    }
}
