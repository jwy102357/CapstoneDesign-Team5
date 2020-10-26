using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShieldItem : CItem, IItem
{
    CPlayerHealth health;

    void Start()
    {
        health = GameObject.Find("Player").GetComponent<CPlayerHealth>();
        this.name = KDefine.ITEM_SHIELD;
    }

    public void Use()
    {
        health.AddShield();
        CItemManager.Instance.PushObject(KDefine.ITEM_SHIELD, this.gameObject);
    }
}
