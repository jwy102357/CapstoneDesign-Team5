﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALGH01EnemyController : CRangedEnemyController
{
	public override void Awake()
	{
		base.Awake();
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(KDefine.LAYER_ENEMY), LayerMask.NameToLayer(KDefine.LAYER_ENEMYBULLET));
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
		if (this.enemyAI.Target != null)
		{
			CEnemyPatternManager.Instance.NormalMissile(this.transform.position);
		}		
	}
}
