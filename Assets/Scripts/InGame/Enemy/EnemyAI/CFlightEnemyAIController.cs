using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFlightEnemyAIController : CEnemyAIController
{
	protected Sequence moveSequence;
	protected Sequence moveChkSequnce;

	public override void Start()
	{
		base.Start();
		this.IsFlight = true;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
	}
}