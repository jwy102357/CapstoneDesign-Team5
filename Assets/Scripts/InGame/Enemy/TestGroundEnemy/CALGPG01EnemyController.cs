using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALGPG01EnemyController : CRangedEnemyController
{
	bool bIsShotReady = false;
	bool bIsGuard = false;
	Vector3 attackPosition;
	public override void Awake()
	{
		base.Awake();
		attackPosition = new Vector3();
	}

	public override void MoveForwardObject()
	{
		base.MoveForwardObject();
	}

	public override void TurnObject()
	{
		base.TurnObject();
	}

	public override void AttackObject(int nDiceValue)
	{
		base.AttackObject(nDiceValue);

		switch (nDiceValue)
		{
			case 1:
				this.MakeDangerObject();
				break;

			case 2:
				break;

			case 3:
				break;

			default:
				Debug.Log("다이스 범위 초과!");
				break;
		}
	}

	public override void OnDisable()
	{
		base.OnDisable();
		this.bIsShotReady = false;
		this.bIsGuard = false;
	}

	public override void OnEnable()
	{
		base.OnEnable();
	}

	
	void Shot()
	{
		bIsShotReady = false;
		CEnemyPatternManager.Instance.GroundAttack(attackPosition, "GPG01_GroundAttack");
	}

	public void MakeDangerObject()
	{
		bIsShotReady = true;
		if (enemyAI.Target != null)
		{
			Vector3 pos = CEnemyPatternManager.Instance.MakeDangerPosition();
			if (CEnemyPatternManager.Instance.failPos != pos)
			{
				attackPosition = pos;
			}
		}
	}

	void Guard()
	{
		bIsGuard = true;
	}

	public override void OnDamage(float damage, Vector3 hitPos, bool isGuard = false)
	{
		Debug.Log("First Damage : " + damage);
		if (bIsGuard)
		{
			damage = 0;
			Debug.Log("Second Damage : " + damage);
		}

		base.OnDamage(damage, hitPos, bIsGuard);
	}

	void GuardEnd()
	{
		bIsGuard = false;
	}
}
