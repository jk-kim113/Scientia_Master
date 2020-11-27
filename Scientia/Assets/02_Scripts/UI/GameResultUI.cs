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

#pragma warning disable 0649
    [SerializeField]
    Text _resultText;
    [SerializeField]
    GameObject[] _showObjArr;
    [SerializeField]
    Image _firstUserIconImg;
    [SerializeField]
    RankDisplay[] _rankDisplayArr;
#pragma warning restore

    eShowType _currentShowType;

    private void OnEnable()
    {
        ChangeShowType(eShowType.Rank);
    }

    void ChangeShowType(eShowType type)
    {
        _currentShowType = type;

        for (int n = 0; n < (int)eShowType.max; n++)
            _showObjArr[n].SetActive(n == (int)type);
    }

    public void NextButton()
    {
        ChangeShowType(eShowType.Exp);
    }
}
