using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CPenetrateWeapon : CWeapon
{
    public override void Start()
    {
        base.Start();
        this.weaponCategory = EWeaponCategory.E_PENETRATE;
    }

    public override void Attack()
    {
        if (fCurrentAtkSpeed >= fAtkDelay)
        {
            this.Shot();
            fCurrentAtkSpeed = 0;
        }

        else
        {
            fCurrentAtkSpeed += Time.deltaTime;
        }
    }
}
