using SimpleJSON;
using System.Collections;
using System.Collections.Generic;

public class PrefabData : TableBase
{
    public enum Index
    {
        Index,
        Name,
        Location,

        max
    }

    public string mainKey = "Index";

    public override void LoadTable(string strJson)
    {
        JSONNode node = JSONNode.Parse(strJson);

        _sheetData = new Dictionary<string, Dictionary<string, string>>();

        for (int n = 0; n < (int)Index.max; n++)
        {
            Index subKey = (Index)n;
            if (string.Compare(mainKey, subKey.ToString()) != 0)
            {
                for (int m = 0; m < node[0].AsArray.Count; m++)
                {
                    Add(node[0][m][mainKey], subKey.ToString(), node[0][m][subKey.ToString()].Value);
                }
            }
        }
    }
}
