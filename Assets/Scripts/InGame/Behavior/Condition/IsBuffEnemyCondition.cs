using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsBuffEnemyCondition : BTNode
{
	CBuffEnemyController buffEnemyController;
	public IsBuffEnemyCondition(CBuffEnemyController buffEnemy)
	{
		buffEnemyController = buffEnemy;
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
