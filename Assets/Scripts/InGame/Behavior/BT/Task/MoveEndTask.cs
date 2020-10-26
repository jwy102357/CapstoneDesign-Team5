using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEndTask : BTNode
{
	float fEndtime;
	IRandomMoveAI myAI;
	WaitForSeconds waitTime;
	public MoveEndTask(IRandomMoveAI AI, float fTime)
	{
		myAI = AI;
		fEndtime = fTime;
		waitTime = new WaitForSeconds(fEndtime);
	}

	public override BTState Evaluate()
	{
		myAI.RandomMoveEnd(waitTime);
		return BTState.SUCCESS;
	}
}
