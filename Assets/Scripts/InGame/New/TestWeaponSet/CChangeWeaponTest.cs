using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CChangeWeaponTest : MonoBehaviour, IPointerClickHandler
{
    void Start()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CWeaponManager.Instance.ChangeWeapon();
    }

}
