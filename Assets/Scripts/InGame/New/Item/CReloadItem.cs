using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CReloadItem : CItem, IItem
{
	public int nCount;
	
	public void Use()
	{
		CWeaponManager.Instance.GetCurrentWeapon().ReloadBullet(nCount);
		CItemManager.Instance.PushObject("Bullets", this.gameObject);
	}
}
