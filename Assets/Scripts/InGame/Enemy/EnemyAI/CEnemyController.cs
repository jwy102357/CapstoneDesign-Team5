using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyController : CBaseEnemyController
{
    public BoxCollider2D oWeapon;
    float fSpeed = 3.0f;

    public override void Awake()
    {
        base.Awake();
        if (enemyObject)
        {
            oWeapon = this.transform.Find("Weapon").GetComponent<BoxCollider2D>();
            oWeapon.enabled = false;
        }
    }
        
    public override void TurnObject()
    {
            enemyAI.FacingRight = !enemyAI.FacingRight;
            animator.SetBool("FacingRight", enemyAI.FacingRight);
            enemyObject.transform.localScale = new Vector3(enemyObject.transform.localScale.x * -1f, 1f, 1f);
    }

    public override void MoveForwardObject(Vector3 moveDirection)
    {
        //Animator SetFloat or trigger? move animation play  DO SOMETHING
        animator.SetFloat("Speed", fSpeed);
        this.transform.Translate(GetDirection() * (this.fSpeed * Time.deltaTime));
    }

    public override void AttackObject(int nDiceValue)
    {
        animator.SetFloat("Speed", 0.0f);
        animator.SetInteger("AttackState", nDiceValue);
        Debug.Log("공격이다!");
        /*oWeapon.enabled = true;
        oWeapon.isTrigger = true;

        Function.LateCall((oParams =>
        {
            oWeapon.enabled = false;
            oWeapon.isTrigger = false;
        }), 0.5f);*/

        switch (nDiceValue)
        {
            case 1:
                break;

            case 2:
                break;

            case 3:
                break;

            default:
                Debug.Log("다이스 범위 초과!");
                break;
        }
    }

    public Vector3 GetDirection()
    {
        return this.enemyAI.FacingRight ? Vector3.right : Vector3.left;

    }

    public void AttackTime()
    {
        oWeapon.enabled = true;
        oWeapon.isTrigger = true;

        Function.LateCall((oParams =>
        {
            oWeapon.enabled = false;
            oWeapon.isTrigger = false;
        }), 0.26f);
    }
}

