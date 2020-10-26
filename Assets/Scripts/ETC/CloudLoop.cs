using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudLoop : MonoBehaviour
{
    private float width;
    // Start is called before the first frame update
    void Awake()
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        width = boxCollider2D.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x <= -width)
        {
            Reposition();
        }
    }

    void Reposition()
    {
        Vector2 offset = new Vector2(width * 2f, 0);
        transform.position = (Vector2)transform.position + offset;
    }
}
