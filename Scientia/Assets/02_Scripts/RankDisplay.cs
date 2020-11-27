using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankDisplay : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Text _nickNameText;
    [SerializeField]
    Text _scoreText;
#pragma warning restore

    public void InitDisplay(string nickName, string score)
    {
        _nickNameText.text = nickName;
        _scoreText.text = score;
    }
}
