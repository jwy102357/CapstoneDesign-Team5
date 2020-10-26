using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAttackRangeSCondition : BTNode
{
	IBasicAI myAI;
	public IsAttackRangeSCondition(IBasicAI AI)
	{
		myAI = AI;
	}

	public override BTState Evaluate()
	{
		if (myAI.EnemySight.IsAttackRange)
		{
			myAI.SetTarget();
			return BTState.SUCCESS;
		}

		return BTState.FAILURE;
	}
}
