using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGroundEnemyAIController : CEnemyAIController
{
	protected Sequence moveSequence;
	protected Sequence moveChkSequnce;
	protected Sequence patrolSequence;
	protected Sequence idleSequence;

	public float fAttackRange; 

	public override void Start()
	{
		base.Start();
		this.IsFlight = false;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
	}

}