using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALGHB01EnemyController : CBuffEnemyController
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

	public override void OnDisable()
	{
		if (this.buffTarget != null && this.buffTarget.gameObject.activeSelf)
		{
			this.buffTarget.ReturnToOriginAtk();
		}

		this.buffTarget = null;
		base.OnDisable();
	}

	public override void OnEnable()
	{
		base.OnEnable();
	}

	public override void Buff()
	{
		if (this.buffTarget != null)
		{
			this.IsBuff = true;
			if (!this.buffTarget.bIsBuff)
			{
				this.buffTarget.fAtk *= 1.3f;
			}
			this.buffTarget.bIsBuff = true;
		}
	}
}
