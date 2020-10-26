using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIdleChkCondition : BTNode
{
	IBasicAI myAI;
	public BuffIdleChkCondition(IBasicAI AI)
	{
		myAI = AI;
	}

	public override BTState Evaluate()
	{
		if (!myAI.IsIdle)
		{
			return BTState.FAILURE;
		}

		else if (myAI.IsIdle)
		{
			if (myAI.LivingEntity.IsDamaged)
			{
				myAI.IsIdle = false;
				return BTState.FAILURE;
			}
		}
		return BTState.SUCCESS;
	}
}
