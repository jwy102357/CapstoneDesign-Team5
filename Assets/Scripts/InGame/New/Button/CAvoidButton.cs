using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CAvoidButton : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler
{
    public bool bDashButton { get; set; }
    
    void Awake()
    {
        bDashButton = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bDashButton = true;
        StartCoroutine(JumpEnd());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bDashButton = false;
    }


    IEnumerator JumpEnd()
    {
        yield return new WaitForEndOfFrame();
        bDashButton = false;
    }
}
