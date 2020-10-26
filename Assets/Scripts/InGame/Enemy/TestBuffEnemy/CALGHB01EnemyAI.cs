﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALGHB01EnemyAI : CGroundEnemyAIController, IController
{
	public event ControllEventInt AtkEvent;
	public event ControllEvent MoveEvent;
	public event ControllEvent TurnEvent;
	public event ControllEventVector3 RandomMoveEvent;

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
				new HoldBuffChkCondition(this, this.GetComponent<CBuffEnemyController>())
			});

			buffSequence = new Sequence(new List<BTNode>
			{
				new HoldIsBuffEnemyCondition(this.GetComponent<CBuffEnemyController>(), this),
				new BuffAtkEnemyTask(this.GetComponent<CBuffEnemyController>(), null)
			});

			rootAI = new Selector(new List<BTNode>
			{
				idleSequence,
				buffSequence,
			});
		}, 0.1f);
	}
}
