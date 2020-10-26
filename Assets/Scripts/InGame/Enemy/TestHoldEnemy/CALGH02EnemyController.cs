using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CALGH02EnemyController : CRangedEnemyController
{
	public int nMaxBullet;
	public float fAngleStart;
	public Transform attackPivot;
	public override void Awake()
	{
		base.Awake();
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(KDefine.LAYER_ENEMY), LayerMask.NameToLayer(KDefine.LAYER_ENEMYBULLET));
		if (attackPivot == null)
		{
			attackPivot = this.transform.Find("AttackPivot");
		}
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
			CEnemyPatternManager.Instance.EightDirAttack(attackPivot.position, this.fBulletSpeed);
		}

	}
}
