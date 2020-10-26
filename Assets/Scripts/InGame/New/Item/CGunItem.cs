using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGunItem : CItem, IInteractItem
{
    CWeapon weapon;
    protected BoxCollider2D interactCollider;
    string myObjectName;
    public string MyName
    {
        get
        {
            return myObjectName;
        }
    }

    void Awake()
    {
        this.weapon = this.GetComponent<CWeapon>();
        interactCollider = this.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        //this.interactCollider.enabled = false;
        myObjectName = this.transform.name;
        this.gameObject.SetActive(true);
        /*
        Function.LateCall((oParams) =>
        {
            this.InteractStartWeapon();
        }, 3.0f);
        */
    }

    public void InteractEndWeapon()
    {
        this.interactCollider.enabled = false;
    }

    public void InteractStartWeapon()
    {
        this.interactCollider.enabled = true;
    }

    public void InteractItem()
    {
        if(interactCollider.enabled == true)
        {
            this.InteractEndWeapon();
        }
        CWeaponManager.Instance.AddWeapon(weapon);
    }
}
