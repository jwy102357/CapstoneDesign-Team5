using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMovingPlatform : MonoBehaviour
{
    public float fMovingUpDistance = 0f;
    public float fMovingDownDistance = 0f;
    public float fMovingLeftDistance = 0f;
    public float fMovingRightDistance = 0f;
    public float fMovingSpeed = 1.0f;
    public bool bDirectionRight = true;
    public bool bDirectionUp = true;

    private Vector2 myStartPosition;
    private Rigidbody2D playerRigidbody;
    private CMovement playerMovement;

    void Start()
    {
        myStartPosition = GetComponent<Transform>().position;
        playerMovement = GameObject.Find("Player").GetComponent<CMovement>();
    }

    void Update()
    {
        DirectionCheck();
        if (bDirectionRight && (transform.position.x < myStartPosition.x + fMovingRightDistance) && bDirectionUp)
        {
            transform.Translate(transform.right * fMovingSpeed * Time.deltaTime);
        }
        else if (bDirectionUp && (transform.position.y < myStartPosition.y + fMovingUpDistance))
        {
            transform.Translate(transform.up * fMovingSpeed * Time.deltaTime);
        }
        else if (!bDirectionRight && (transform.position.x > myStartPosition.x - fMovingLeftDistance))
        {
            transform.Translate(-transform.right * fMovingSpeed * Time.deltaTime);
        }
        else if(!bDirectionUp && (transform.position.y > myStartPosition.y - fMovingDownDistance))
        {
            transform.Translate(-transform.up * fMovingSpeed * Time.deltaTime);
        }
    }

    void DirectionCheck()
    {
        if (transform.position.x <= myStartPosition.x - fMovingLeftDistance)
        {
            bDirectionRight = true;
        }
        else if (transform.position.x >= myStartPosition.x + fMovingRightDistance)
        {
            bDirectionRight = false;
        }
        if (transform.position.y <= myStartPosition.y - fMovingDownDistance)
        {
            bDirectionUp = true;
        }
        else if (transform.position.y >= myStartPosition.y + fMovingUpDistance)
        {
            bDirectionUp = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(KDefine.TAG_PLAYER))
        {
            playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if(!playerMovement.bIsDash)
            {
                collision.transform.SetParent(transform);
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(KDefine.TAG_PLAYER))
        {
            collision.transform.SetParent(null);
        }

    }
}
