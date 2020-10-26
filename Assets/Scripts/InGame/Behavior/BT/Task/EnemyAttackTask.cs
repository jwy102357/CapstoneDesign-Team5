using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTask : BTNode
{
    IBasicAI myAI;
    ControllEventInt attackEvent;
    int nDiceRange;
    int nStartDiceValue = 1;

    public EnemyAttackTask(IBasicAI AI, int nAtkDiceRange, ControllEventInt eventAttack)
    {
        myAI = AI;
        nDiceRange = nAtkDiceRange;
        attackEvent = eventAttack;
    }

    public override BTState Evaluate()
    {
        if (attackEvent != null)
        {
            myAI.IsIdle = false;
            myAI.IsMoving = false;
            myAI.GetAnimator().SetBool("Idle", myAI.IsIdle);
            myAI.GetAnimator().SetBool("Attack", myAI.IsAttack);
            int nRandomDiceValue = Random.Range(nStartDiceValue, nStartDiceValue + nDiceRange);
            //myAI.AttackEnd();
            attackEvent(nRandomDiceValue);
            //myAI.IdleTimeChk(myAI.fIdleTime);
        }

        return BTState.SUCCESS;
    }
}
