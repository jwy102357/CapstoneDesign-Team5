﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALGH01EnemyAI : CFixedEnemyAIController, IController
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

			attackChkSequence = new Sequence(new List<BTNode>
			{
				new IsAttackRangeSCondition(this),
				new IsAttackCondition(this)
			});

			attackSequence = new Sequence(new List<BTNode>
			{
				attackChkSequence,
				new DecideAttackTask(this),
				new EnemyAttackTask(this, 1, this.AtkEvent)
			});

			rootAI = new Selector(new List<BTNode>
			{
				idleSequence,
				attackSequence
			});
		}, 0.1f);
	}


}

