using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldBuffChkCondition : BTNode
{
	IBasicAI myAI;
	CBuffEnemyController buffEnemyController;
	public HoldBuffChkCondition(IBasicAI AI, CBuffEnemyController controller)
	{
		myAI = AI;
		buffEnemyController = controller;
	}

	public override BTState Evaluate()
	{
		if (myAI.IsIdle && !buffEnemyController.IsBuff)
		{
			myAI.IsIdle = false;
		}

		if (myAI.IsIdle && buffEnemyController.buffTarget != null && !buffEnemyController.buffTarget.gameObject.activeSelf)
		{
			myAI.IsIdle = false;
		}

		if (!myAI.IsIdle)
		{
			myAI.GetAnimator().SetBool("Idle", myAI.IsIdle);
			return BTState.FAILURE;
		}

		myAI.IsIdle = true;
		myAI.GetAnimator().SetBool("Idle", myAI.IsIdle);
		return BTState.SUCCESS;
	}
}
