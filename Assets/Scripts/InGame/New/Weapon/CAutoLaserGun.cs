using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAutoLaserGun : CPenetrateWeapon
{
	public override void Start()
	{
		base.Start();
        fAtkDelay = 0.10f;
		fCurrentAtkSpeed = fAtkDelay;
		Mathf.Clamp(fCurrentAtkSpeed, 0, fAtkDelay);
		fKnockback = 50f;
	}

	public override void Attack()
	{
		base.Attack();
	}

	public override void Shot()
	{
		base.Shot();
		var bullet = CRangedObjectManager.Instance.GetRangedObject("LaserInVisibleBullet");
		bullet.transform.position = this.gunAttackPivot.position;
		bullet.transform.localScale = this.GetDirection() * bullet.transform.localScale;
		bullet.GetComponent<CLaser>().fDamage = fAtk;
		bullet.GetComponent<CLaser>().SetLaserStart(this.gunAttackPivot.position);
		bullet.GetComponent<CLaser>().rigidbody.AddForce(this.GetDirection() * bullet.transform.right * fBulletSpeed);
	}
	
	protected override void SetDamage()
	{
		fAtk = 3 * this.fBonusDamage;
	}

	public override void SetGunImageSetting()
	{
		throw new System.NotImplementedException();
	}
}
