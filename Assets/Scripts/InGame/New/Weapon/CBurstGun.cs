using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBurstGun : CWeapon
{
    Vector3 autoAimDirection;
    public int burstCount = 3;
    int upDown = 0;

    public override void Start()
    {
        base.Start();
    }

    public override void Shot()
    {
        base.Shot();
        if (bIsShotAble)
        {
            CEffectManager.Instance.GetEffect("Fire", gunAttackPivot.position);
            var bullet = CRangedObjectManager.Instance.GetRangedObject(this.bulletName);
            bullet.transform.position = this.gunAttackPivot.position + new Vector3(0, 0.1f * upDown, 0);
            upDown = upDown == 0 ? 1 : 0;
            bullet.transform.localScale = new Vector2(this.GetDirection(), 1);
            bullet.GetComponent<CBullet>().fDamage = fAtk;
            bullet.GetComponent<CBullet>().fDistance = this.fbulletDistance;
            bullet.GetComponent<CBullet>().SetStartPos();
            FindTarget(10f, 0.5f);
            if (autoTarget != null && false)
            {
                autoAimDirection = (autoTarget.transform.position - transform.position + new Vector3(0f, 0.3f, 0f)).normalized;
                bullet.GetComponent<Rigidbody2D>().velocity = autoAimDirection * fBulletSpeed;
            }
            else
            {
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(fBulletSpeed * this.GetDirection(), 0f);
            }
        }
    }

    protected override void SetDamage()
    {
        //fAtk *= this.fBonusDamage;
    }
    
    public override void Attack()
    {
        if (Time.time >= fLastShotTime + fAtkDelay)
        {
            StartCoroutine("BurstShot");
            fLastShotTime = Time.time;
        }
        if (fAtkDelay >= 0.5 && bIsShotAble)
        {
            ShowReloading();
        }
    }

    public IEnumerator BurstShot()
    {
        for(int i=0; i<burstCount; i++)
        {
            Shot();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void SetGunImageSetting()
    {
        switch (gunName)
        {
            case "M.GUN":
                //this.weaponUIImage = CResourceManager.Instance.GetSpriteForKey("Sprites/Gun/UI/DAKGUN_UI");
                this.weaponUIImage = CResourceManager.Instance.GetSpriteForKey("Sprites/Gun/M.GUN");
                this.bulletName = "M.GUN_BULLET";
                //프리팹 생성
                break;
            case "M.GUN II":
                //this.weaponUIImage = CResourceManager.Instance.GetSpriteForKey("Sprites/Gun/UI/DAKGUN_UI");
                this.weaponUIImage = CResourceManager.Instance.GetSpriteForKey("Sprites/Gun/M.GUN");
                this.bulletName = "M.GUN_BULLET";
                //프리팹 생성
                break;
            default:
                break;
        }
    }
}
