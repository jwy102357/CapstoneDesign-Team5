using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGuidedMissile : MonoBehaviour
{
    private Vector3 point0;
    private Vector3 point1;
    private Vector3 point2;
    private GameObject targetObj;
    private bool bIsShot;
    private float fTime = 0f;
    private bool bIsFollowing = false;
    private Rigidbody2D rigidbody;
    public int nCollisionCnt = 3;
    public string bulletName;
    public float fDamage = 1f;
    public float fbulletSpeed;
    public float spriteAngle;

    public void OnEnable()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(KDefine.LAYER_ENEMY), LayerMask.NameToLayer(KDefine.LAYER_COLLISIONBULLET));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(KDefine.LAYER_COLLISIONBULLET), LayerMask.NameToLayer(KDefine.LAYER_COLLISIONBULLET));

        targetObj = CWeaponManager.Instance.playerShooter.gameObject;
        point0 = targetObj.transform.position + new Vector3(10f, 0f, 0f);
        this.transform.position = point0;
        point1 = point0 + new Vector3(5f, 5f, 0f);
        point2 = point1 + new Vector3(-5f, 5f, 0f);
        if (rigidbody == null)
        {
            rigidbody = this.GetComponent<Rigidbody2D>();
        }
    }

    public void OnDisable()
    {
        if (bIsShot)
        {
            StopCoroutine(Curve());
        }

        if (bIsFollowing)
        {
            StopCoroutine(TargetFollowing());
        }

        targetObj = null;
        nCollisionCnt = 3;
        this.fTime = 0f;
        this.bIsShot = false;
    }

    IEnumerator Curve()
    {
        bIsShot = true;
        while (fTime < 1)
        {
            fTime += Time.deltaTime;
            var prevPos = this.transform.position;
            this.transform.position = GetCurve(point0, point1, point2, fTime);

            var dir = (this.transform.position -  prevPos).normalized;
            float fAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - spriteAngle;
            this.transform.rotation = Quaternion.AngleAxis(fAngle, Vector3.forward);
            yield return new WaitForEndOfFrame();
        }
        fTime = 0f;
        bIsShot = false;
        StartCoroutine(TargetFollowing());
    }

    IEnumerator TargetFollowing()
    {
        bIsFollowing = true;

        while(this.gameObject.activeSelf || targetObj != null)
        {
            var dir = ((targetObj.transform.position + (Vector3.up * 0.63f)) - this.transform.position).normalized;
            float fAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - spriteAngle;
            this.transform.rotation = Quaternion.AngleAxis(fAngle, Vector3.forward);
            rigidbody.velocity = dir * fbulletSpeed;
            yield return new WaitForEndOfFrame();
        }

        bIsFollowing = false;
    }

    public void SetPoint(Vector2 p0, Vector2 p1, Vector2 p2, GameObject target)
    {
        point0 = p0;
        point1 = p1;
        point2 = p2;
        targetObj = target;
    }

    public void Shot()
    {
        StartCoroutine(Curve());
    }

    Vector3 GetCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.activeSelf)
        {
            if (!collision.gameObject.CompareTag(KDefine.TAG_ENEMY) && !collision.gameObject.CompareTag(KDefine.TAG_PLATFORM))
            {
                IDamageable target = collision.gameObject.GetComponent<IDamageable>();
                if (target != null)
                {
                    Vector3 hitnormal = collision.transform.position - this.transform.position;
                    target.OnDamage(fDamage, hitnormal);
                    CEffectManager.Instance.GetEffect("Boom", this.transform.position);

                    if (this.gameObject.activeSelf)
                    {
                        CRangedObjectManager.Instance.PushRangedObject(bulletName, this.gameObject);
                    }
                }

                else if (target == null && collision.gameObject.CompareTag(KDefine.TAG_PLAYERBULLET))
                {

                    this.nCollisionCnt--;
                    CEffectManager.Instance.GetEffect("Boom", this.transform.position);

                    if (this.nCollisionCnt <= 0)
                    {
                        Debug.Log(this.nCollisionCnt);
                        CEffectManager.Instance.GetEffect("Boom", this.transform.position);

                        if (this.gameObject.activeSelf)
                        {
                            CRangedObjectManager.Instance.PushRangedObject(bulletName, this.gameObject);
                        }
                    }
                }
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
