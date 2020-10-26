using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBox : MonoBehaviour, IPushable
{
    Transform myTransform;
    float fSpeed = 3f;
    bool bIsContact = false;
    Rigidbody2D myRigidbody;
    public bool IsContact
    {
        get
        {
            return bIsContact;
        }

        set
        {
            bIsContact = value;
        }
    }

    public void Push(float fDir)
    {
        bIsContact = true;
        if (bIsContact)
        {
            myRigidbody.MovePosition(new Vector2(myRigidbody.position.x + fDir * Time.deltaTime , myRigidbody.position.y));
        }
    }

    void Awake()
    {
        myTransform = this.transform;
        myRigidbody = this.GetComponent<Rigidbody2D>();
    }
}
