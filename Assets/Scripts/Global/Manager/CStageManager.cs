using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class CStageManager : CSingleton<CStageManager>
{
    private Dictionary<string, KeyValuePair<string, CObjectPool>> stageDict;
    private StringBuilder keyString;
    public CStage CurrentStage { get; set; }

    public override void Awake()
    {
        stageDict = new Dictionary<string, KeyValuePair<string, CObjectPool>>();
        keyString = new StringBuilder();
    }


    public GameObject GetStageObject(string objectName)
    {
        if (!stageDict.ContainsKey(objectName))
        {
            this.PushKeyValue(objectName);
        }

        var filePath = stageDict[objectName].Key;
        var pool = stageDict[objectName].Value;

        return pool.VisibleObject(filePath, this.transform);
    }

    public void PushStageObject(string objectName, GameObject gameObject)
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        stageDict[objectName].Value.InVisbileObject(gameObject);
    }

    public void PushGameEndObject()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                this.PushStageObject(this.transform.GetChild(i).gameObject.name, this.transform.GetChild(i).gameObject);
            }
        }
    }

    public void ReUseStageSetting()
    {
        if (CurrentStage != null)
        {
            CurrentStage.StageEnd();
        }
    }


    private void PushKeyValue(string objectName)
    {
        keyString.Clear();
        keyString.Append("Prefabs/Stage/");
        keyString.Append(objectName);
        stageDict.Add(objectName, new KeyValuePair<string, CObjectPool>(keyString.ToString(), new CObjectPool()));
    }
}
