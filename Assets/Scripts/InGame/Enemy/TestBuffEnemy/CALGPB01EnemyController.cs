using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALGPB01EnemyController : CBuffEnemyController
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
		if (this.buffTarget != null)
		{
			this.buffTarget.bIsBuff = false;
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
			this.buffTarget.SpeedUp();
		}
	}
}
