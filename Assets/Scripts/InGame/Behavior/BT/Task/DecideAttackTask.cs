using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecideAttackTask : BTNode
{
	IBasicAI myAI;
	public DecideAttackTask(IBasicAI AI)
	{
		myAI = AI;
	}

	public override BTState Evaluate()
	{
        if (!myAI.IsAttack && !myAI.IsAttackDelay)
        {
		    myAI.IsAttack = true;
		    return BTState.SUCCESS;
        }
        else
        {
            return BTState.FAILURE;
        }
	}
}
