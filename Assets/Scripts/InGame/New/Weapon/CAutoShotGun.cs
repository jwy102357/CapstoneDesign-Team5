using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//기본시간 랜덤으로 변수주고 +0.04씩 더해주며 생성하기. 코루틴으로
public class CAutoShotGun : CAutoWeapon
{
	public int nMaxBullet;
	private float fCreateTime = 0.017f;
    public float fRotationAngle;

    public override void Start()
	{
		base.Start();
		fCurrentAtkSpeed = fAtkDelay;
		Mathf.Clamp(fCurrentAtkSpeed, 0, fAtkDelay);
        /*
		fKnockback = 50f;
        fBulletSpeed = 20f;
        fAtkDelay = 0.10f;
        fAtk = 3f;
        nMaxBullet = 12;
        */
    }

    public override void Attack()
    {
        if (Time.time >= fLastShotTime + fAtkDelay)
        {
            Shot();
            fLastShotTime = Time.time;
        }
        if (fAtkDelay >= 0.5 && bIsShotAble)
        {
            ShowReloading();
        }
    }

	public override void Shot()
	{
		base.Shot();
        if (bIsShotAble)
        {
            StartCoroutine(CreateBullet());
        }
	}

	IEnumerator CreateBullet()
	{

        //CreateTime도 변수로 만들어서 랜덤 해도됨
        for (int i = 0; i < nMaxBullet; i++)
		{
            float fRandomTime = fCreateTime / nMaxBullet;
            int nRandom = Random.Range(0, 11);
            var bullet = CRangedObjectManager.Instance.GetRangedObject(bulletName);
            //bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            bullet.transform.position = this.gunAttackPivot.position;
            bullet.transform.localRotation = Quaternion.Euler(0, 0, -(fRotationAngle * 4) + fRotationAngle * nRandom);
            bullet.transform.localScale = this.GetDirection() * bullet.transform.localScale;
            bullet.GetComponent<CBullet>().fDamage = fAtk;
            bullet.GetComponent<CBullet>().fDistance = this.fbulletDistance;
            bullet.GetComponent<CBullet>().SetStartPos();
            bullet.GetComponent<Rigidbody2D>().velocity = (this.GetDirection() * bullet.transform.right * fBulletSpeed);
            //yield return new WaitForSeconds(fRandomTime);
            yield return new WaitForEndOfFrame();
            //fRandomTime += fCreateTime;
        }
    }

    protected override void SetDamage()
    {
        //fAtk = 3;
    }

    public override void SetGunImageSetting()
    {
        switch(gunName)
        {
            case "S.GUN":
                //this.weaponUIImage = CResourceManager.Instance.GetSpriteForKey("Sprites/Gun/UI/DAKDAKGUN_UI");
                this.weaponUIImage = CResourceManager.Instance.GetSpriteForKey("Sprites/Gun/S.GUN");
                this.bulletName = "S.GUN_BULLET";
                break;
        }
    }
}
