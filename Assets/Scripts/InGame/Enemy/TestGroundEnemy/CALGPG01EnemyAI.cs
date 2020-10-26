using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALGPG01EnemyAI : CGroundEnemyAIController, IController
{
	public event ControllEventInt AtkEvent;
	public event ControllEvent MoveEvent;
	public event ControllEvent TurnEvent;
	public event ControllEventVector3 RandomMoveEvent;

	protected Sequence attackSequence;
	protected Sequence attackChkSequence;

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

			patrolSequence = new Sequence(new List<BTNode>
			{
				new IsPatrolCondition(this),
				new MoveChkTask(this, 1f),
				new IsAttackCondition(this),
				new PatrolMoveTask(this, MoveEvent),
				new ChangeDirectionTask(this, TurnEvent)
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
				patrolSequence,
				attackSequence
			});
		}, 0.1f);
	}
}
