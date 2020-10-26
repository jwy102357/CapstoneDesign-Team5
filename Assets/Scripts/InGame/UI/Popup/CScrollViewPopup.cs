using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CScrollViewPopup : CBasePopup
{
    GameObject scrollViewObject;
    ScrollRect scrollRect;
    public float fScrollSpeed;
    void Awake()
    {
        scrollRect = this.GetComponent<ScrollRect>();
        this.scrollViewObject = this.scrollRect.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (scrollRect.verticalNormalizedPosition > 0.0f)
        {
            scrollRect.verticalNormalizedPosition -= fScrollSpeed;
        }

    }

    public void ResetScroll()
    {
        this.scrollRect.verticalNormalizedPosition = 1.0f;
        this.scrollViewObject.transform.localPosition = new Vector2(0.0f, 0.0f);
    }

    public void EndScroll()
    {
        this.scrollViewObject.SetActive(false);
        this.scrollViewObject.transform.localPosition = new Vector2(0.0f, 10000.0f);

    }

    public override void AppearPopup()
    {
        this.gameObject.SetActive(true);
    }

    public override void DisAppearPopup()
    {
        this.gameObject.SetActive(false);
    }
}
