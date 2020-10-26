using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CAutoWeapon : CWeapon
{
	public override void Start()
	{
		base.Start();
		this.weaponCategory = EWeaponCategory.E_AUTOGUN;
	}

    public override void Shot()
    {
        base.Shot();
    }

    public override void Attack()
	{
		if (Time.time >= fLastShotTime + fAtkDelay)
		{
			this.Shot();
			fLastShotTime = Time.time;
		}
	}
}
