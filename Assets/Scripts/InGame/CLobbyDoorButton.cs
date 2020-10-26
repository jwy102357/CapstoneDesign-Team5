using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLobbyDoorButton : MonoBehaviour
{
    public Animator myanimator;
    public SpriteRenderer myRenderer;

    private void Start()
    {
        myanimator = transform.parent.GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            myRenderer.enabled = false;
            myanimator.SetBool("Open", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            myRenderer.enabled = true;
        }
    }
}
