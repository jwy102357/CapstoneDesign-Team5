using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLaser : MonoBehaviour
{
	public string bulletName;

	public float fDamage { get; set; }
    private LineRenderer laserLine;
    private bool bIsCollision = false;
    public Rigidbody2D rigidbody;

    void Awake()
    {
        laserLine = this.GetComponentInChildren<LineRenderer>();
        rigidbody = this.GetComponent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(KDefine.TAG_ENEMY))
        {
            CEffectManager.Instance.GetEffect("Boom", this.transform.position);
            if (this.gameObject.activeSelf)
            {
                CRangedObjectManager.Instance.PushRangedObject(bulletName, this.gameObject);
            }

            IDamageable target = collision.GetComponent<IDamageable>();

            if (target != null)
            {
                Vector3 hitnormal = collision.transform.position - this.transform.position;
                target.OnDamage(fDamage, hitnormal);
            }
        }

        if (collision.CompareTag(KDefine.TAG_TILE) || collision.CompareTag(KDefine.TAG_PLATFORM))
        {
            CEffectManager.Instance.GetEffect("Boom", this.transform.position);
            bIsCollision = true;
            this.SetLaserEnd(this.transform.position);
        }
    }

    public void SetLaserStart(Vector3 position)
    {
        this.laserLine.enabled = false;
        laserLine.SetPosition(0, position);
        Debug.Log(laserLine.GetPosition(0));
    }

    public void SetLaserEnd(Vector3 position)
    {
        this.laserLine.enabled = true;
        laserLine.SetPosition(1, position);
    }
}
