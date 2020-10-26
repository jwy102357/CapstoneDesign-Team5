using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSubEnemySpawn : MonoBehaviour
{
    [System.Serializable]
    public struct stEnemy
    {
        public CBaseEnemyController enemyOrigin;
        public Vector2 enemyPosition;
        [HideInInspector]
        public CBaseEnemyController cloneEnemy;
    }
    public stEnemy[] enemies;
    public bool isAllSpawn = false;
    public bool spawnable = true;
    public bool isGuardCreate;
    public GameObject guardObject;
    public bool bIsClear = false;
    private string oString;

    void Awake()
    {
        oString = "Fire";
        if (isGuardCreate)
        {
            guardObject = this.transform.parent.parent.Find("Grid").Find("Tilemap(Guard)").gameObject;
        }
    }

    public void AllEnemySpawn()
    {
        for (int i = 0; i < enemies.Length; ++i)
        {
            this.EnemySpawn(enemies[i].enemyOrigin, enemies[i].enemyPosition, i);
        }

        isAllSpawn = true;
        Function.LateCall((oParams) =>
        {
            StartCoroutine(EnemyAllDead());
        }, 1.1f);
    }

    private void EnemySpawn(CBaseEnemyController enemy, Vector2 pos, int index)
    {
        StartCoroutine(Spawn(enemy, 1.0f, pos, index));
        //enemy.gameObject.SetActive(true);
    }

    private IEnumerator Spawn(CBaseEnemyController enemy, float delaytime, Vector2 pos, int index)
    {
        var b = (Vector2)this.transform.position + pos;
        CEffectManager.Instance.GetEffect("DeathBoom_01", b);
        yield return new WaitForSeconds(delaytime);
        var enemyClone = CEnemyManager.Instance.GetEnemyObject(enemy.name);
        enemyClone.name = enemy.name;
        enemyClone.transform.position = b;
        enemies[index].cloneEnemy = enemyClone.GetComponent<CBaseEnemyController>();
    }

    private IEnumerator EnemyAllDead()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].cloneEnemy == null)
            {
                i -= 1;
                yield return new WaitForEndOfFrame();
                continue;
            }
            //Debug.Log(enemies[i].cloneEnemy.IsDead);
            if (!enemies[i].cloneEnemy.IsDead) i -= 1;
            yield return new WaitForEndOfFrame();
        }


        this.gameObject.SetActive(false);
        if (guardObject != null && guardObject.activeSelf) guardObject.SetActive(false);
        this.bIsClear = true;
        yield return null;
    }

    public void StageEndSetting()
    {
        this.isAllSpawn = false;
        this.spawnable = true;
        this.bIsClear = false;
        this.gameObject.SetActive(true);
        if (guardObject != null) guardObject.SetActive(false);
        StopCoroutine(EnemyAllDead());
    }

    public void LastSaveSetting()
    {
        this.isAllSpawn = true;
        this.spawnable = false;
        this.bIsClear = true;
        this.gameObject.SetActive(false);
        guardObject.SetActive(false);
        StopCoroutine(EnemyAllDead());
    }
}
