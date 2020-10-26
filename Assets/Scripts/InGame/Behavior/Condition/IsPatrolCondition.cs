using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPatrolCondition : BTNode
{
	private IBasicAI myAI;
	
	public IsPatrolCondition(IBasicAI AI)
	{
		myAI = AI;
	}

	public override BTState Evaluate()
	{
		if (myAI.EnemySight.IsAttackRange && !myAI.IsAttackDelay)
		{
			return BTState.FAILURE;
		}
		if (myAI.Target != null) myAI.Target = null;
		return BTState.SUCCESS;
	}
}
