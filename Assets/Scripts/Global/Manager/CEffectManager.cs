using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class CEffectManager : CSingleton<CEffectManager>
{
    private Dictionary<string, KeyValuePair<string, CObjectPool>> effectObjectDict;
    private StringBuilder keyString;

    public override void Awake()
    {
        effectObjectDict = new Dictionary<string, KeyValuePair<string, CObjectPool>>();
        keyString = new StringBuilder();
    }

    public GameObject GetEffect(string effectName, Vector3 position)
    {
        if (!effectObjectDict.ContainsKey(effectName))
        {
            this.PushKeyValue(effectName);
        }

        var filePath = effectObjectDict[effectName].Key;
        var pool = effectObjectDict[effectName].Value;

        var obj = pool.VisibleObject(filePath, this.transform);
        obj.transform.position = position;
        return obj;
    }

    public void PushEffect(GameObject effect, string effectName)
    {
        effectObjectDict[effectName].Value.InVisbileObject(effect);
    }

    //파일패스 변동가능 베이스 파일패스 변수 추가 할 것 (const)
    private void PushKeyValue(string effectName)
    {
        keyString.Clear();
        keyString.Append("Effects/FX_");
        keyString.Append(effectName);
        effectObjectDict.Add(effectName, new KeyValuePair<string, CObjectPool>(keyString.ToString(), new CObjectPool()));
    }

}

