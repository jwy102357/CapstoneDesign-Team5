using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CBasePopup : MonoBehaviour
{
	protected string oTitleString = "";

	public abstract void AppearPopup();

	public abstract void DisAppearPopup();
}
