using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAttackRangeCondition : BTNode
{
	IBasicAI myAI;
	float fAttackRange;

	public IsAttackRangeCondition(IBasicAI AI, float fRange)
	{
		myAI = AI;
		fAttackRange = fRange;
	}

	public override BTState Evaluate()
	{
		if (myAI.GetTargetDistance() > fAttackRange)
		{
			return BTState.FAILURE;
		}
		return BTState.SUCCESS;
	}
}
