using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCodePopup : CBasePopup
{
	public override void AppearPopup()
	{
		this.gameObject.SetActive(true);
	}

	public override void DisAppearPopup()
	{
		this.gameObject.SetActive(false);
	}

	public void ClickCheckButton(Text oSender)
	{
		if (oSender.text == "aaabbbccc")
		{
			Debug.Log("성공");
		}

		else
		{
			Debug.Log("코드가 안 맞아 슈발");
		}

		Debug.Log(oSender.text);
		this.gameObject.SetActive(false);
	}
}
