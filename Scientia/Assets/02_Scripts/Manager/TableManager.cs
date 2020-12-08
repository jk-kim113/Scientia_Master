using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTableType
{
    PrefabData,
    SystemMessageData,
    ImageData,
    CharacterData,
    CharacterExplainData,
    CardData,
    CardTabData,
    ItemData,

    max
}

public class TableManager : TSingleton<TableManager>
{
    Dictionary<eTableType, TableBase> _tableData = new Dictionary<eTableType, TableBase>();

    protected override void Init()
    {
        base.Init();
    }

    TableBase Load<T>(eTableType type) where T : TableBase, new()
    {
        if (_tableData.ContainsKey(type))
            return _tableData[type];

        TextAsset tAsset = Resources.Load("bin/" + type.ToString()) as TextAsset;
        if (tAsset != null)
        {
            T t = new T();
            t.LoadTable(tAsset.text);
            _tableData.Add(type, t);

            return _tableData[type];
        }

        return null;
    }

    public void LoadAll()
    {
        Load<PrefabData>(eTableType.PrefabData);
        Load<SystemMessageData>(eTableType.SystemMessageData);
        Load<ImageData>(eTableType.ImageData);
        Load<CharacterData>(eTableType.CharacterData);
        Load<CharacterExplainData>(eTableType.CharacterExplainData);
        Load<CardData>(eTableType.CardData);
        Load<CardTabData>(eTableType.CardTabData);
        Load<ItemData>(eTableType.ItemData);
    }

    public TableBase Get(eTableType type)
    {
        if (_tableData.ContainsKey(type))
            return _tableData[type];

        return null;
    }

    public void AllClear()
    {
        _tableData.Clear();
    }
}
