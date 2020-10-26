using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMoveTask : BTNode
{
	IBasicAI myAI;
	ControllEvent moveEvent;
	public PatrolMoveTask(IBasicAI AI, ControllEvent eventMove)
	{
		myAI = AI;
		moveEvent = eventMove;
	}

	public override BTState Evaluate()
	{
		if(!myAI.EnemySight.IsdirectionChange && myAI.IsMoving && !myAI.IsAttack)
		{
			moveEvent();
		}

		return BTState.SUCCESS;
	}
}
