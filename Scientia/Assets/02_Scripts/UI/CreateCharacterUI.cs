using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacterUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Text _effectText;
    [SerializeField]
    Text _explainText;
    [SerializeField]
    Transform _characterKindTr;
    [SerializeField]
    InputField _nickNameField;
#pragma warning restore

    List<CharacterKind> _cKindList = new List<CharacterKind>();
    int _selectedIndex;
    int _characterSlot;

    private void OnEnable()
    {
        _characterKindTr.position = new Vector3(_characterKindTr.position.x, 0, _characterKindTr.position.z);

        if (_selectedIndex != 0)
            _cKindList[_selectedIndex - 1].ClickCancel();

        _selectedIndex = 0;
        _effectText.text = string.Empty;
        _explainText.text = string.Empty;
        _nickNameField.text = string.Empty;
    }

    private void Start()
    {
        for (int n = 0; n < TableManager._instance.Get(eTableType.CharacterData)._datas.Count; n++)
        {
            GameObject go = ResourcePoolManager._instance.GetObj<GameObject>(ResourcePoolManager.eResourceKind.Prefab, "CharacterKind");
            CharacterKind cKind = Instantiate(go, _characterKindTr).GetComponent<CharacterKind>();
            cKind.InitKind(ResourcePoolManager._instance.GetObj<Sprite>(ResourcePoolManager.eResourceKind.Image,
                TableManager._instance.Get(eTableType.CharacterData).ToS(n + 1, "EnglishName")), this, n + 1);
            _cKindList.Add(cKind);
        }
    }

    public void InitUI(int characterSlot)
    {
        _characterSlot = characterSlot;
    }

    public void SelectInfo(int index)
    {
        if(_selectedIndex == index)
        {
            _cKindList[_selectedIndex - 1].ClickCancel();
            _selectedIndex = 0;
            _effectText.text = string.Empty;
            _explainText.text = string.Empty;
        }
        else
        {
            if(_selectedIndex != 0)
                _cKindList[_selectedIndex - 1].ClickCancel();

            _selectedIndex = index;
            _effectText.text = TableManager._instance.Get(eTableType.CharacterData).ToS(_selectedIndex, "EffectText");

            string temp = string.Empty;
            int start = TableManager._instance.Get(eTableType.CharacterData).ToI(_selectedIndex, "ExplainStart");
            int end = TableManager._instance.Get(eTableType.CharacterData).ToI(_selectedIndex, "ExplainEnd");
            for (int n = start; n <= end; n++)
            {
                temp += TableManager._instance.Get(eTableType.CharacterExplainData).ToS(n, "Explain");
            }
            _explainText.text = temp;
        }
    }

    public void OkButton()
    {
        if (!string.IsNullOrEmpty(_nickNameField.text))
        {
            if (_selectedIndex != 0)
            {
                if (CheckRule.IsValidNickName(_nickNameField.text))
                {
                    ClientManager._instance.OverlapCheck_NickName(_nickNameField.text);
                }
                else
                    SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.NickName_Rule);
            }
            else
                SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.Character_NonSelect);
        }
        else
            SystemMessageUI.Open(SystemMessageUI.eSystemMessageType.NickName_NonEnter);
    }

    public void BackButton()
    {
        UIManager._instance.Close(UIManager.eKindWindow.CreateCharacterUI);
    }

    public void CreateCharacter()
    {
        ClientManager._instance.CreateCharacter(_nickNameField.text, _selectedIndex, _characterSlot);
    }
}
