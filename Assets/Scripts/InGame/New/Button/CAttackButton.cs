using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CAttackButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler
{
    public bool IsAttack { get; set; }
    public bool IsInteract { get; set; }
    public Sprite attackSprite;
    public Sprite interactSprite;
    public Image currentImage;
    private Animator animator;

    public void Awake()
    {
        IsAttack = false;
        currentImage = this.transform.Find("Image").GetComponent<Image>();
        animator = GetComponentInChildren<Animator>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //if (!IsAttack && !IsInteractImage())
        //{
        //    IsAttack = true;
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsAttack && !IsInteractImage())
        {
            IsAttack = true;
        }

        else if (!IsAttack && IsInteractImage())
        {
            IsInteract = true;
            StartCoroutine(this.InteractEnd());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsAttack)
        {
            IsAttack = false;
        }

        if (IsInteract)
        {
            IsInteract = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsAttack)
        {
            IsAttack = false;
        }

        if (IsInteract)
        {
            IsInteract = false;
        }
    }

    public void ChangeInteractImage()
    {
        //currentImage.sprite = interactSprite;
        if(!animator.enabled)
        {
            animator.enabled = true;
        }
        animator.SetTrigger("ToInteract");
    }

    public void ChangeAttackImage()
    {
        //currentImage.sprite = this.attackSprite;
        if (!animator.enabled)
        {
            animator.enabled = true;
        }
        animator.SetTrigger("ToAttack");
    }
    
    public bool IsInteractImage()
    {
        return currentImage.sprite == interactSprite ? true : false;
    }

    private IEnumerator InteractEnd()
    {
        yield return new WaitForEndOfFrame();
        IsInteract = false;
    }

}
