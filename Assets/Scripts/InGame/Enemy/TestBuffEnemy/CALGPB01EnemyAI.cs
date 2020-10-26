using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALGPB01EnemyAI : CGroundEnemyAIController, IController
{
    public event ControllEventInt AtkEvent;
    public event ControllEvent MoveEvent;
    public event ControllEvent TurnEvent;
    public event ControllEventVector3 RandomMoveEvent;

	public float fMoveTime;
	private Sequence buffSequence;
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
				new BuffIdleChkCondition(this)
			});

			patrolSequence = new Sequence(new List<BTNode>
			{
				new IsPatrolCondition(this),
				new MoveChkTask(this, fMoveTime),
				new PatrolMoveTask(this, MoveEvent),
				new ChangeDirectionTask(this, TurnEvent)
			});

			buffSequence = new Sequence(new List<BTNode>
			{
				new IsBuffEnemyCondition(this.GetComponent<CBuffEnemyController>()),
				new BuffSpeedEnemyTask(this.GetComponent<CBuffEnemyController>(), null)
			});

			rootAI = new Selector(new List<BTNode>
			{
				idleSequence,
				buffSequence,
				patrolSequence
			});
		}, 0.1f);
	}
}
