using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResultUI : MonoBehaviour
{
    public enum eShowType
    {
        Rank,
        Exp,

        max
    }

    public enum eExpType
    {
        Account,
        Physics,
        Chemistry,
        Biology,
        Astronomy,

        max
    }

#pragma warning disable 0649
    [SerializeField]
    Text _resultText;
    [SerializeField]
    GameObject[] _showObjArr;
    [SerializeField]
    Image _firstUserIconImg;
    [SerializeField]
    RankDisplay[] _rankDisplayArr;
    [SerializeField]
    ShowExpObj[] _showExpObjArr;
#pragma warning restore

    eShowType _currentShowType;

    int _accountExp;
    int _physicsExp;
    int _chemistryExp;
    int _biologyExp;
    int _astronomyExp;

    private void OnEnable()
    {
        _currentShowType = eShowType.Rank;
        ChangeShowType(_currentShowType);
    }

    public void InitResult(bool isWin, int firstCharacIndex, string nickNameArr, int[] rankScore, int accountExp, int physicsExp, int chemistryExp, int biologyExp, int astronomyExp)
    {
        _resultText.text = isWin ? "승리" : "패배";
        _resultText.color = isWin ? Color.blue : Color.red;

        _firstUserIconImg.sprite = ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image,
            TableManager._instance.Get(eTableType.ImageData).ToS(firstCharacIndex, ImageData.Index.Name.ToString()));

        string[] nickName = nickNameArr.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        for (int n = 0; n < _rankDisplayArr.Length; n++)
            _rankDisplayArr[n].gameObject.SetActive(false);

        for (int n = 0; n < nickName.Length; n++)
        {
            _rankDisplayArr[n].InitDisplay(nickName[n], rankScore[n].ToString());
            _rankDisplayArr[n].gameObject.SetActive(true);
        }

        _accountExp = accountExp;
        _physicsExp = physicsExp;
        _chemistryExp = chemistryExp;
        _biologyExp = biologyExp;
        _astronomyExp = astronomyExp;
    }

    void ChangeShowType(eShowType type)
    {
        _currentShowType = type;

        for (int n = 0; n < (int)eShowType.max; n++)
            _showObjArr[n].SetActive(n == (int)type);
    }

    public void NextButton()
    {
        for(int n = 0; n < _showExpObjArr.Length; n++)
        {
            if(n == 0)
            {
                _showExpObjArr[n].Init(UIManager._instance.GetWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI)._LevelArr[n],
                UIManager._instance.GetWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI)._ExpArr[n] /
                TableManager._instance.Get(eTableType.AccountExpData).ToI(UIManager._instance.GetWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI)._LevelArr[n],
                AccountExpData.Index.Exp.ToString()));
            }
            else
            {
                _showExpObjArr[n].Init(UIManager._instance.GetWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI)._LevelArr[n],
                UIManager._instance.GetWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI)._ExpArr[n] /
                TableManager._instance.Get(eTableType.FieldExpData).ToI(UIManager._instance.GetWnd<MyInfoUI>(UIManager.eKindWindow.MyInfoUI)._LevelArr[n],
                FieldExpData.Index.Exp.ToString()));
            }
        }

        ChangeShowType(eShowType.Exp);

        //StartCoroutine(ShowExp());
    }

    //IEnumerator ShowExp()
    //{

    //}
}
