using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSettingPopup : CBasePopup
{
	public GameObject feedBackObejct;
	Text oBGMText;
	Text oEffectVolumeText;
	CScrollViewPopup scrollViewDeveloper;
	GameObject scrollViewObject;
	private void Start()
	{
		oBGMText = GameObject.Find("BGMText").GetComponent<Text>();
		oEffectVolumeText = GameObject.Find("EffectVolumeText").GetComponent<Text>();
		scrollViewObject = CPopupManager.Instance.GetObjectForKey("Developer");
		scrollViewDeveloper = scrollViewObject.GetComponent<CScrollViewPopup>();
	}

	public override void AppearPopup()
	{
		this.gameObject.SetActive(true);
	}

	public override void DisAppearPopup()
	{
		this.gameObject.SetActive(false);
	}

	public void ClickBGMButton()
	{
		CSoundManager.Instance.BackgroundMute = !CSoundManager.Instance.BackgroundMute;
		if (!CSoundManager.Instance.BackgroundMute)
		{
			oBGMText.text = "켜짐";
		}

		else
		{
			oBGMText.text = "꺼짐";
		}

		Debug.Log("브금 눌렷다");
	}

	public void ClickEffectVolumeButton()
	{
		CSoundManager.Instance.EffectMute = !CSoundManager.Instance.EffectMute;

		if (!CSoundManager.Instance.EffectMute)
		{
			this.oEffectVolumeText.text = "켜짐";
		}

		else
		{

			this.oEffectVolumeText.text = "꺼짐";
		}

		Debug.Log("이펙트 눌렷다");
	}

	public void ClickFeedBackButton()
	{
		feedBackObejct.SetActive(true);
	}

	public void ClickDeveloperButton()
	{
		this.scrollViewObject.SetActive(true);
		this.scrollViewDeveloper.ResetScroll();
	}

	public void ClickCodeButton()
	{
		CPopupManager.Instance.ShowPopup("Code");
		Debug.Log("코드 눌렷다");
	}
}
