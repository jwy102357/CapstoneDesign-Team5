using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//본체에서 레이저 패턴 발생 -> 본체 레이저 패턴 애니메이션 -> 팔 애니메이션 실행 -> 팔 위치 이동 -> 팔 레이저 발사
//레이저 쏠 때 윗팔 이동 및 스르르 사라지는 효과, 레이저 쏠 때 아랫팔 x축 간격 늘리기

//지진 패턴때문에 피벗조정 펀치 완료
//레이저 패턴 완료시키기 (스케일 조정 쿨타임 조정) 완료
//레이저 패턴 완료 넣기.
//팔 다때지면 몸통 콜라이더 켜기.? // 완료
//챕터포탈 생성 // 끝 , 바디가 죽었을 때 생성. // 완ㅌ료
//로켓펀치 불 피벗 조정 // 끝
//레이저 꺼지는 애니메이션 제작
//레이저 꺼지는 애니메이션에 맞춰 어택 종료 및 쿨타임 조정 및 앵글 조정 등등..

public class CC1BOLowerArm : CLivingEntity
{
	public float fDamage;
	public Animator animator;
	public CC1BOLowerArm oppositeArm;
	public CC1BOLowerPunch myPunch;
	public Transform laserPivot;
	public Transform attackBulletPivot;
	public IBasicAI enemyAI;
	public float armPointX;
	public float fBulletSpeed;
	private int groundLayerMask;
	private Vector3 basePosition;

	public override void Awake()
	{
		base.Awake();
		onDeath += ArmBroken;
		if (animator == null) animator = this.GetComponent<Animator>();
		basePosition = this.transform.localPosition;
		groundLayerMask = 1 << LayerMask.NameToLayer(KDefine.LAYER_GROUND);
		myPunch = this.GetComponentInChildren<CC1BOLowerPunch>();
		this.laserPivot = this.transform.Find("LaserAttackPivot");
	}

	void Start()
	{
		fDamage = fDamage <= 0 ? 1 : fDamage;
	}

	public override void OnDamage(float damage, Vector3 hitPos, bool isGuard = false)
	{
		myPunch.StartCoroutine(myPunch.FlashSprite());
		base.OnDamage(damage, hitPos, isGuard);
	}

	void ArmBroken()
	{
		Debug.Log("broken!");
		StartCoroutine(DeadChk());
	}

	IEnumerator DeadChk()
	{
		while (this.enemyAI.IsAttack)
		{
			yield return new WaitForEndOfFrame();
		}

		enemyAI.AttackEnd();
		enemyAI.IdleTimeChk(enemyAI.fIdleTime);
		this.gameObject.SetActive(false);
		yield break;
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

	public void SetOppositeArm(CC1BOLowerArm opposite)
	{
		this.oppositeArm = opposite;
		opposite.SetOppositeArm(this.oppositeArm);
	}

	public void LaserPatternArmPoint(int laser)
	{
		Vector3 armPoint = this.transform.position;
		armPoint.x = this.transform.parent.position.x + armPointX;
		armPoint.y = CEnemyPatternManager.Instance.playerObj.transform.position.y;
		this.transform.position = armPoint;
		if (laser == 1) CEffectManager.Instance.GetEffect("LaserFireEffect", this.laserPivot.position);
	}
	
	public void LaserPattern()
	{
		GameObject Laser = CRangedObjectManager.Instance.GetRangedObject("ArmLaser");
		Laser.transform.position = (this.laserPivot.position + this.oppositeArm.laserPivot.position) / 2f;
		//CEnemyPatternManager.Instance.NotRotateLaserPattern(myPunch.GetComponent<LineRenderer>(),
		//	myPunch.transform.position, this.oppositeArm.myPunch.transform.position);
	}

	public void ArmAttackEnd()
	{
		enemyAI.LivingEntity.gameObject.SendMessage("AppearUpperArms", SendMessageOptions.DontRequireReceiver);
		this.transform.localPosition = this.basePosition;
	}

	public void RocketPunch(int patternIndex)
	{
		this.myPunch.RocketPunch(enemyAI, patternIndex);
	}

	public void RocketPunchEnd()
	{
		StopCoroutine(this.myPunch.PunchMove(enemyAI, 2));
	}

	public void RocketReturn(int patternIndex)
	{
		this.myPunch.RockectReturn(enemyAI, patternIndex);
	}

	public Vector3 GetPosition()
	{
		return this.transform.position;
	}

	public void EarthQuake(int patternIndex)
	{
		Vector3 startPos = this.transform.position;
		Vector3 upPos = this.transform.position + Vector3.up * 5;
		Vector3 downPos = Vector3.zero;

		RaycastHit2D rayInfo = Physics2D.Raycast(this.transform.position, Vector3.down, 30f, groundLayerMask);
		if (rayInfo.collider == true)
		{
			downPos = rayInfo.point;
			StartCoroutine(MoveArm(upPos, downPos, patternIndex));
		}
	}

	public void BulletFire()
	{
		CEnemyPatternManager.Instance.EightDirAttack(this.attackBulletPivot.position, this.fBulletSpeed);
	}

	public IEnumerator MoveArm(Vector3 upPos, Vector3 downPos, int patternIndex)
	{
		float fTime = 0f;
		while (fTime < 1f)
		{
			fTime += Time.deltaTime;
			this.transform.position = Vector3.Lerp(this.transform.position, upPos, fTime);
			yield return new WaitForEndOfFrame();
		}

		fTime = 0f;
		
		while (fTime < 1f)
		{
			fTime += Time.deltaTime * 1.5f;
			this.transform.position = Vector3.Lerp(this.transform.position, downPos, fTime);
			yield return new WaitForEndOfFrame();
		}

		bool bIsGround = CEnemyPatternManager.Instance.playerObj.GetComponent<CMovement>().animator.GetBool("Grounded");
		if (bIsGround) CEnemyPatternManager.Instance.playerObj.GetComponent<IDamageable>().OnDamage(fDamage, Vector3.zero);
		//CEnemyPatternManager.Instance.Earthquake(downPos);
		//이펙트 생성

		fTime = 0f;
		
		yield return new WaitForSeconds(0.1f);

		while (fTime < 1f)
		{
			fTime += Time.deltaTime * 1.5f;
			this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, basePosition, fTime);
			yield return new WaitForEndOfFrame();
		}

		this.animator.SetTrigger("Idle");
		enemyAI.AttackEnd();
		enemyAI.IdleTimeChk(enemyAI.fIdleTime);
		enemyAI.LivingEntity.gameObject.SendMessage("PatternCoolDownSetting", patternIndex, SendMessageOptions.DontRequireReceiver);
	}

	//이부분은 펀치부분에 있어야 될듯. 펀치 부분이 따로 그려져서 나온다면 수정.
	
}
