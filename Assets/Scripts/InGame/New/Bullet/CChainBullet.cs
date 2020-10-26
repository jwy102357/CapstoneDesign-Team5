using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CChainBullet : CBullet
{
    private Collider2D[] findTargetList;
    private int enemyLayerMask;
    private LineRenderer lineRenderer;
    private int maxBounding = 3;
    public override void Awake()
    {
        base.Awake();
        this.bIsEnemyBullet = false;
        enemyLayerMask = 1 << LayerMask.NameToLayer(KDefine.LAYER_ENEMY);
        lineRenderer = this.GetComponent<LineRenderer>();
        findTargetList = new Collider2D[maxBounding];
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.gameObject.activeSelf)
        {
            if (!collision.gameObject.CompareTag(KDefine.TAG_PLAYER) && !collision.gameObject.CompareTag(KDefine.TAG_PLATFORM))
            {
                Boom(collision);
            }
        }
    }

    protected void Boom(Collision2D collision)
    {
        if (this.gameObject.activeSelf)
        {
            if (!collision.transform.CompareTag(KDefine.TAG_TILE))
            {
                Physics2D.OverlapCircleNonAlloc(this.transform.position, 5f, findTargetList, enemyLayerMask);
                if (findTargetList[0] != null)
                {
                    int cnt = 0;
                    for (int i = 0; i < findTargetList.Length; i++)
                    {
                        if (findTargetList[i] == null)
                        {
                            continue;
                        }
                        cnt++;
                        lineRenderer.positionCount = cnt;
                        Debug.Log(lineRenderer.positionCount);
                        lineRenderer.SetPosition(i, findTargetList[i].transform.position + new Vector3(0f, 1f, 0f));
                        IDamageable chainTarget = findTargetList[i].GetComponent<IDamageable>();

                        if (chainTarget != null)
                        {
                            Vector3 hitnormal = collision.transform.position - this.transform.position;
                            chainTarget.OnDamage(fDamage, hitnormal);
                        }

                        findTargetList[i] = null;
                    }
                }

                Function.LateCall((oParams) =>
                {
                    lineRenderer.positionCount = 0;
                    CRangedObjectManager.Instance.PushRangedObject(bulletName, this.gameObject);
                }, 0.03f);
            }

            else if (collision.transform.CompareTag(KDefine.TAG_TILE))
            {
                CRangedObjectManager.Instance.PushRangedObject(bulletName, this.gameObject);
            }
        }
    }

    public void OnDisable()
    {
        for (int i = 0; i < findTargetList.Length; i++)
        {
            findTargetList[i] = null;
        }

        lineRenderer.positionCount = 3;
    }

    public int FindMaxEnemyBouncing()
    {
        int res = 0;
        for (int i = 0; i < findTargetList.Length; i++)
        {
            if (findTargetList[i] == null)
            {
                res = i;
            }
        }

        return res;
    }
}
