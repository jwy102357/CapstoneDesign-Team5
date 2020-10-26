using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CPausePopup : CBasePopup
{
	public Image soundBtn;
	public Image vibrateBtn;
	public Sprite[] onOffSprites;

	public override void AppearPopup()
	{
        if(!Initiate.areWeFading)
        {
            this.gameObject.SetActive(true);
            Function.StopGame();
        }
	}

	public override void DisAppearPopup()
    {
        Function.StartGame();
        this.gameObject.SetActive(false);
	}

	public void OnTouchLobbyReturnBtn()
	{
		CPopupManager.Instance.ShowPopup("LobbyReturn");
	}

	public void OnTouchSoundBtn()
	{
		bool bIsMute = CSoundManager.Instance.BackgroundMute;
		if (bIsMute)
		{
			soundBtn.sprite = onOffSprites[0];
			CSoundManager.Instance.BackgroundMute = false;
			CSoundManager.Instance.EffectMute = false;
		}

		else
		{
			soundBtn.sprite = onOffSprites[1];
			CSoundManager.Instance.BackgroundMute = true;
			CSoundManager.Instance.EffectMute = true;
		}
	}

	public void OnTouchVibrateBtn()
	{
        //DO SOMETHING Vibrate 해제 기능 생길시 적용
        vibrateBtn.sprite = vibrateBtn.sprite == onOffSprites[0] ? onOffSprites[1] : onOffSprites[0];
    }

}
