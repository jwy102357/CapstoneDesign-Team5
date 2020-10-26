using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGroundBullet : MonoBehaviour
{
	public float fDamage;
	public string bulletName;

	void Awake()
	{
		fDamage = fDamage == 0 ? 1f : fDamage;	
	}
	void AttackEnd()
	{
		CRangedObjectManager.Instance.PushRangedObject(bulletName, this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(KDefine.TAG_PLAYER))
		{
			IDamageable target = collision.GetComponent<IDamageable>();
			if (target != null)
			{
				target.OnDamage(fDamage, Vector3.zero);
			}
		}
	}
}
