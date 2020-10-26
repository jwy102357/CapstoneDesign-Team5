using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CItemBox : MonoBehaviour, IInteractItem
{
    BoxCollider2D myCollider;
    Transform myTransform;
    string myObjectName;
    public string MyName
    {
        get
        {
            return myObjectName;
        }
    }

    void Start()
    {
        myCollider = this.GetComponent<BoxCollider2D>();
        myTransform = this.GetComponent<Transform>();
        this.name = KDefine.ITEM_BOX;
    }

    public void InteractItem()
    {
        this.myCollider.enabled = false;
        //CItemManager.Instance.ItemCreate(myTransform.position);
        Destroy(this.gameObject);
    }
}
