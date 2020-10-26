using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleChkCondition : BTNode
{
	IBasicAI myAI;
	public BossIdleChkCondition(IBasicAI AI)
	{
		myAI = AI;
	}

	public override BTState Evaluate()
	{
		if (!myAI.IsAttack && !myAI.IsAttackDelay)
		{
			return BTState.FAILURE;
		}

		if (myAI.IsIdle)
		{
			myAI.IsMoving = false;
			myAI.GetAnimator().SetBool("Move", myAI.IsMoving);
			return BTState.SUCCESS;
		}

		return BTState.FAILURE;
	}

}
