using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLobbyPortalPopup : CInteractable
{
    CChapterSelectPopup popup;

    private void Awake()
    {
        popup = GameObject.Find("Canvas_UI").transform.Find("ChapterSelectPopup").GetComponent<CChapterSelectPopup>();
    }

    public override void InteractAction()
    {
        if(!popup.selected)
        {
            popup.gameObject.SetActive(true);
        }
    }

    public void SetActivePortal()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<CMovement>().interactPopup = this;
            collision.GetComponent<CInput>().AttackButton.ChangeInteractImage();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<CInput>().AttackButton.ChangeAttackImage();
            collision.GetComponent<CMovement>().interactPopup = null;
        }
    }
}
