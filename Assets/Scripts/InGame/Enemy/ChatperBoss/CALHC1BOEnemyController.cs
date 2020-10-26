using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALHC1BOEnemyController : CRangedEnemyController
{
	bool[] patternCoolDown;
	public float[] patternCoolDownTime;
	public int phase1 = 5;
	public int phase2 = 6;

	public CC1BOLowerArm[] lowerArms;
	public CC1BOUpperArm[] upperArms;
	public CC1BOBody body;
	
	List<int> patternList;
	private Transform bodyAttackPivot;

    Transform player;
    private BoxCollider2D[] attackLine;
    public LineRenderer[] lineList;
    public Vector3[] lineRange;

    public override void Awake()
	{
		base.Awake();
		patternCoolDown = new bool[10];
		patternList = new List<int>();
		if (bodyAttackPivot == null) bodyAttackPivot = this.body.transform.Find("AttackPivot");

        player = GameObject.Find(KDefine.TAG_PLAYER).GetComponent<Transform>();
        lineList = GetComponentsInChildren<LineRenderer>();
        lineRange = new Vector3[2];
        attackLine = body.transform.Find("AttackLine").GetComponentsInChildren<BoxCollider2D>();

    }

    private void Start()
    {
        lineRange[0] = transform.position + Vector3.left * 15 + Vector3.up * 7.5f;
        lineRange[1] = transform.position + Vector3.right * 14 + Vector3.down * 4.5f;
        Debug.Log(lineRange[0]);
        Debug.Log(lineRange[1]);
        Debug.Log(transform.position);
        attackLine[0].size = new Vector2(lineRange[1].x + 1 - lineRange[0].x, 0.5f);
        attackLine[1].size = new Vector2(0.5f, lineRange[0].y + 1 - lineRange[1].y);
        attackLine[0].gameObject.SetActive(false);
        attackLine[1].gameObject.SetActive(false);
    }


    public override void Die()
	{
		StartCoroutine(DeadChk());
	}

	IEnumerator DeadChk()
	{
		while (this.enemyAI.IsAttack)
		{
			this.IsDead = false;
			yield return new WaitForEndOfFrame();
		}

		enemyAI.AttackEnd();
		enemyAI.IdleTimeChk(enemyAI.fIdleTime);
		CEffectManager.Instance.GetEffect("DeathBoom_01", this.transform.position + new Vector3(0f, 0.4f, 0f));
		CMerchandiseManager.Instance.ManyCoinCreate(1 + (int)fMaxHealth / 100, 1 + (int)fMaxHealth / 30, this.transform.position);
		if (20 + (fMaxHealth / 50) * (fMaxHealth / 50) > Random.Range(1, 100))
		{
			CItemManager.Instance.MaterialItemCreate(this.transform.position);
		}
		if (Random.Range(1, 101) < 10)
		{
			CItemManager.Instance.HeartItemCreate(this.transform.position);
		}

		this.IsDead = true;
		Destroy(this.gameObject);
		yield break;
	}

	public override void OnEnable()
	{
		base.OnEnable();
		for (int i = 0; i < lowerArms.Length; i++)
		{
			lowerArms[i].enemyAI = this.enemyAI;
			lowerArms[i].fBulletSpeed = this.fBulletSpeed;
			upperArms[i].fBulletSpeed = this.fBulletSpeed;
			upperArms[i].enemyAI = this.enemyAI;
		}

		StartCoroutine(ArmBrokenChk());
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
		body.boxCollider.enabled = false;
		
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
		this.body.AttackEnd();
		StartCoroutine(PatternCoolDownSet(patternIndex));
	}

	public void SendEnd(int patternIndex)
	{
		patternCoolDown[patternIndex] = true;
		this.PatternCoolDownSetting(patternIndex);
		enemyAI.AttackEnd();
		enemyAI.IdleTimeChk(enemyAI.fIdleTime);
	}

	IEnumerator ArmBrokenChk()
	{
		while(!ArmsAllBroken())
		{
			yield return new WaitForEndOfFrame();
		}

		while(enemyAI.IsAttack)
		{
			yield return new WaitForEndOfFrame();
		}

		body.boxCollider.enabled = true;
		yield break;
	}

	IEnumerator PatternCoolDownSet(int patternIndex)
	{
		yield return new WaitForSeconds(patternCoolDownTime[patternIndex]);
		if (patternIndex == 2 && lowerArms[0].IsDead && lowerArms[1].IsDead)
		{
			patternCoolDown[patternIndex] = true;
			yield break;
		}

		if (patternIndex == 1 && lowerArms[0].IsDead && lowerArms[1].IsDead)
		{
			patternCoolDown[patternIndex] = true;
			yield break;
		}

		patternCoolDown[patternIndex] = false;
	}

	public int FindAttackPattern()
	{
		int res = 0;
		int cnt = 0;
		
		if (!ArmsAllBroken())
		{
			for (int i = 0; i < lowerArms.Length; i++)
			{
				if (lowerArms[i].IsDead)
				{
					cnt++;
				}
			}

			if (cnt >= 2)
			{
				patternCoolDown[1] = true;
				patternCoolDown[2] = true;
			}

			cnt = 0;
			for (int i = 0; i < upperArms.Length; i++)
			{
				if (upperArms[i].IsDead)
				{
					cnt++;
				}
			}

			if (cnt >= 2)
			{
				patternCoolDown[4] = true;
				patternCoolDown[5] = true;
			}
		}
		

		if (ArmsAllBroken())
		{
			for (int i = phase2; i < patternCoolDownTime.Length; i++)
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
		}

		else
		{
			for (int i = 1; i <= phase1; i++)
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

	public void AppearUpperArms()
	{
		for (int i = 0; i < upperArms.Length; i++)
		{
			if (upperArms[i].IsDead) continue;
			upperArms[i].Appear();
		}
	}

	public void DisAppearUpperArms()
	{
		for (int i = 0; i < upperArms.Length; i++)
		{
			if (upperArms[i].IsDead) continue;
			upperArms[i].DisAppear();
		}
	}

	public void AttackPattern(int index)
	{
		//공격 타이밍에 실행

		if (index >= 1 && index <= 9)
		{
			this.body.Attack();
		}

		switch (index)
		{
			case 1:
				this.PatternOne(index);
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
			case 6:
				this.PatternSix();
				break;
			case 7:
				this.PatternSeven();
				break;
			case 8:
				this.PatternEight();
				break;
			case 9:
				this.PatternNine();
				break;
			default:
				break;
		}
	}

	void PatternOne(int patternIndex)
	{
		if (this.enemyAI.Target != null)
		{
			int cnt = 0;
			for (int i = 0; i < lowerArms.Length; i++)
			{
				if (!lowerArms[i].IsDead)
				{
					lowerArms[i].animator.SetTrigger("EarthQuake");
				}

				else
				{
					cnt++;
				}
			}

			if (cnt >= 2)
			{
				enemyAI.AttackEnd();
				enemyAI.IdleTimeChk(enemyAI.fIdleTime);
				patternCoolDown[1] = true;
			}
		}
	}

	void PatternTwo()
	{
		int cnt = 0;
		for (int i = 0; i < lowerArms.Length; i++)
		{
			if (lowerArms[i].IsDead)
			{
				cnt++;
			}
		}

		if (cnt >= 1 && cnt < 2)
		{
			for (int i = 0; i < lowerArms.Length; i++)
			{
				if (!lowerArms[i].IsDead)
				{
					lowerArms[i].animator.SetTrigger("RocketPunch");
				}
			}

			this.DisAppearUpperArms();
		}

		else if (cnt == 0)
		{
			lowerArms[0].animator.SetTrigger("Laser");
			lowerArms[1].animator.SetTrigger("Laser");
			this.DisAppearUpperArms();
		}

		else if (cnt >= 2)
		{
			enemyAI.AttackEnd();
			enemyAI.IdleTimeChk(enemyAI.fIdleTime);
			patternCoolDown[2] = true;
		}
	}

	void PatternThree()
	{
		if (this.enemyAI.Target != null)
		{
			CEffectManager.Instance.GetEffect("C1BOFireEffect", this.body.effectPivot.position);
			Function.LateCall((oParams =>
			{
				CEnemyPatternManager.Instance.LaserPattern(this.bodyAttackPivot.position, -170f, -10f, 3);
			}), 0.5f);
		}
	}

	void PatternFour()
	{
		if (this.enemyAI.Target != null)
		{
			int cnt = 0;
			for (int i = 0; i < upperArms.Length; i++)
			{
				if (!upperArms[i].IsDead)
				{
					upperArms[i].animator.SetTrigger("GuidedMissile");
				}

				else
				{
					cnt++;
				}
			}

			if (cnt >= 2)
			{
				enemyAI.AttackEnd();
				enemyAI.IdleTimeChk(enemyAI.fIdleTime);
				patternCoolDown[4] = true;
			}
		}
	}

	void PatternFive()
	{
		StartCoroutine(PatternFiveTrigger());
	}
	
	void PatternSix()
	{
		this.body.EightDirAttack(this.bodyAttackPivot.transform.position, this.fBulletSpeed);
		enemyAI.AttackEnd();
		enemyAI.IdleTimeChk(enemyAI.fIdleTime);
		this.PatternCoolDownSetting(6);
	}

	void PatternSeven()
	{
		this.body.SpiralBullet(this.bodyAttackPivot.transform.position);
		enemyAI.AttackEnd();
		enemyAI.IdleTimeChk(enemyAI.fIdleTime);
		this.PatternCoolDownSetting(7);
	}

	void PatternEight()
	{
		CEnemyPatternManager.Instance.LaserPattern(this.bodyAttackPivot.position, -170f, -10f, 8);
	}

	void PatternNine()
    {
        StartCoroutine("PatternNineMakeDangerLine");
        StartCoroutine("PatternNineShot");
	}

	bool ArmsAllBroken()
	{
		int res = 0;
		for (int i = 0; i < upperArms.Length; i++)
		{
			if (upperArms[i].IsDead) res++;
			if (lowerArms[i].IsDead) res++;
		}

		if (res >= 4) return true;

		return false;
	}

	IEnumerator PatternFiveTrigger()
	{
		int nRandom = Random.Range(0, 2);

		if (nRandom == 0)
		{
			for (int i = 0; i < upperArms.Length; i++)
			{
				if (upperArms[i].IsDead) continue;
				upperArms[i].animator.SetTrigger("BulletFire");
				yield return new WaitForSeconds(0.3f);
			}

			for (int i = 0; i < lowerArms.Length; i++)
			{
				if (lowerArms[i].IsDead) continue;
				lowerArms[i].animator.SetTrigger("BulletFire");
				yield return new WaitForSeconds(0.3f);
			}
		}

		else
		{
			for (int i = 0; i < lowerArms.Length; i++)
			{
				if (lowerArms[i].IsDead) continue;
				lowerArms[i].animator.SetTrigger("BulletFire");
				yield return new WaitForSeconds(0.3f);
			}

			for (int i = 0; i < upperArms.Length; i++)
			{
				if (upperArms[i].IsDead) continue;
				upperArms[i].animator.SetTrigger("BulletFire");
				yield return new WaitForSeconds(0.3f);
			}
		}

		enemyAI.AttackEnd();
		enemyAI.IdleTimeChk(enemyAI.fIdleTime);
		this.PatternCoolDownSetting(5);
		yield break;
	}


    public IEnumerator PatternNineMakeDangerLine()
    {
        float tempRandom = 0;
        //for (int i = 0; i < 6 - fCurrentHealth / fMaxHealth * 4; i++)
        for (int i = 0; i < 6; i++)
        {
            lineList[i].positionCount = 2;
            if (i % 3 != 0)
            {
                tempRandom = Random.Range(Mathf.Max(Mathf.FloorToInt(player.position.x - 3), Mathf.FloorToInt(lineRange[0].x)), Mathf.Min(Mathf.FloorToInt(player.position.x + 4), Mathf.FloorToInt(lineRange[1].x + 1)));
                for (int j = 1; j < i; j++)
                {
                    if (j % 3 == 0) continue;
                    while (tempRandom == lineList[j].GetPosition(0).x - 0.5f)
                    {
                        tempRandom = Random.Range(Mathf.Max(Mathf.FloorToInt(player.position.x - 3), Mathf.FloorToInt(lineRange[0].x)), Mathf.Min(Mathf.FloorToInt(player.position.x + 4), Mathf.FloorToInt(lineRange[1].x + 1)));
                        j = 1;
                    }
                }
                Debug.Log(tempRandom);
                lineList[i].SetPosition(0, new Vector3(tempRandom + 0.5f, lineRange[0].y + 1f, 0));
                lineList[i].SetPosition(1, new Vector3(tempRandom + 0.5f, lineRange[1].y, 0));
            }
            else
            {
                tempRandom = Random.Range(Mathf.Max(Mathf.FloorToInt(player.position.y - 2), Mathf.FloorToInt(lineRange[1].y)), Mathf.Min(Mathf.FloorToInt(player.position.y + 3), Mathf.FloorToInt(lineRange[0].y + 1)));
                for (int j = 0; j < i; j++)
                {
                    if (j % 3 != 0) continue;
                    while (tempRandom == lineList[j].GetPosition(0).y - 0.5f)
                    {
                        tempRandom = Random.Range(Mathf.Max(Mathf.FloorToInt(player.position.y - 2), Mathf.FloorToInt(lineRange[1].y)), Mathf.Min(Mathf.FloorToInt(player.position.y + 3), Mathf.FloorToInt(lineRange[0].y + 1)));
                        j = 0;
                    }
                }
                Debug.Log(tempRandom);
                lineList[i].SetPosition(0, new Vector3(lineRange[0].x, tempRandom + 0.5f, 0));
                lineList[i].SetPosition(1, new Vector3(lineRange[1].x + 1f, tempRandom + 0.5f, 0));
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    public IEnumerator PatternNineShot()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 6; i++)
        {
            lineList[i].startWidth = 1.8f;
            lineList[i].endWidth = 1.8f;
            if (i % 3 != 0)
            {
				attackLine[1].gameObject.SetActive(true);
                attackLine[1].transform.position = new Vector3(lineList[i].GetPosition(0).x, (lineRange[0].y + 1 + lineRange[1].y) * 0.5f, 0);
                yield return new WaitForSeconds(0.15f);
				attackLine[1].gameObject.SetActive(false);
			}
			else
            {
				attackLine[0].gameObject.SetActive(true);
                attackLine[0].transform.position = new Vector3((lineRange[0].x + lineRange[1].x + 1) * 0.5f, lineList[i].GetPosition(0).y, 0);
				yield return new WaitForSeconds(0.15f);
				attackLine[0].gameObject.SetActive(false);
			}
			lineList[i].startWidth = 1f;
            lineList[i].endWidth = 1f;
            lineList[i].positionCount = 0;
        }
        enemyAI.AttackEnd();
        enemyAI.IdleTimeChk(enemyAI.fIdleTime);
        this.PatternCoolDownSetting(9);
    }


}
