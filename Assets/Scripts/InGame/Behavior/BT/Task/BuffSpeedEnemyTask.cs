using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpeedEnemyTask : BTNode
{
	CBuffEnemyController buffEnemy;
	CBaseEnemyController upEnemy;
	List<CBaseEnemyController> upEnemyList;
	public BuffSpeedEnemyTask(CBuffEnemyController buffEnemyController, CBaseEnemyController speedUpEnemy)
	{
		buffEnemy = buffEnemyController;
		upEnemy = speedUpEnemy;
		upEnemyList = new List<CBaseEnemyController>();
	}

	public void FindEnemy()
	{
		var enemySpawn = CStageManager.Instance.CurrentStage.enemySpawn[0].enemySubSpawner[0];

		for (int i = 0; i < CStageManager.Instance.CurrentStage.enemySpawn.Length; i++)
		{
			if (CStageManager.Instance.CurrentStage.enemySpawn[i].gameObject.activeSelf)
			{
				for (int j = 0; i < CStageManager.Instance.CurrentStage.enemySpawn[i].enemySubSpawner.Length; j++)
				{
					if (CStageManager.Instance.CurrentStage.enemySpawn[i].enemySubSpawner[j].isAllSpawn)
					{
						enemySpawn = CStageManager.Instance.CurrentStage.enemySpawn[i].enemySubSpawner[j];
						break;
					}
				}
			}
		}

		if (enemySpawn.isAllSpawn && enemySpawn.gameObject.activeSelf)
		{
			var enemyList = enemySpawn.enemies;
			if (enemyList.Length > 1)
			{
				for (int i = 0; i < enemyList.Length; i++)
				{
					if (enemyList[i].cloneEnemy.gameObject.activeSelf && !enemyList[i].cloneEnemy.GetComponent<CBaseEnemyController>().bIsBuff)
					{
						if (enemyList[i].cloneEnemy.fSpeed != 0 && enemyList[i].cloneEnemy.gameObject.GetComponent<CBuffEnemyController>() == null)
						{
							upEnemyList.Add(enemyList[i].cloneEnemy.GetComponent<CBaseEnemyController>());
						}
					}
				}
			}

			if (upEnemyList.Count >= 1)
			{
				int nRandom = Random.Range(0, upEnemyList.Count);
				upEnemy = upEnemyList[nRandom];
				buffEnemy.buffTarget = upEnemy;
				buffEnemy.Buff();
				upEnemyList.Clear();
			}
		}
	}

	public override BTState Evaluate()
	{
		upEnemy = null;
		FindEnemy();
		if (upEnemy == null)
		{
			return BTState.FAILURE;
		}
		return BTState.SUCCESS;
	}
}
