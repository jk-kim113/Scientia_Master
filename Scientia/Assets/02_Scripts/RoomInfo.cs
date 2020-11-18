using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfo : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Text _roomNumberText;
    [SerializeField]
    Text _nameText;
    [SerializeField]
    GameObject _lockState;
    [SerializeField]
    Text _memberCntText;
    [SerializeField]
    Text _modeText;
    [SerializeField]
    Text _ruleText;
    [SerializeField]
    Text _isPlayText;
#pragma warning restore

    int _myRoomNumber;

    public void InitRoomInfo(int roomNumber, string name, bool isLock, int currentMemberCnt, int maxMemberCnt, string mode, string rule, string isPlay)
    {
        _myRoomNumber = roomNumber;
        _roomNumberText.text = roomNumber.ToString();
        _nameText.text = name;
        _lockState.SetActive(isLock);
        _memberCntText.text = string.Format("{0} / {1}", currentMemberCnt, maxMemberCnt);
        _modeText.text = mode;
        _ruleText.text = rule;
        _isPlayText.text = isPlay;
    }

    public void EnterRoomButton()
    {

    }
}
