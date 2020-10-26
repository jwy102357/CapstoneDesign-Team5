using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDamagedCondition : BTNode
{
	IBasicAI myAI;
	
	public IsDamagedCondition(IBasicAI AI)
	{
		myAI = AI;
	}

	public override BTState Evaluate()
	{
		if (!myAI.LivingEntity.IsDamaged)
		{
			return BTState.SUCCESS;
		}
		return BTState.FAILURE;
	}
}
