using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CChargeWeapon : CWeapon
{
	protected float fCurrenrtCharge;
	protected float fFullCharge;
	public bool bIsCharge = false;

	public override void Start()
	{
		base.Start();
		fCurrenrtCharge = 1;
		fFullCharge = 2;
		Mathf.Clamp(fCurrenrtCharge, 1, fFullCharge);
		this.weaponCategory = EWeaponCategory.E_CHARGEGUN;
	}

	public virtual void Charge()
	{
		if (!bIsCharge)
		{
			bIsCharge = true;
		}
		fCurrenrtCharge += Time.deltaTime;
	}

	public override void Shot()
	{
		if (!bIsCharge)
		{
			return;
		}
		base.Shot();
	}

	public override void Attack()
	{
		this.Charge();
	}

}
