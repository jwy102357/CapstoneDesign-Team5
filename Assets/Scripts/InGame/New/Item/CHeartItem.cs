using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHeartItem : CItem
{
    public int nAddHealthCount = 1;

    private void Awake()
    {
        gravityScale = 1f;
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = gravityScale;
        itemName = KDefine.ITEM_HEART;
    }

    private void OnEnable()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void OnDisable()
    {
        bIsContact = false;
        bIsGrounded = false;
        rigidbody.gravityScale = gravityScale;
        rigidbody.velocity = Vector2.zero;
        gameObject.layer = 17;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(KDefine.TAG_PLAYER))
        {
            collision.gameObject.GetComponent<CPlayerHealth>().RestoreHealth(nAddHealthCount);
            CItemManager.Instance.PushObject(KDefine.ITEM_HEART, this.gameObject);
        }
        else if (collision.gameObject.CompareTag(KDefine.TAG_TILE) || collision.gameObject.CompareTag(KDefine.TAG_PLATFORM))
        {
            if (collision.contacts[0].normal.y > 0.7f)
            {
                rigidbody.velocity = Vector2.zero;
                if (!bIsGrounded)
                {
                    rigidbody.gravityScale = 0;
                    bIsGrounded = true;
                    if (gameObject.activeSelf)
                    {
                        StartCoroutine("ItemFloating");
                    }
                }
            }
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(KDefine.TAG_PLAYER))
        {
            if (!bIsContact)
            {
                bIsContact = true;
                if (bIsGrounded)
                {
                    StopCoroutine("ItemFloating");
                }
                StartCoroutine("Magnetic");
            }
        }
    }

}
