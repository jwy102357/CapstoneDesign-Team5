using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//스왑 버튼 있고
//총의 갯수제한 변동 가능해야되고
//버튼을 누르는 순간 바뀌어야됨 

//gunName 키값을 확인한다.
//존재하냐? -> //gunName을 갖고온다.
//없다 -> 딱총 기본 총의 건네임으로 설정한다.
//autotestgun의 건네임을 딱총으로 바꾼다.
//save한다.
//이것을 슈터에서 currentWeapon한다.
//만약 총 선택팝업에서 총을 선택한다면 gunName키값을 수정한다.
//autotestgun의 네임을 바꾼 총으로 바꾼다.
//총 이미지 바꾼다.

public class CShooter : MonoBehaviour
{
    public CInput playerInput;
    //웨펀 클래스 가지고 웨펀에서 샷
    public CWeapon weapon;
    public bool bIsAttackTime;

    private Transform gunSlot;
    
    private void Awake()
    {
        playerInput = this.GetComponent<CInput>();
        gunSlot = this.transform.Find("GunSlot");
        CWeaponManager.Instance.playerShooter = this;
        CWeaponManager.Instance.weaponParent = this.gunSlot;
        if (SceneManager.GetActiveScene().name != "LobbyScene")
        {
            CWeaponManager.Instance.listClear();
            CWeaponManager.Instance.LoadWeaponList();
            SetBonusDamage(1.0f);
        }
    }

    void Update()
    {
        if (weapon.gameObject.activeSelf)
        {
            if (this.playerInput.AttackButton.IsAttack)
            {
                weapon.Attack();
            }

            else
            {
                if (weapon.GetWeaponCategory() == EWeaponCategory.E_CHARGEGUN)
                {
                    weapon.Shot();
                }
            }
        }
    }

    public void SetBonusDamage(float Damage)
    {
        weapon.SetBonusDamage(Damage);
    }

    public void SetCurrentWeapon(CWeapon changeWeapon)
    {
        weapon = changeWeapon;
        weapon.SetGunImageSetting();
    }
}
