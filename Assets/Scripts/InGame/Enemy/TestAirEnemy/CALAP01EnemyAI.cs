using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALAP01EnemyAI : CFlightEnemyAIController, IController, IRandomMoveAI
{
	//보류
	//정지해있다가 총알을쏘고 3번 움직여?
	//처음에 범위에 없으면 노무빙 범위에 오면 어택 후 3무브 1번 2번 반복

	public event ControllEventInt AtkEvent;
    public event ControllEvent MoveEvent;
    public event ControllEvent TurnEvent;
    public event ControllEventVector3 RandomMoveEvent;

	protected Sequence attackSequence;
	protected Sequence attackChkSequence;
	protected Sequence idleSequence;


	public int nMoveCount = 3;
	public float fAttackRange;
    public int RandomCount { get; set; }

	public override void Start()
	{
		base.Start();

	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		Function.LateCall((a_oParams =>
		{
			this.OnEnter();
		}), 0.15f);
	}

	public override void OnDisable()
	{
		base.OnDisable();
		AtkEvent = null;
		MoveEvent = null;
		TurnEvent = null;
		RandomMoveEvent = null;
	}

	private void OnEnter()
	{
		idleSequence = new Sequence(new List<BTNode>
		{
			new IdleChkCondition(this)
		});

		attackChkSequence = new Sequence(new List<BTNode>
		{
			new MovingCondition(this),
			new IsAttackRangeSCondition(this),
			new IsAttackCondition(this)
		});

		attackSequence = new Sequence(new List<BTNode>
		{
			attackChkSequence,
			new FindTargetTask(this, TurnEvent),
			new DecideAttackTask(this),
			new AttackTargetTask(this, this.AtkEvent, 1, fAttackRange)
		});

		moveChkSequnce = new Sequence(new List<BTNode>
		{
			new IsAttackCondition(this),
			new MovingCondition(this),
			new MoveCountCondition(nMoveCount, this)
		});

		moveSequence = new Sequence(new List<BTNode>
		{
			new FindTargetTask(this, TurnEvent),
			moveChkSequnce,
			new RandomMoveTask(RandomMoveEvent, this),
			new MoveEndTask(this, fAttackDelay / nMoveCount)
		}); ;

		rootAI = new Selector(new List<BTNode>
		{
			idleSequence,
			moveSequence,
			attackSequence
		});
	}

	public void StartIdle()
	{
		if (!this.IsIdle)
		{
			StartCoroutine(IdleEnd((fIdleTime)));
		}
	}

	public void RandomMoveEnd(WaitForSeconds waitSec)
	{
		StartCoroutine(MoveEnd(waitSec));
	}

	private IEnumerator MoveEnd(WaitForSeconds waitSec)
	{
		this.IsMoving = true;
		yield return waitSec;
		this.IsMoving = false;
		animator.SetBool("Move", this.IsMoving);
	}
}
