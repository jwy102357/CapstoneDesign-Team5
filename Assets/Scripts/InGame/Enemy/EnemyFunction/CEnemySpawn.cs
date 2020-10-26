using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CEnemySpawn : MonoBehaviour
{
    public CSubEnemySpawn[] enemySubSpawner;

    public void Awake()
    {
        enemySubSpawner = this.GetComponentsInChildren<CSubEnemySpawn>();
        Debug.Log(enemySubSpawner.Length);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(this.gameObject.name);

        if (collision.CompareTag(KDefine.TAG_PLAYER) && enemySubSpawner[0].spawnable)
        {
            if (enemySubSpawner[0].isGuardCreate) enemySubSpawner[0].guardObject.SetActive(true);
            Debug.Log(enemySubSpawner.Length);
            enemySubSpawner[0].AllEnemySpawn();
            enemySubSpawner[0].spawnable = false;
            this.GetComponent<BoxCollider2D>().isTrigger = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(this.EnemyAllDead());
        }
    }

    private IEnumerator EnemyAllDead()
    {
        for (int i = 0; i < enemySubSpawner.Length; ++i)
        {
            if (!enemySubSpawner[i].bIsClear)
            {
                i -= 1;
                yield return new WaitForEndOfFrame();
                continue;
            }

            else if (enemySubSpawner[i].bIsClear && i < enemySubSpawner.Length - 1)
            {
                if (enemySubSpawner[i + 1].isGuardCreate) enemySubSpawner[i + 1].guardObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                enemySubSpawner[i + 1].AllEnemySpawn();
                enemySubSpawner[i + 1].spawnable = false;
            }
        }

        this.gameObject.SetActive(false);
        yield return null;
    }

    public void StageEndSetting()
    {
        if (enemySubSpawner != null)
        {
            for (int i = 0; i < enemySubSpawner.Length; ++i)
            {
                enemySubSpawner[i].StageEndSetting();
            }
        }

        this.gameObject.SetActive(true);
        this.GetComponent<BoxCollider2D>().isTrigger = true;
        this.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void LastSaveSetting()
    {
        if (enemySubSpawner != null)
        {
            for (int i = 0; i < enemySubSpawner.Length; ++i)
            {
                enemySubSpawner[i].LastSaveSetting();
            }

            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
