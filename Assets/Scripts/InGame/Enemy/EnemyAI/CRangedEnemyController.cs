using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRangedEnemyController : CBaseEnemyController
{
    protected Transform gunAttackPivot;
    [SerializeField]
    protected float fBulletSpeed;
    public override void Awake()
    {
        base.Awake();
        gunAttackPivot = this.transform.Find("GunAttackPivot");
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

}
