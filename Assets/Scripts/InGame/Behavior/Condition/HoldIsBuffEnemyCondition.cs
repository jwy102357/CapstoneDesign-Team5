using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldIsBuffEnemyCondition : BTNode
{
	CBuffEnemyController buffEnemyController;
	IBasicAI myAI;

	public HoldIsBuffEnemyCondition(CBuffEnemyController controller, IBasicAI AI)
	{
		buffEnemyController = controller;
		myAI = AI;
	}

	public override BTState Evaluate()
	{
		if (buffEnemyController.buffTarget != null && buffEnemyController.buffTarget.gameObject.activeSelf)
		{
			return BTState.FAILURE;
		}

		if (buffEnemyController.buffTarget != null && !buffEnemyController.buffTarget.gameObject.activeSelf)
		{
			buffEnemyController.buffTarget = null;
			buffEnemyController.IsBuff = false;
		}

		if (buffEnemyController.IsBuff)
		{
			return BTState.FAILURE;
		}

		return BTState.SUCCESS;
	}
}
