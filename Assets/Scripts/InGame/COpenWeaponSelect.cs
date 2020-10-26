using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COpenWeaponSelect : MonoBehaviour
{
    private float lastDownTime;
    private Vector3 OriginalPosition;
    private bool bOpenable = true;
    private bool bMoveable = false;
    private bool bPushable = true;

    public float movingTime = 0.5f;
    public float movingDistance = 0.35f;
    public float movingCut = 10;
    public GameObject targetObject;
    public SpriteRenderer renderer;

    private void Start()
    {
        OriginalPosition = transform.position;
        lastDownTime = Time.time;
        renderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (bMoveable)
        {
            Moving();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y <= -0.9f)
        {
            if(collision.collider.CompareTag(KDefine.TAG_PLAYER))
            {
                collision.transform.SetParent(transform);
                bMoveable = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.CompareTag(KDefine.TAG_PLAYER))
        {
            collision.transform.SetParent(null);
            transform.position = OriginalPosition;
            bMoveable = false;
            bOpenable = true;
        }
    }

    private void Moving()
    {
        if (transform.position.y > OriginalPosition.y - movingDistance)
        {
            if (Time.time > lastDownTime + movingTime / (movingCut - 1))
            {
                lastDownTime = Time.time;
                transform.position = new Vector3(transform.position.x, transform.position.y - movingDistance / movingCut);
            }
        }
        else if (bOpenable)
        {
            bOpenable = false;
            if (targetObject == null)
            {
                GameObject.Find("Canvas_UI").transform.Find("WeaponSelectPopup").gameObject.SetActive(true);
            }
            else
            {
                targetObject.SetActive(true);
            }
        }
    }
}
