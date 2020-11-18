using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviour
{
    public enum eModeType
    {
        나만의_덱,
        랜덤_덱,
        모두의_덱,
        AI대전,

        max
    }

    public enum eRuleType
    {
        빠른_대전,
        일반_대전,

        max
    }

#pragma warning disable 0649
    [SerializeField]
    InputField _nameField;
    [SerializeField]
    InputField _pwField;
    [SerializeField]
    Toggle _pwCheckToggle;
    [SerializeField]
    Dropdown _modeDrop;
    [SerializeField]
    Dropdown _ruleDrop;
#pragma warning restore

    private void OnEnable()
    {
        _nameField.text = string.Empty;
        _pwField.text = string.Empty;
        _pwCheckToggle.isOn = false;
        _pwField.interactable = _pwCheckToggle.isOn;
    }

    private void Start()
    {
        List<string> temp = new List<string>();

        for (int n = 0; n < (int)eModeType.max; n++)
            temp.Add(((eModeType)n).ToString());

        _modeDrop.ClearOptions();
        _modeDrop.AddOptions(temp);
        temp.Clear();

        for (int n = 0; n < (int)eRuleType.max; n++)
            temp.Add(((eRuleType)n).ToString());

        _ruleDrop.ClearOptions();
        _ruleDrop.AddOptions(temp);
    }

    private void Update()
    {
        _pwField.interactable = _pwCheckToggle.isOn;
    }

    public void OkButton()
    {
        if(!string.IsNullOrEmpty(_nameField.text))
        {
            if(_pwCheckToggle.isOn && string.IsNullOrEmpty(_pwField.text))
            {
                //TODO Sytem message Empty pw field
            }
            else
            {
                ClientManager._instance.CreateRoom(_nameField.text, _pwCheckToggle.isOn, _pwField.text, _modeDrop.itemText.text, _ruleDrop.itemText.text);
            }
        }
        else
        {
            //TODO Sytem Message Empty name field
        }
    }

    public void ExitButton()
    {
        UIManager._instance.Close(UIManager.eKindWindow.CreateRoomUI);
    }
}
