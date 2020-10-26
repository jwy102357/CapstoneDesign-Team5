using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFlash : MonoBehaviour
{
    private SpriteRenderer myRenderer;
    private Color myColor;

    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("FlashOff");
    }

    IEnumerator FlashOn()
    {
        while (myColor.a < 1.0f)
        {
            myColor = myRenderer.color;
            myColor.a += 0.1f;
            myRenderer.color = myColor;
            yield return new WaitForSeconds(0.07f);
        }
        StartCoroutine("FlashOff");
    }

    IEnumerator FlashOff()
    {
        while(myColor.a > 0.0f)
        {
            myColor = myRenderer.color;
            myColor.a -= 0.1f;
            myRenderer.color = myColor;
            yield return new WaitForSeconds(0.07f);
        }
        StartCoroutine("FlashOn");
    }
}
