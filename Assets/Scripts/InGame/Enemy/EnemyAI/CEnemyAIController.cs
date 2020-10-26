using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//똑같이 체크를하고 공격직전에 쿨다운인지 체크를 하고 쿨다운이면 fail보내고 무브.
public class CEnemyAIController : MonoBehaviour, IBasicAI
{
	public float fAttackDelay = 1f;
	public float fIdleTime { get; set; }
    protected Transform effectPoint;

    protected Transform myTransform;
    protected Animator animator;
    protected Selector rootAI;

	protected float prevPosSetTimer = 0f;
	private float prevPosMaxTimer = 0.3f;

	public GameObject Target { get; set; }

	public bool FacingRight { get; set; }

	public bool IsFlight { get; set; }

	public bool IsJump { get; set; }

	public bool IsAttack { get; set; }

	public bool IsMoving { get; set; }

	public bool IsIdle { get; set; }

	public bool IsAttackDelay { get; set; }

	public float Speed { get; set; }

	public CLivingEntity LivingEntity { get; set; }

	public CEnemySight EnemySight { get; set; }
	
	private GameObject playerObj;

	// Start is called before the first frame update
	public virtual void Start()
    {
		fIdleTime = this.fAttackDelay;
    }

    public virtual void FixedUpdate()
    {
        if (!LivingEntity.IsDead && rootAI != null)
        { 
            rootAI.Evaluate();
        }
    }

	public Animator GetAnimator()
	{
		return this.animator;
	}


	public void AttackEnd()
	{
		this.IsAttack = false;
		animator.SetBool("Attack", this.IsAttack);
		animator.SetInteger("AttackState", 0);
		this.IsAttackDelay = true;
		Function.LateCall((oParams) =>
		{
			this.IsAttackDelay = false;
		}, this.fAttackDelay);
	}

	public void SetTarget()
	{
		this.Target = playerObj;
	}
	
	IEnumerator MoveEnd(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		this.IsMoving = false;
		animator.SetBool("Move", this.IsMoving);
		if (!this.IsIdle)
		{
			StartCoroutine(IdleEnd((fIdleTime)));
		}
	}

	public void MoveTimeChk(float seconds)
	{
		StartCoroutine(MoveEnd(seconds));
	}

	protected IEnumerator IdleEnd(float seconds)
	{
		this.IsIdle = true;
		animator.SetBool("Idle", this.IsIdle);
		yield return new WaitForSeconds(seconds);
		this.IsIdle = false;
		animator.SetBool("Idle", this.IsIdle);

	}

	public void IdleTimeChk(float seconds)
	{
		StartCoroutine(IdleEnd(seconds));
	}

	public virtual void OnEnable()
	{
		myTransform = this.GetComponent<Transform>();
		animator = this.GetComponent<Animator>();
		playerObj = GameObject.Find("Player");
		Mathf.Clamp(this.prevPosSetTimer, 0, this.prevPosMaxTimer);
	}

	public virtual void OnDisable()
	{
		myTransform = null;
		animator = null;
		Target = null;
		this.IsIdle = false;
		this.IsAttack = false;
		this.LivingEntity = null;
		this.IsMoving = false;
		this.IsAttackDelay = false;
	}



	public float GetTargetDistance()
	{
		return Vector2.Distance(Target.transform.position, this.transform.position);
	}

	public Vector3 GetTargetPosition()
	{
		return this.Target.transform.position;
	}

	public Transform GetTransform()
	{
		return this.myTransform;
	}
}
