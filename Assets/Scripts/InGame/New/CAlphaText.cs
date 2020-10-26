using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAlphaText : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer alphaSprite;
    public float fTimeDelay = 1f;
    void Start()
    {
        StartCoroutine(AlphaModulate());
    }

    private IEnumerator AlphaModulate()
    {
        var spriteColor = alphaSprite.color;
        int nValue = -1;
        while (this.gameObject != null)
        {
            spriteColor.a += (Time.deltaTime / fTimeDelay) * nValue;
            spriteColor.a = Mathf.Clamp(spriteColor.a, 0f, 1f);

            if (spriteColor.a <= 0f || spriteColor.a >= 1f)
            {
                nValue *= -1;
            }

            alphaSprite.color = spriteColor;
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("End");
        yield break;
    }
}
