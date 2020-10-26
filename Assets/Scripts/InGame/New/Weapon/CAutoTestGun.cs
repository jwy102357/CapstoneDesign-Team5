using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAutoTestGun : CAutoWeapon
{
    Vector3 autoAimDirection;
    public override void Start()
	{
		base.Start();
		fCurrentAtkSpeed = fAtkDelay;
		Mathf.Clamp(fCurrentAtkSpeed, 0, fAtkDelay);
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
            var bullet = CRangedObjectManager.Instance.GetRangedObject(bulletName);
			bullet.transform.position = this.gunAttackPivot.position;
			bullet.transform.localScale = new Vector2(this.GetDirection(), 1);
			bullet.GetComponent<CBullet>().fDamage = fAtk;
			bullet.GetComponent<CBullet>().fDistance = this.fbulletDistance;
			bullet.GetComponent<CBullet>().SetStartPos();
            FindTarget(10f, 0.5f);
            if(autoTarget != null && false)
            {
                autoAimDirection = (autoTarget.transform.position - transform.position + new Vector3(0f, 0.3f, 0f)).normalized;
                bullet.GetComponent<Rigidbody2D>().velocity = autoAimDirection * fBulletSpeed;
            }
            else
            {
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(fBulletSpeed * this.GetDirection(), 0f);
            }
            if(fAtkDelay > 0.5)
            {
                ShowReloading();
            }
        }
	}

    protected override void SetDamage()
    {
        //fAtk *= this.fBonusDamage;
    }

    public override void SetGunImageSetting()
    {
        switch (gunName)
        {
            case "ONEPUNGUN":
                this.weaponUIImage = CResourceManager.Instance.GetSpriteForKey("Sprites/Gun/UI/DAKDAKGUN_UI");
                this.bulletName = "TestAutoGunBullet";
                break;
            case "DAKGUN":
                this.weaponUIImage = CResourceManager.Instance.GetSpriteForKey("Sprites/Gun/UI/DAKGUN_UI");
                this.bulletName = "TestAutoGunBullet";
                //프리팹 생성
                break;
            case "DAKDAKGUN":
                this.weaponUIImage = CResourceManager.Instance.GetSpriteForKey("Sprites/GUN/UI/DAKDAKGUN_UI");
                this.bulletName = "TestAutoGunBullet";
                break;
            default:
                break;
        }
    }

}
