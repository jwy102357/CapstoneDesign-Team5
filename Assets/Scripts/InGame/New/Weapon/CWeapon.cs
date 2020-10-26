using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public enum EWeaponCategory
{
    E_AUTOGUN,
    E_PENETRATE,
    E_CHARGEGUN
};

public abstract class CWeapon : MonoBehaviour
{
    protected CMovement playerMovement;
    protected Rigidbody2D playerRigidbody;
    protected Transform gunAttackPivot;
    public StringBuilder remainBulletString;
    [SerializeField]
    protected int nCurrentBulletCount;
    [SerializeField]
    protected int nMaxBulletCount;
    protected int nQuality;
    [SerializeField]
    protected float fAtk;
    protected float fDistance;
    [SerializeField]
    protected float fKnockback;
    protected float fCurrentAtkSpeed;
    protected float fLastShotTime;
    protected bool bIsShotAble = true;
   
    [SerializeField]
    protected float fbulletDistance;
    public float fBulletSpeed;
    public float fAtkDelay;
    public string gunName;
    public string bulletName;
    public int IndexNumber { get; set; }
    public CReloading reloadUI;
    
    public float fBonusDamage;
    protected EWeaponCategory weaponCategory;
    public Sprite weaponUIImage;

    protected Collider2D autoTarget;
    protected const int maxOverlap = 10;
    protected Collider2D[] enemyList;
    public CCameraShakeInCinemachine cameraShake;

    public virtual void Start()
    {
        if (gunAttackPivot == null)
        {
            OnEnter();
        }
        this.SetCameraShake();
        enemyList = new Collider2D[maxOverlap];
    }

    public virtual void Shot()
    {
        if (nCurrentBulletCount <= 0)
        {
            bIsShotAble = false;
            return;
        }

        else
        {
            bIsShotAble = true;
        }
        cameraShake.StartingShake();
        this.KnockBack();
        this.MinusBulletCount();
    }
    
    public void SetBonusDamage(float fDamage)
    {
        fBonusDamage = fDamage;
        this.SetDamage();
    }
    public abstract void Attack();
    protected abstract void SetDamage();
    protected void KnockBack()
    {
        playerRigidbody.AddForce(new Vector2(-this.GetDirection() * this.fKnockback, 0)); ;
    }
    protected int GetDirection()
    {
        return playerMovement.bIsFacingRight ? 1 : -1;
    }

    public EWeaponCategory GetWeaponCategory()
    {
        return weaponCategory;
    }

    public int GetCurrentBullet()
    {
        return this.nCurrentBulletCount;
    }

    public void SetCurrentBullet(int bullet)
    {
        this.nCurrentBulletCount = bullet;
    }

    public int GetMaxBullet()
    {
        return this.nMaxBulletCount;
    }

    public void MadeBulletString()
    {
        if (nMaxBulletCount == 123456789)
        {
            remainBulletString.Clear();
            return;
        }
        if (remainBulletString == null)
        {
            remainBulletString = new StringBuilder();
        }

        remainBulletString.Clear();
        remainBulletString.Append(nCurrentBulletCount.ToString());
        remainBulletString.Append("/");
        remainBulletString.Append(nMaxBulletCount.ToString());
        CWeaponManager.Instance.SetBulletText(remainBulletString.ToString());
    }

    public void MinusBulletCount()
    {
        this.nCurrentBulletCount--;

        if (remainBulletString.Length != 0)
        {
            this.MadeBulletString();
        }
    }

    public void ReloadBullet(int nCount)
    {
        nCurrentBulletCount += (nCurrentBulletCount + nCount <= nMaxBulletCount) ? nCount : (nMaxBulletCount - nCurrentBulletCount);
    }

    public void OnEnter()
    {
        playerMovement = FindObjectOfType<CMovement>();
        gunAttackPivot = this.transform.Find("GunAttackPivot");
        playerRigidbody = playerMovement.GetComponent<Rigidbody2D>();
        weaponCategory = new EWeaponCategory();
        if (remainBulletString == null)
        {
            remainBulletString = new StringBuilder();
        }
        Mathf.Clamp(nCurrentBulletCount, 0, nMaxBulletCount);
        SetDamage();
    }

    public void SetCameraShake()
    {
        if (cameraShake == null)
        {
            cameraShake = GetComponentInParent<CCameraShakeInCinemachine>();
        }
    }

    public abstract void SetGunImageSetting();

    protected void FindTarget(float xWidth = 10f, float yWidth = 1f)
    {
        float minDistance = 100000f;
        Vector2 point1 = transform.position + new Vector3(0f, yWidth, 0f);
        Vector2 point2 = transform.position + new Vector3(playerMovement.transform.localScale.x * xWidth, -yWidth, 0f);
        int layermask = 1 << LayerMask.NameToLayer(KDefine.LAYER_ENEMY);

        for (int i = 0; i < enemyList.Length; i++)
        {
            enemyList[i] = null;
        }

        autoTarget = null;

        Physics2D.OverlapAreaNonAlloc(point1, point2, enemyList, layermask);

        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i] == null)
            {
                break;
            }
            if (!enemyList[i].enabled)
            {
                break;
            }
            Vector3 offset = this.transform.position - enemyList[i].transform.position;
            float distance = offset.sqrMagnitude;

            if (distance < minDistance)
            {
                autoTarget = enemyList[i];
                minDistance = distance;
            }
        }
    }

    protected void ShowReloading()
    {
        if (reloadUI == null)
        {
            Debug.Log("Weapon's reloadUI is null");
            return;
        }
        reloadUI.gameObject.SetActive(true);
        reloadUI.reloadingTime = fAtkDelay;
    }
}
