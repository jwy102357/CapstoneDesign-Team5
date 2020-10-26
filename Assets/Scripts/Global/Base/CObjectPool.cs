using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CObjectPool
{
    Stack<GameObject> objectPool;
    public CObjectPool()
    {
        objectPool = new Stack<GameObject>();
    }

    public GameObject VisibleObject(string filePath, Transform parent)
    {
        GameObject gameObject;
        if (objectPool.Count <= 0)
        {
            return GameObject.Instantiate(CResourceManager.Instance.GetObjectForKey(filePath), parent);
        }

        gameObject = objectPool.Pop();
        gameObject.SetActive(true);
        gameObject.transform.parent = parent;
        return gameObject;
    }

    public void InVisbileObject(GameObject gameObject)
    {
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
        objectPool.Push(gameObject);
    }
}
