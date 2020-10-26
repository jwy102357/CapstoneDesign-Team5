using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALMB02EnemyAI : CFlightEnemyAIController, IController
{
    public event ControllEventInt AtkEvent;
    public event ControllEvent MoveEvent;
    public event ControllEvent TurnEvent;
    public event ControllEventVector3 RandomMoveEvent;

    protected Sequence attackSequence;
    protected Sequence attackChkSequence;
    protected Sequence idleSequence;
    protected Sequence patrolSequence;

    public override void Start()
    {
        base.Start();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        AtkEvent = null;
        MoveEvent = null;
        TurnEvent = null;
        RandomMoveEvent = null;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        Function.LateCall((oParams) =>
        {
            idleSequence = new Sequence(new List<BTNode>
            {
                new IdleChkCondition(this)
            });

            attackChkSequence = new Sequence(new List<BTNode>
            {
                new IsAttackRangeSCondition(this),
                new IsAttackCondition(this)
            });

            attackSequence = new Sequence(new List<BTNode>
            {
                attackChkSequence,
                new FindTargetTask(this, TurnEvent),
                new DecideAttackTask(this),
                new EnemyAttackTask(this, 2, this.AtkEvent)
            });

            rootAI = new Selector(new List<BTNode>
            {
                idleSequence,
                attackSequence
            });
        }, 0.1f);
    }
}
