using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMaterialItem : CItem
{
    public List<Sprite> itemSpriteList = new List<Sprite>();

    enum EItem
    {
        E_BATTERY = 1,
        E_BOLT,
        E_IRONORE,
        E_IRONPLATE,
        E_NUT,
        E_OLDALIENGUN = 6,
    }

    SpriteRenderer renderer;
    int itemNum;

    private void Awake()
    {
        gravityScale = 1f;
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = gravityScale;
        itemName = "MaterialItem";
    }

    private void OnDisable()
    {
        bIsContact = false;
        bIsGrounded = false;
        rigidbody.gravityScale = gravityScale;
        rigidbody.velocity = Vector2.zero;
        gameObject.layer = 17;
    }

    private void OnEnable()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        itemNum = Random.Range(0, 6);
        SetItem(itemNum);
    }

    public void SetItem(int num)
    {
        renderer.sprite = itemSpriteList[num];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(KDefine.TAG_PLAYER))
        {
            // 세이브 데이터에 아이템 갯수 추가
            CMixManager.Instance.PlayerMaterialPlus((EMatType)itemNum, 1);

            CItemManager.Instance.PushObject(itemName, this.gameObject);
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
