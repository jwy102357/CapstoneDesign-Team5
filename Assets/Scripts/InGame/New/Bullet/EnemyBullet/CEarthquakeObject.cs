using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEarthquakeObject : MonoBehaviour
{
	public float fDamage;
	public float fDelay;
	public string bulletName;
	private void OnEnable()
	{
		fDamage = fDamage == 0f ? 1f : fDamage;
		fDelay = fDelay == 0f ? 0.3f : fDelay;
		bulletName = "EarthquakeBullet";
		Function.LateCall(oParams =>
		{
			if (this.gameObject.activeSelf)
			{
				CRangedObjectManager.Instance.PushRangedObject(bulletName, this.gameObject);
			}
		}, fDelay);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(KDefine.TAG_PLAYER))
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
		}
	}
}
