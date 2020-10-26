using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALAC2BOEnemyController : CRangedEnemyController
{
	bool[] patternCoolDown;
	public float[] patternCoolDownTime;
	public int missileCnt = 4;
	public float leftX, rightX, upY, downY;

	List<int> patternList;
	private Transform bodyAttackPivot;

	Transform player;

	public override void Awake()
	{
		base.Awake();
		patternCoolDown = new bool[10];
		patternList = new List<int>();

		player = GameObject.Find(KDefine.TAG_PLAYER).GetComponent<Transform>();
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

		if (nDiceValue == 0 || patternCoolDown[nDiceValue])
		{
			enemyAI.AttackEnd();
			enemyAI.IdleTimeChk(enemyAI.fIdleTime);
			nDiceValue = 0;
		}

		Debug.Log(nDiceValue);
		this.AttackPattern(nDiceValue);

		patternCoolDown[nDiceValue] = true;

		base.AttackObject(nDiceValue);
	}

	public void ShuffleList()
	{
		Function.ShuffleArray(this.patternList);
	}

	public void PatternCoolDownSetting(int patternIndex)
	{
		StartCoroutine(PatternCoolDownSet(patternIndex));
	}

	public void SendEnd(int patternIndex)
	{
		patternCoolDown[patternIndex] = true;
		this.PatternCoolDownSetting(patternIndex);
		enemyAI.AttackEnd();
		enemyAI.IdleTimeChk(enemyAI.fIdleTime);
	}


	IEnumerator PatternCoolDownSet(int patternIndex)
	{
		yield return new WaitForSeconds(patternCoolDownTime[patternIndex]);
		patternCoolDown[patternIndex] = false;
	}

	public int FindAttackPattern() // 수정 요망
	{
		int res = 0;
		int cnt = 0;

		for (int i = 1; i <= patternCoolDownTime.Length; i++)
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

		if (index >= 1 && index <= 9)
		{
		}

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
			case 5:
				this.PatternFive();
				break;
			default:
				break;
		}
	}

	void PatternOne(int index = 1)
	{
		StartCoroutine(this.DropStones(5, index));
	}

	void PatternTwo(int index = 2)
	{
		StartCoroutine(this.AllDropStones(5, index));
	}

	void PatternThree()
	{
		CEnemyPatternManager.Instance.SpiralBullet(this.transform.position);
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
			for (int i = 1; i <= missileCnt; i++)
			{
				pos.x = playerPos.x - Random.Range(-3f, 3f);
				pos.y = playerPos.y - Random.Range(-3, 3);

				pos.y += 0.5f;

				if (pos.x <= leftX || pos.x >= rightX || pos.y >= upY|| pos.y <= downY)
				{
					i -= 1;
					continue;
				}

				StartCoroutine(this.PatternFourShot(pos));
			}
		}
	}

	void PatternFive()
	{
		//구르기 패턴 
		//leftx까지 갔다가 도착하면 addforce시키고, 그래비티 scale 조정
		//그리고 rightX까지 가면 어택끝?
		//lerp써서 leftx까지 이동 와일문끝나면 time = 0 시키고 애드포스 그리고 그래비티 스케일 
		//땅에 닿았다면? 그럼 콜라이ㅏ더는 2개여야함 그림에 맞춰야하기 때문에. 그래비티 스케일 0
		//오른쪽 lerp 끝.
		//편도로 3번
	}

	IEnumerator PatternFourShot(Vector3 pos)
	{
		var warningObj = CRangedObjectManager.Instance.GetRangedObject("WarningObject");
		warningObj.transform.position = pos;
		yield return new WaitForSeconds(1.5f);
		CRangedObjectManager.Instance.PushRangedObject("WarningObject", warningObj);
		var bullet = CRangedObjectManager.Instance.GetRangedObject("GH01_Missle").GetComponent<CMissile>();
		bullet.bIsTargetAim = true;
		Vector3 p0 = this.transform.position + new Vector3(0f, 1f, 0f);
		Vector3 p1 = p0 + new Vector3(0f, 5f, 0f);
		bullet.SetPoint(p0, p1, pos);
		bullet.Shot();
	}

	IEnumerator DropStones(int stoneCnt, int index)
	{
		for (int i = 1; i <= stoneCnt; i++)
		{
			var stoneObj = CRangedObjectManager.Instance.GetRangedObject("StoneObject");
			yield return new WaitForSeconds(0.5f);
			//stoneobj shot
		}

		enemyAI.AttackEnd();
		enemyAI.IdleTimeChk(enemyAI.fIdleTime);
		this.PatternCoolDownSetting(index);
		yield break;
	}

	IEnumerator AllDropStones(int stoneCnt, int index)
	{
		for (int i = stoneCnt; i >= 1; i++)
		{
			var stoneObj = CRangedObjectManager.Instance.GetRangedObject("StoneObject");
			Function.LateCall((oParams) =>
			{
				//stoneObj Shot!
			}, 0.5f * i);
			yield return new WaitForSeconds(0.5f);
			
		}

		enemyAI.AttackEnd();
		enemyAI.IdleTimeChk(enemyAI.fIdleTime);
		this.PatternCoolDownSetting(index);
		yield break;
	}
}
