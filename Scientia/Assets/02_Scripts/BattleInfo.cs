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
#pragma warning restore

    bool _isEmpty;
    public bool _IsEmpty { get { return _isEmpty; } }

    int _myIndex;
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
    }

    public void ShowInfo(int myindex, string nickName, int level, bool isReady)
    {
        _myIndex = myindex;
        _isEmpty = false;
        _nickNameText.text = nickName;
        _levelText.text = level.ToString();
        ReadyState(isReady);
    }

    public void ReadyState(bool isReady)
    {
        if(isReady)
            _stateImg.color = Color.yellow;
        else
            _stateImg.color = Color.white;
    }

    public void ShowMaster(bool isMaster)
    {
        _masterIcon.SetActive(isMaster);
    }
}
