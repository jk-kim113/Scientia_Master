using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyFriendObj : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Text _indexText;
    [SerializeField]
    Text _nickNameTxt;
    [SerializeField]
    Text _levelTxt;
#pragma warning restore

    public void InitMyFriend(int index, string nickName, int level)
    {
        _indexText.text = index.ToString();
        _nickNameTxt.text = nickName;
        _levelTxt.text = "Lv. " + level.ToString();
    }
}
