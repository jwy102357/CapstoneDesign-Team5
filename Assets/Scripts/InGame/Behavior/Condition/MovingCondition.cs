using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCondition : BTNode
{
	IRandomMoveAI myAI;
	public MovingCondition(IRandomMoveAI AI)
	{
		myAI = AI;
	}

	public override BTState Evaluate()
	{
		if (myAI.IsMoving)
		{
			return BTState.FAILURE;
		}

		return BTState.SUCCESS;
	}
}
