using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CLocalize : CComponent
{
	private Text _oText = null;
	private string _oLocalizeKey = "";

	public override void Awake()
	{
		base.Awake();

		_oText = this.GetComponentInChildren<Text>();

		Function.LateCall((oParams) =>
		{
			_oLocalizeKey = _oText.text;
			this.ResetLocalize();
		}, 0.0f);
	}

	public void ResetLocalize()
	{
		_oText.text = CLocalizeManager.Instance.GetStringForKey(_oLocalizeKey);
	}
}
