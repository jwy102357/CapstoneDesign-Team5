using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CBuffEnemyController : CBaseEnemyController
{
	bool bIsFirstDamage = false;
	public bool IsBuff = false;
	public CBaseEnemyController buffTarget;

	public override void Awake()
	{
		base.Awake();
	}

	public override void OnDamage(float damage, Vector3 hitPos, bool isGuard = false)
	{
		base.OnDamage(damage, hitPos);

		if (!bIsFirstDamage)
		{
			bIsFirstDamage = true;
			if (enemyAI != null)
			{
				if (!bIsBuff) enemyAI.Speed *= 1.3f;
			}
		}
	}

	public override void OnDisable()
	{
		base.OnDisable();
		this.IsBuff = false;
		this.bIsFirstDamage = false;
	}

	public abstract void Buff();
}
