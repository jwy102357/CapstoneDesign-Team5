using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALMB02EnemyController : CRangedEnemyController
{
    Transform player;
    private int tempRandom;
    private BoxCollider2D[] attackLine;
    public LineRenderer[] lineList;
    public Vector3[] lineRange;

    public override void Awake()
    {
        base.Awake();
        player = GameObject.Find(KDefine.TAG_PLAYER).GetComponent<Transform>();
        tempRandom = 0;
        lineList = GetComponentsInChildren<LineRenderer>();
        lineRange = new Vector3[2];
        attackLine = transform.Find("AttackLine").GetComponentsInChildren<BoxCollider2D>();
    }

    private void Start()
    {
        lineRange[0] = transform.position + Vector3.left * 15 + Vector3.up * 6;
        lineRange[1] = transform.position + Vector3.right * 14 + Vector3.down * 3;
        attackLine[0].size = new Vector2(lineRange[1].x + 1 - lineRange[0].x, 0.5f);
        attackLine[1].size = new Vector2(0.5f, lineRange[0].y + 1 - lineRange[1].y);
        attackLine[0].gameObject.SetActive(false);
        attackLine[1].gameObject.SetActive(false);
    }


    public override void OnDisable()
    {
        base.OnDisable();
        for(int i = 0; i < lineList.Length; i++)
        {
            lineList[i].positionCount = 0;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void AttackObject(int nDiceValue)
    {
        base.AttackObject(nDiceValue);
        if (nDiceValue == 1)
        {
            StartCoroutine("MakeDangerLine");
            StartCoroutine("Shot");
        }
    }
    
    public IEnumerator MakeDangerLine()
    {
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

    public IEnumerator Shot()
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
    }

    public void Move()
    {
        transform.Translate(player.position - transform.position);
    }

    public void MoveShot()
    {
        for (int i = 0; i < 8; i++)
        {
            var bullet = CRangedObjectManager.Instance.GetRangedObject("EnemyBullet");
            bullet.transform.position = transform.position + Vector3.up;
            Vector2 dir = new Vector3(Mathf.Cos(i * 0.25f * Mathf.PI), Mathf.Sin(i * 0.25f * Mathf.PI), 0).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = dir * fBulletSpeed;
        }
    }

    /*
    void Shot()
    {
        bIsShotReady = false;
        lineRenderer.positionCount = 0;
        var bullet = CRangedObjectManager.Instance.GetRangedObject("Enemy");
        bullet.transform.position = lineRenderer.transform.position;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(KDefine.LAYER_ENEMY), LayerMask.NameToLayer(KDefine.LAYER_ENEMYBULLET));
        Vector2 dir = this.GetDirection().normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = dir.normalized * fBulletSpeed;
    }
    */
}
