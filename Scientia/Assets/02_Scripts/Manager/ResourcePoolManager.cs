using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePoolManager : TSingleton<ResourcePoolManager>
{
    public enum eResourceKind
    {
        Image,
        Prefab,

        max
    }

    Dictionary<eResourceKind, Dictionary<string, object>> _resourceData = new Dictionary<eResourceKind, Dictionary<string, object>>();

    protected override void Init()
    {
        base.Init();
    }

    public T GetObj<T>(eResourceKind kind, string name) where T : class
    {
        if (_resourceData.ContainsKey(kind))
        {
            if (_resourceData[kind].ContainsKey(name))
            {
                object obj = _resourceData[kind][name];
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            else
            {
                Debug.LogWarning("Key is not exist / name : " + name);
            }
        }
        else
        {
            Debug.LogWarning("Key is not exist / kind : " + kind.ToString());
        }

        return null;
    }

    //void LoadData<T>(eResourceKind kind, eTableType type) where T : class
    //{
    //    Dictionary<string, object> tempDic = new Dictionary<string, object>();

    //    TableBase tb = TableManager._instance.Get(type);

    //    foreach (string key in tb._datas.Keys)
    //    {
    //        object obj = Resources.Load(tb._datas[key]["Location"], typeof(T));
    //        T temp = (T)Convert.ChangeType(obj, typeof(T));
    //        tempDic.Add(tb._datas[key]["Name"], temp);
    //    }

    //    _resourceData.Add(kind, tempDic);
    //}

    public void ResourceAllLoad()
    {
        //LoadData<Sprite>(eResourceKind.Image, eTableType.ImageData);
        //LoadData<GameObject>(eResourceKind.Prefab, eTableType.PrefabData);
    }
}
