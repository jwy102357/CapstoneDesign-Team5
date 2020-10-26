using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class CEnemyManager : CSingleton<CEnemyManager>
{
    private Dictionary<string, KeyValuePair<string, CObjectPool>> enemyDict;
    private StringBuilder keyString;

    public override void Awake()
    {
        enemyDict = new Dictionary<string, KeyValuePair<string, CObjectPool>>();
        keyString = new StringBuilder();
    }


    public GameObject GetEnemyObject(string objectName)
    {
        if (!enemyDict.ContainsKey(objectName))
        {
            this.PushKeyValue(objectName);
        }

        var filePath = enemyDict[objectName].Key;
        var pool = enemyDict[objectName].Value;

        return pool.VisibleObject(filePath, this.transform);
    }

    public void PushEnemyObject(string objectName, GameObject gameObject)
    {
        var localScale = gameObject.transform.localScale;
        if (localScale.x < 0f)
        {
            localScale.x = localScale.x * -1f;
        }

        if (!enemyDict.ContainsKey(objectName))
        {
            this.PushKeyValue(objectName);
        }

        gameObject.transform.localScale = localScale;
        enemyDict[objectName].Value.InVisbileObject(gameObject);
    }

    public void PushGameEndObject()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (this.transform.GetChild(i).gameObject.name == "FinalBoss")
                {
                    Destroy(this.transform.GetChild(i).gameObject);
                    break;
                }
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                 this.PushEnemyObject(this.transform.GetChild(i).gameObject.name, this.transform.GetChild(i).gameObject);
            }
        }
    }

    private void PushKeyValue(string objectName)
    {
        keyString.Clear();
        keyString.Append("Prefabs/Enemy/");
        keyString.Append(objectName);
        enemyDict.Add(objectName, new KeyValuePair<string, CObjectPool>(keyString.ToString(), new CObjectPool()));
    }
}
