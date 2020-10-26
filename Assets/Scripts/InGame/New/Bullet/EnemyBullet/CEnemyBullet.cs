using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyBullet : CBullet
{
    public override void Awake()
    {
        base.Awake();
        fDamage = 1f;
        this.bIsEnemyBullet = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
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
                }

                CEffectManager.Instance.GetEffect("Boom", this.transform.position);
                if (this.gameObject.activeSelf)
                {
                    CRangedObjectManager.Instance.PushRangedObject(bulletName, this.gameObject);
                }
                //rigidbody.velocity = Vector2.zero;
            }
        }
    }
}
