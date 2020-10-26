using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//미사일 프리팹 바꾸기.
public class CC1BOUpperArm : CLivingEntity
{
	public Animator animator;
	public float fDamage;
	public IBasicAI enemyAI;
	public Transform attackBulletPivot;
	public float fBulletSpeed;
	private BoxCollider2D myCollider;
	private Vector3 basePosition;
	private bool bIsIdle = false;
	[SerializeField]
	private Transform[] MissilePoints;


	public override void Awake()
	{
		base.Awake();
		onDeath += ArmBroken;
		myCollider = this.GetComponent<BoxCollider2D>();
		if (animator == null) this.GetComponent<Animator>();
		basePosition = this.transform.position;
	}

	void Start()
	{
		fDamage = fDamage <= 0 ? 1 : fDamage;
	}

	void ArmBroken()
	{
		Debug.Log("broken!");
		StartCoroutine(DeadChk());
	}

	public void AttackEnd()
	{
		enemyAI.AttackEnd();
		enemyAI.IdleTimeChk(enemyAI.fIdleTime);
	}

	public void PatternCoolDown(int patternIndex)
	{
		enemyAI.LivingEntity.gameObject.SendMessage("PatternCoolDownSetting", patternIndex);
	}

	public void DisAppear()
	{
		StartCoroutine(this.DisAppearArm(0f));
	}

	public void Appear()
	{
		StartCoroutine(this.DisAppearArm(1f));
	}

	public void BulletFire()
	{
		CEnemyPatternManager.Instance.EightDirAttack(this.attackBulletPivot.position, this.fBulletSpeed);
	}

	public void GuidedMissile()
	{
		CEnemyPatternManager.Instance.GuidedMissile(MissilePoints[0].position, MissilePoints[1].position, MissilePoints[2].position, true);
	}


	public Vector3 GetPosition()
	{
		return this.transform.position;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (!IsDead)
		{
			if (collision.CompareTag(KDefine.TAG_PLAYER))
			{
				IDamageable target = collision.GetComponent<IDamageable>();
				if (fDamage != 0)
				{
					target.OnDamage(fDamage, Vector3.zero);
				}
			}
		}
	}

	IEnumerator DeadChk()
	{
		while (this.enemyAI.IsAttack)
		{
			yield return new WaitForEndOfFrame();
		}

		this.gameObject.SetActive(false);
		yield break;
	}

	IEnumerator DisAppearArm(float fEnd)
	{
		float fTime = 0f;
		Color color = this.spriteRenderer.color;
		while (fTime < 1f)
		{
			fTime += Time.deltaTime * 1.3f;
			color.a = Mathf.Lerp(color.a, fEnd, fTime);
			this.spriteRenderer.color = color;
			yield return new WaitForEndOfFrame();
		}

		myCollider.enabled = fEnd == 0 ? false : true;
		yield break;
	}
}
