using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CAddWeaponTest : MonoBehaviour, IPointerClickHandler
{
    public List<CWeapon> weaponList;
    public int nIndex = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        CWeaponManager.Instance.AddWeapon(weaponList[nIndex]);
        nIndex = nIndex >= 2 ? 0 : nIndex + 1;
        Debug.Log(nIndex);
    }

}
