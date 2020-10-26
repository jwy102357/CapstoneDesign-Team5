using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChkTask : BTNode
{
	float fEndTime;
	IBasicAI myAI;
	public MoveChkTask(IBasicAI AI, float fMoveEndTime)
	{
		myAI = AI;
		fEndTime = fMoveEndTime;
	}

	public override BTState Evaluate()
	{
		if (!myAI.IsMoving)
		{
			myAI.IsMoving = true;
			myAI.MoveTimeChk((Random.Range(fEndTime, fEndTime + 1)));
		}

		return BTState.SUCCESS;
	}
}
