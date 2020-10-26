using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCondition : BTNode
{
	IBasicAI myAI;
	float fAttackRange;

	public MoveCondition(IBasicAI AI, float fRange)
	{
		myAI = AI;
		fAttackRange = fRange;
	}

	public override BTState Evaluate()
	{
		if (myAI.GetTargetDistance() > fAttackRange)
		{
			return BTState.SUCCESS;
		}

		return BTState.FAILURE;
	}
}
