using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCountCondition : BTNode
{
	int nCurrentMoveCount;
	int nMaxCount;
	IRandomMoveAI myRandomAI;
	public MoveCountCondition(int nCount, IRandomMoveAI randomAI)
	{
		nMaxCount = nCount;
		nCurrentMoveCount = 0;
		myRandomAI = randomAI;
	}

	public override BTState Evaluate()
	{
		if (nCurrentMoveCount > nMaxCount)
		{
			nCurrentMoveCount = 0;
			myRandomAI.StartIdle();
			return BTState.FAILURE;
		}
		nCurrentMoveCount++;
		return BTState.SUCCESS;
	}
}
