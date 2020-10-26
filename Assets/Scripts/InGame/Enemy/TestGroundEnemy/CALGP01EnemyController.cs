using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALGP01EnemyController : CRangedEnemyController
{
	public override void Awake()
	{
		base.Awake();
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
	}

	public override void OnDisable()
	{
		base.OnDisable();
	}

	public override void OnEnable()
	{
		base.OnEnable();
	}

	void Shot()
	{
		CEnemyPatternManager.Instance.NormalBulletAttack(this.GetDirection().normalized, 
			gunAttackPivot.transform.position, fBulletSpeed);
	}
}
