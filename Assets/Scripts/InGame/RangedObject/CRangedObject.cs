using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CRangedObject : MonoBehaviour
{
    protected int nAtk;
    protected float fSpeed;
    protected SpriteRenderer spriteRenderer;
    protected Vector3 startPosition;
    protected Vector3 readyPosition;
    protected GameObject objectPrefab;
    protected GameObject objectParent;
    protected Rigidbody2D rigidbody;

    public bool isUse;
    public Transform attackPivot;

    public abstract void ReloadObject();

    public abstract void ThrowingObject(Vector3 target, Vector3 enemy);

    public abstract void CreatObject(GameObject gameObject);

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(KDefine.TAG_PLAYER))
        {
            //PlayerInstance.damage(atk) Do Something
            ReloadObject();
        }

        else if (collider.CompareTag(KDefine.TAG_TILE))
        {
            ReloadObject();
        }
    }
}
