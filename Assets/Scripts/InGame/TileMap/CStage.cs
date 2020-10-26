using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CStage : MonoBehaviour
{
	public Transform startTransform;
	public Transform nextDungeonEntrance;
	public Transform itemCreateTransform;
	public CEnemySpawn[] enemySpawn;
	public bool bIsStageEnd = false;
	public Vector2 StartPos { get; private set; }
	public Vector2 nextDungeonPos { get; private set; }
	void Awake()
	{
		enemySpawn = this.GetComponentsInChildren<CEnemySpawn>();
		StartPos = this.transform.position + startTransform.localPosition;
		nextDungeonPos = this.transform.position + nextDungeonEntrance.localPosition;
	}

	public void StageEnd()
	{
		for (int i = 0; i < enemySpawn.Length; i++)
		{
			enemySpawn[i].StageEndSetting();
		}
	}

	private void OnEnable()
	{
		StartCoroutine(EnemyAllDead());
	}

	private int EnemyAllDeadCheck()
	{
		int nDeathCount = this.enemySpawn.Length;

		for (int i = 0; i < this.enemySpawn.Length; ++i)
		{
			if (!enemySpawn[i].gameObject.activeSelf)
			{
				nDeathCount--;
			}
		}
		return nDeathCount;
	}

	private IEnumerator EnemyAllDead()
	{
		while (this.EnemyAllDeadCheck() > 0)
		{
			yield return new WaitForSeconds(Time.deltaTime);
		}

		yield return new WaitForSeconds(1.0f);
		this.bIsStageEnd = true;
		Debug.Log("열려라 참꺠! " + this.gameObject);
		yield return null;
	}

	public void LastSaveStage()
	{
		for (int i = 0; i < enemySpawn.Length; i++)
		{
			enemySpawn[i].LastSaveSetting();
		}
	}

	public void OnDisable()
	{
		this.bIsStageEnd = false;
	}
}
