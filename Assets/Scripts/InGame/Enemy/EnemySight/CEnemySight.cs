using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemySight : MonoBehaviour
{
    public bool IsAttackRange { get; set; }
    public bool IsdirectionChange { get; private set; }
    //Ray Check
    public Transform groundDetection;
    public Transform wallDetection;

    public RaycastHit2D groundInfo;
    public RaycastHit2D wallInfo;

    int groundLayerMask;
    int wallLayerMask;

    public void Start()
    {
        this.groundDetection = transform.Find("GroundDetection");
        this.wallDetection = transform.Find("WallDetection");
        groundLayerMask = (1 << LayerMask.NameToLayer(KDefine.LAYER_PLATFORM)) + (1 << LayerMask.NameToLayer(KDefine.LAYER_GROUND));
        wallLayerMask = 1 << LayerMask.NameToLayer(KDefine.LAYER_WALL);
    }
    public void Update()
    {
        groundInfo = Physics2D.Raycast(this.groundDetection.position, Vector2.down, 1f, groundLayerMask);
        wallInfo = Physics2D.Raycast(this.wallDetection.position, Vector2.left, 1f, wallLayerMask);

        if (groundInfo.collider == false || wallInfo.collider == true)
        {
            IsdirectionChange = true;
        }

        else
        {
            IsdirectionChange = false;
        }
    }

    public void OnDisable()
    {
        IsdirectionChange = false;
        IsAttackRange = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(KDefine.TAG_PLAYER))
        {
            IsAttackRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(KDefine.TAG_PLAYER))
        {
            IsAttackRange = false;
        }
    }
}
