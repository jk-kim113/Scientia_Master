using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    static BattleManager _uniqueInstance;
    public static BattleManager _instance { get { return _uniqueInstance; } }

    public enum eBattleState
    {
        Ready,
        Progress,
        End,
        Result
    }

    public enum eReadyState
    {
        GameWait,
        GameStart,
        ReadCard = 1,
        PickCard,
        SelectionAction,

        WaitServer
    }

    int _roomNumber;
    public int _RoomNumber { get { return _roomNumber; } set { _roomNumber = value; } }

    float _timeGoal;
    eBattleState _currentBattleState;
    eReadyState _currentReadyState;
    public eReadyState _nowReadyState { get { return _currentReadyState; } }

    private void Awake()
    {
        _uniqueInstance = this;

        _currentBattleState = eBattleState.Ready;

        UIManager._instance.Close(UIManager.eKindWindow.CreateRoomUI);
        UIManager._instance.Close(UIManager.eKindWindow.LobbyUI);

        UIManager._instance.OpenWnd<BattleUI>(UIManager.eKindWindow.BattleUI);
    }

    private void Start()
    {
        ReadyStateChange(eReadyState.GameWait);
    }

    private void Update()
    {
        switch(_currentReadyState)
        {
            case eReadyState.ReadCard:

                _timeGoal -= Time.deltaTime;
                if(_timeGoal < 0)
                {
                    ReadyStateChange(eReadyState.WaitServer);
                    ClientManager._instance.FinishReadCard();
                }

                UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).ShowReadCardTime((int)_timeGoal);

                break;
        }
    }

    public void ReadyStateChange(eReadyState state)
    {
        _currentReadyState = state;
        UIManager._instance.GetWnd<BattleUI>(UIManager.eKindWindow.BattleUI).StateChange(_currentReadyState);

        switch (state)
        {
            case eReadyState.ReadCard:

                _timeGoal = 30.0f;

                break;
        }
    }
}
