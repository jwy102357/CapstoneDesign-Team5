using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CJumpButton : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler 
{
    public bool IsJump { get; set; }
    
    void Awake()
    {
        IsJump = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsJump = true;
        StartCoroutine(JumpEnd());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsJump = false;
    }


    IEnumerator JumpEnd()
    {
        yield return new WaitForEndOfFrame();
        IsJump = false;
    }

}
