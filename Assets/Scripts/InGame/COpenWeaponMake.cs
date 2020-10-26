using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COpenWeaponMake : CInteractable
{
    public override void InteractAction()
    {
        GameObject.Find("Canvas_UI").transform.Find("WeaponMakePopup").gameObject.SetActive(true);
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
