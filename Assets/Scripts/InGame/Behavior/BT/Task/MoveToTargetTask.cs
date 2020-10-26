using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetTask : BTNode
{
	IBasicAI myAI;
	Vector3 myPosition;
	Vector3 targetPosition;
	event ControllEvent moveEvent;
	float fAttackRange;

	public MoveToTargetTask(IBasicAI AI, ControllEvent eventMove, float fRange)
	{
		myAI = AI;
		moveEvent = eventMove;
		fAttackRange = fRange;
	}

	public override BTState Evaluate()
	{
		if (moveEvent != null)
		{
			moveEvent();
		}
		
		return BTState.SUCCESS;
	}
}
