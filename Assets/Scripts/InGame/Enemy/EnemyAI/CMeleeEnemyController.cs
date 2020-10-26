using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMeleeEnemyController : CBaseEnemyController
{
    public BoxCollider2D attackRangeCollider;
    public override void Awake()
    {
        base.Awake();
        if (enemyObject)
        {
            attackRangeCollider = this.transform.Find("AttackRange").GetComponent<BoxCollider2D>();
            attackRangeCollider.enabled = false;
        }
    }

    public override void TurnObject()
    {
        base.TurnObject();
    }

    public override void MoveForwardObject(Vector3 moveDirection)
    {
        base.MoveForwardObject(moveDirection);
    }


    public override void AttackObject(int nDiceValue)
    {
        base.AttackObject(nDiceValue);
    }

    public override void AttackTime()
    {
        attackRangeCollider.enabled = true;
        attackRangeCollider.isTrigger = true;
    }

    public override void AttackTimeEnd()
    {
        attackRangeCollider.enabled = false;
        attackRangeCollider.isTrigger = false;
    }

}
