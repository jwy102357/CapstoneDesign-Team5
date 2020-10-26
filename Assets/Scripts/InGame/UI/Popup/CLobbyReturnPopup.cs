using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLobbyReturnPopup : CBasePopup
{
	public override void AppearPopup()
	{
		Function.StopGame();
		this.gameObject.SetActive(true);
	}

	public override void DisAppearPopup()
	{
		this.gameObject.SetActive(false);
	}

	public void OnTouchReturnButton()
    {
        Function.StartGame();
        CEnemyPatternManager.Instance.playerObj.SendMessage("EndGame", SendMessageOptions.DontRequireReceiver);
        CPopupManager.Instance.DisAppearPopup("Pause");
        this.DisAppearPopup();
    }

    public void OnTouchExitButton()
	{
		this.DisAppearPopup();
	}
}
