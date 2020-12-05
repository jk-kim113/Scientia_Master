using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInfo : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Text _nickNameText;
    [SerializeField]
    Text _levelText;
    [SerializeField]
    GameObject _masterIcon;
    [SerializeField]
    Image _stateImg;
    [SerializeField]
    GameObject _connectObj;
    [SerializeField]
    GameObject _disConnectObj;
    [SerializeField]
    Text _skillCubeTxt;
    [SerializeField]
    Text _flaskCubeTxt;
    [SerializeField]
    SkillTrack[] _skillTrackArr;
#pragma warning restore

    bool _isEmpty;
    public bool _IsEmpty { get { return _isEmpty; } }

    int _myIndex = -1;
    public int _MyIndex { get { return _myIndex; } }

    private void Awake()
    {
        _isEmpty = true;
    }

    private void Start()
    {
        _nickNameText.text = string.Empty;
        _levelText.text = string.Empty;
        _masterIcon.SetActive(false);

        ConnectState(!_isEmpty);
    }

    void ConnectState(bool isConnect)
    {
        _connectObj.SetActive(isConnect);
        _disConnectObj.SetActive(!isConnect);
    }

    public void ShowInfo(int myindex, string nickName, int level, bool isReady)
    {
        _myIndex = myindex;
        _isEmpty = false;
        _nickNameText.text = nickName;
        _levelText.text = level.ToString();
        ConnectState(!_isEmpty);
        ReadyState(isReady);

        _skillCubeTxt.text = "x 0";
        _flaskCubeTxt.text = "x 0";
    }

    public void ReadyState(bool isReady)
    {
        if(isReady)
            _stateImg.color = Color.red;
        else
            _stateImg.color = Color.white;
    }

    public void InitInfo()
    {
        for(int n = 0; n < _skillTrackArr.Length; n++)
        {
            _skillTrackArr[n].InitSkillCube();
        }

        _skillCubeTxt.text = "x 4";
    }

    public void ShowTurn(bool isTurn)
    {
        if (isTurn)
            _stateImg.color = Color.blue;
        else
            _stateImg.color = Color.white;
    }

    public void ShowMaster(bool isMaster)
    {
        _masterIcon.SetActive(isMaster);
    }

    public void ShowSkillCube(int skillcube, int field, int[] skillPos)
    {
        _skillCubeTxt.text = "x " + skillcube.ToString();
        _skillTrackArr[field].ShowSkillPos(skillPos);
    }

    public void ShowFlaskCube(int flaskCube)
    {
        _flaskCubeTxt.text = "x " + flaskCube.ToString();
    }
}
