using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTargetTask : BTNode
{
	IBasicAI myAI;
	event ControllEvent turnEvent;

	public FindTargetTask(IBasicAI AI, ControllEvent eventTurn)
	{
		myAI = AI;
		turnEvent = eventTurn;
	}

	public override BTState Evaluate()
	{
		if (myAI.Target != null)
		{
			if (turnEvent != null && !myAI.IsAttack)
			{
				turnEvent();
			}
		}
		return BTState.SUCCESS;
	}
}
