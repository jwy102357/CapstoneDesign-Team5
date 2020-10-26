using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALGPGMBO01EnemyController : CRangedEnemyController
{
	bool bIsGuard = false;
	bool[] patternCoolDown;
	bool bIsMove = false;
	int groundLayerMask;
	
	public float[] patternCoolDownTime;
	public RaycastHit2D groundInfo;
	public Transform attackPivot;
	List<int> patternList;

	public override void Awake()
	{
		base.Awake();
		patternCoolDown = new bool[3];
		patternList = new List<int>();
		if (attackPivot == null)
		{
			attackPivot = this.transform.Find("AttackPivot");
		}

		groundLayerMask = (1 << LayerMask.NameToLayer(KDefine.LAYER_PLATFORM)) + (1 << LayerMask.NameToLayer(KDefine.LAYER_GROUND));
	}

	public override void MoveForwardObject()
	{
		base.MoveForwardObject();
	}

	public override void TurnObject()
	{
		base.TurnObject();
	}

	public override void AttackObject(int nDiceValue)
	{
		if (patternCoolDown[nDiceValue])
		{
			nDiceValue = this.FindAttackPattern();
		}
		patternCoolDown[nDiceValue] = true;
		
		base.AttackObject(nDiceValue);
	}

	public override void OnDisable()
	{
		base.OnDisable();
		this.bIsGuard = false;
		for (int i = 1; i < patternCoolDown.Length; i++)
		{
			if (patternCoolDown[i])
			{
				StopCoroutine(this.PatternCoolDownSet(i));
				patternCoolDown[i] = false;
			}
		}

		patternList.Clear();
		this.bIsMove = false;
		base.OnDisable();
	}

	public override void OnEnable()
	{
		base.OnEnable();
	}

	public void PatternCoolDownSetting(int patternIndex)
	{
		//공격 애니메이션 끝날 때.
		StartCoroutine(PatternCoolDownSet(patternIndex));
	}

	IEnumerator PatternCoolDownSet(int patternIndex)
	{
		yield return new WaitForSeconds(patternCoolDownTime[patternIndex]);
		patternCoolDown[patternIndex] = false;
	}


	IEnumerator DangerAfterShot()
	{
		Vector3 attackPosition = CEnemyPatternManager.Instance.MakeDangerPosition();
		if (attackPosition != CEnemyPatternManager.Instance.failPos)
		{
			yield return new WaitForSeconds(1f);
		}

		else
		{
			yield break;
		}

		CEnemyPatternManager.Instance.GroundAttack(attackPosition, "GPGBO_GroundAttack");
	}
	
	IEnumerator GuardAttack()
	{
		while (bIsGuard)
		{
			StartCoroutine(this.DangerAfterShot());
			yield return new WaitForSeconds(2f);
		}
	}

	public override void OnDamage(float damage, Vector3 hitPos, bool isGuard = false)
	{
		Debug.Log("First Damage : " + damage);
		if (bIsGuard)
		{
			damage = 0;
			Debug.Log("Second Damage : " + damage);
		}

		base.OnDamage(damage, hitPos, bIsGuard);
	}

	public void AttackPattern(int index)
	{
		//공격 타이밍에 실행
		switch (index)
		{
			case 1:
				this.PatternOne();
				break;
			case 2:
				this.PatternTwo();
				break;
			default:
				break;
		}
	}

	void PatternOne()
	{
		bIsGuard = true;
		StartCoroutine(this.GuardAttack());
	}

	void PatternOneEnd()
	{
		bIsGuard = false;
	}

	void PatternTwo()
	{
		bIsMove = true;
		StartCoroutine(this.StartPatternTwo());
	}

	IEnumerator StartPatternTwo()
	{
		while (bIsMove)
		{
			this.TurnObject();
			this.transform.Translate(GetDirection() * (this.enemyAI.Speed * Time.deltaTime));
			yield return new WaitForEndOfFrame();
		}
	}

	void Shot()
	{
		if (this.enemyAI.Target != null)
		{
			CEnemyPatternManager.Instance.PlayerDirBulletAttack(attackPivot.position, this.fBulletSpeed);
		}
	}

	void MoveEnd()
	{
		bIsMove = false;
	}

	private int FindAttackPattern()
	{
		int res = 0;
		bool isFlag = false;
		for (int i = 1; i < patternCoolDown.Length; i++)
		{
			if (patternCoolDown[i])
			{
				continue;
			}

			else
			{
				if (i == 1)
				{
					isFlag = true;
				}

				if (i != 1 && isFlag)
				{
					patternList.Add(i);
					patternList.Add(i);
				}

				else
				{
					patternList.Add(i);
				}
			}
		}

		if (patternList.Count != 0)
		{
			Function.ShuffleArray(this.patternList);
			int randomValue = Random.Range(0, patternList.Count);
			res = patternList[randomValue];
		}

		patternList.Clear();
		return res;
	}
}
