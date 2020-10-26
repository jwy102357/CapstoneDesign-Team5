using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CChainLightningWeapon : CAutoWeapon
{
	public override void Start()
	{
		base.Start();
		fCurrentAtkSpeed = fAtkDelay;
		this.weaponUIImage = CResourceManager.Instance.GetSpriteForKey("Sprites/PNG/HUD/WEAPON ICONS/SMG HUD");
	}

	public override void Attack()
	{
		base.Attack();
	}

	public override void Shot()
	{
		base.Shot();
		if (bIsShotAble)
		{
			CEffectManager.Instance.GetEffect("Fire", gunAttackPivot.position);
			var bullet = CRangedObjectManager.Instance.GetRangedObject("LightningGunBullet");
			bullet.transform.position = this.gunAttackPivot.position;
			bullet.transform.localScale = new Vector2(this.GetDirection(), 1);
			bullet.GetComponent<CBullet>().fDamage = fAtk;
			bullet.GetComponent<CBullet>().fDistance = this.fbulletDistance;
			bullet.GetComponent<CBullet>().SetStartPos();
			bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(fBulletSpeed * this.GetDirection(), 0f);
		}
	}

	protected override void SetDamage()
	{
		fAtk = fAtk * this.fBonusDamage;
	}

	public override void SetGunImageSetting()
	{
		throw new System.NotImplementedException();
	}
}
