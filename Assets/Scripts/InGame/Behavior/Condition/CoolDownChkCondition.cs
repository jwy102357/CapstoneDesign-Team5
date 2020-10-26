using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownChkCondition : BTNode
{
	IBasicAI myAI;
	public CoolDownChkCondition(IBasicAI AI)
	{
		myAI = AI;
	}

	public override BTState Evaluate()
	{
		if (myAI.GetAnimator().GetInteger("AttackState") == 0)
		{
			if (myAI.IsAttack)
			{
				return BTState.SUCCESS;
			}
		}
		
		return BTState.FAILURE;
	}
}
