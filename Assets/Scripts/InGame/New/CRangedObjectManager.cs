using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class CRangedObjectManager : MonoBehaviour
{
    private static CRangedObjectManager instance;
    public static CRangedObjectManager Instance
    {
        get
        {
            if (instance == null)
            {
                var gameObject = new GameObject(typeof(CRangedObjectManager).ToString());
                gameObject.AddComponent<CRangedObjectManager>();
                instance = FindObjectOfType<CRangedObjectManager>();
            }

            return instance;
        }
    }

    private Dictionary<string, KeyValuePair<string, CObjectPool>> rangeObjectDict;
    private StringBuilder keyString;

    void Awake()
    {
        rangeObjectDict = new Dictionary<string, KeyValuePair<string, CObjectPool>>();
        keyString = new StringBuilder();
    }


    public GameObject GetRangedObject(string objectName)
    {
        if (!rangeObjectDict.ContainsKey(objectName))
        {
            this.PushKeyValue(objectName);
        }

        var filePath = rangeObjectDict[objectName].Key;
        var pool = rangeObjectDict[objectName].Value;

        return pool.VisibleObject(filePath, this.transform);
    }

    public void PushRangedObject(string objectName, GameObject gameObject)
    {
        if (!rangeObjectDict.ContainsKey(objectName))
        {
            this.PushKeyValue(objectName);
        }

        gameObject.transform.localScale = new Vector3(1, 1, 1);
        rangeObjectDict[objectName].Value.InVisbileObject(gameObject);
    }

    public void PushRangedObject(string objectName, GameObject gameObject,  Vector3 scale)
    {
        if (!rangeObjectDict.ContainsKey(objectName))
        {
            this.PushKeyValue(objectName);
        }

        gameObject.transform.localScale = scale;
        rangeObjectDict[objectName].Value.InVisbileObject(gameObject);
    }

    public static CRangedObjectManager Create()
    {
        return Instance;
    }

    private void PushKeyValue(string objectName)
    {
        keyString.Clear();
        keyString.Append("Prefabs/Bullet/");
        keyString.Append(objectName);
        rangeObjectDict.Add(objectName, new KeyValuePair<string, CObjectPool>(keyString.ToString(), new CObjectPool()));
    }
}
