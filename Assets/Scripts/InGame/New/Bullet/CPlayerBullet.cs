using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerBullet : CBullet
{
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        this.bIsEnemyBullet = false;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.gameObject.activeSelf)
        {
            if (!collision.gameObject.CompareTag(KDefine.TAG_PLAYER) && !collision.gameObject.CompareTag(KDefine.TAG_PLATFORM))
            {
                Boom(collision);
            }
        }
    }

    protected void Boom(Collision2D collision)
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
