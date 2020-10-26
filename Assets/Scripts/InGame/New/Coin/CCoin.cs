using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class CCoin : CItem
{
    int coinValue;

    private void Awake()
    {
        gravityScale = 1f;
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = gravityScale;
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
        coinValue = 0;
        rigidbody.velocity = Vector2.zero;
        gameObject.layer = 17;
    }

    public void OnEnter(int value)
    {
        coinValue = value;

        if(coinValue == 1)
        {
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        else if(coinValue == 10)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if(coinValue == 100)
        {
            transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }

        float xRange = Random.Range(-0.4f, 0.4f);
        float yRange = Random.Range(1f, 3f);
        rigidbody.AddForce(new Vector3(xRange, yRange, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(KDefine.TAG_PLAYER))
        {
            CMerchandiseManager.Instance.PlusCoin(coinValue);
            CMerchandiseManager.Instance.PushCoinObject(KDefine.MONEY_COIN, this.gameObject);
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
                    if(gameObject.activeSelf)
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
