﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//내일 가드몹으로 애니메이션 복사해서 테스트 해보기.
public class CALGPGMBO01EneymAI : CFixedEnemyAIController, IController
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

	public override void OnEnable()
	{
		base.OnEnable();
		Function.LateCall((oParams) =>
		{
			idleSequence = new Sequence(new List<BTNode>
			{
				new BossIdleChkCondition(this)
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
				new EnemyAttackTask(this, 2, this.AtkEvent)
			});

			rootAI = new Selector(new List<BTNode>
			{
				idleSequence,
				attackSequence
			});
		}, 0.1f);
	}

	public override void OnDisable()
	{
		base.OnDisable();
		AtkEvent = null;
		MoveEvent = null;
		TurnEvent = null;
		RandomMoveEvent = null;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
	}
}

