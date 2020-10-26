using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirectionTask : BTNode
{
    IBasicAI myAI;
    ControllEvent turnEvent;

    public ChangeDirectionTask(IBasicAI AI, ControllEvent eventTurn)
    {
        myAI = AI;
        turnEvent = eventTurn;
    }

    public override BTState Evaluate()
    {
        if (myAI.EnemySight.IsdirectionChange)
        {
            if (turnEvent != null && !myAI.IsAttack)
            {
                turnEvent();
            }
        }

        return BTState.SUCCESS;
    }
}
