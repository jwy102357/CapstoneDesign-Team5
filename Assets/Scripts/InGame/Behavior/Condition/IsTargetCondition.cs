using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTargetCondition : BTNode
{
	private IBasicAI myAI;

	public IsTargetCondition(IBasicAI AI)
	{
		myAI = AI;
	}

	public override BTState Evaluate()
	{
		if (myAI.Target == null)
		{
			return BTState.SUCCESS;
		}

		return BTState.FAILURE;
	}
}
