using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALHMBO01EnemyController : CRangedEnemyController
{
	bool[] patternCoolDown;
	public float[] patternCoolDownTime;
	public int pattern4Number;
	public Transform attackPivot;
	public float fLeftX, fRightX, fUpY, fDownY;

	List<int> patternList;
	public override void Awake()
	{
		base.Awake();
		patternCoolDown = new bool[5];
		patternList = new List<int>();

		if (attackPivot == null)
		{
			attackPivot = this.transform.Find("AttackPivot");
		}
		if (pattern4Number == 0)
		{
			pattern4Number = 5;
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
	}

	public override void OnDisable()
	{
		for (int i = 1; i < patternCoolDown.Length; i++)
		{
			if (patternCoolDown[i])
			{
				StopCoroutine(this.PatternCoolDownSet(i));
				patternCoolDown[i] = false;
			}
		}

		patternList.Clear();
		base.OnDisable();
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

	public void ShuffleList()
	{
		Function.ShuffleArray(this.patternList);
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

	IEnumerator PatternFourShot(Vector3 pos)
	{
		var warningObj = CRangedObjectManager.Instance.GetRangedObject("WarningObject");
		warningObj.transform.position = pos;
		yield return new WaitForSeconds(1.5f);
		CRangedObjectManager.Instance.PushRangedObject("WarningObject", warningObj);
		var bullet = CRangedObjectManager.Instance.GetRangedObject("GH01_Missile").GetComponent<CMissile>();
		bullet.bIsTargetAim = true;
		Vector3 p0 = this.transform.position + new Vector3(0f, 1f, 0f);
		Vector3 p1 = p0 + new Vector3(0f, 5f, 0f);
		bullet.SetPoint(p0, p1, pos);
		bullet.Shot();
	}

	public void PatternOneShot()
	{
		CEnemyPatternManager.Instance.EightDirAttack(attackPivot.position, this.fBulletSpeed);
	}

	public int FindAttackPattern()
	{
		int res = 0;
		for (int i = 1; i < patternCoolDown.Length; i++)
		{
			if (patternCoolDown[i])
			{
				continue;
			}

			else
			{
				patternList.Add(i);
			}
		}

		if (patternList.Count != 0)
		{
			ShuffleList();
			int randomValue = Random.Range(0, patternList.Count);
			res = patternList[randomValue];
		}

		patternList.Clear();
		return res;
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
			case 3:
				this.PatternThree();
				break;
			case 4:
				this.PatternFour();
				break;
			default:
				break;
		}
	}

	void PatternOne()
	{
		if (this.enemyAI.Target != null)
		{
			//애니메이션 이벤트로 처리했다.	
		}
	}

	void PatternTwo()
	{
		CEnemyPatternManager.Instance.SpiralBullet(attackPivot.position);
	}

	void PatternThree()
	{
		if (this.enemyAI.Target != null)
		{
			CEnemyPatternManager.Instance.GuidedMissile(attackPivot.position);
		}
	}

	void PatternFour()
	{
		if (this.enemyAI.Target != null)
		{
			//에임 오브젝트 만들기. 프리팹.
			//테스트 해보기.
			//구석 꼭지점 포지션 따로 있어서 그범위내에 하면 댐
			var playerPos = this.enemyAI.Target.transform.position;
			Vector3 pos = Vector3.zero;
			for (int i = 1; i <= pattern4Number; i++)
			{
				pos.x = playerPos.x - Random.Range(-3f, 3f);
				pos.y = playerPos.y - Random.Range(-3, 3);
				
				pos.y += 0.5f;

				if (pos.x <= fLeftX|| pos.x >= fRightX || pos.y >= fUpY || pos.y <= fDownY)
				{
					i -= 1;
					continue;
				}

				StartCoroutine(this.PatternFourShot(pos));
			}
		}
	}

	
}
