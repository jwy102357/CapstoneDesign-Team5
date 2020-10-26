using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGameOverPopup : CBasePopup
{
	public Image fillGauge;
	public Transform playerHead;
	public GameObject lobbyReturnBtn;
	public GameObject adBtn;
	private float fClearRate;
	public override void AppearPopup()
	{
		//390
		this.gameObject.SetActive(true);
		StartCoroutine(FillGauge());
		Function.StopGame();
	}

	public void OnEnable()
	{
		StartCoroutine(FillGauge());
		Function.StopGame();
	}

	public override void DisAppearPopup()
	{
		Function.StartGame();
		this.gameObject.SetActive(false);
		lobbyReturnBtn.SetActive(false);
		adBtn.SetActive(false);
	}

	public void	OnTouchADBtn()
	{
		this.DisAppearPopup();
		//플레이어 살리는 함수를 만들고 셋hp부분 없앰.
		//Player Fix CPlayer.Instance.SetHP(100);
		//Player Fix CPlayer.Instance.characterState = CCharacter.CharacterState.IDLE;
		Function.StartGame();
		//DO SOMETHING -> GO TO AD 
	}

	public void OnTouchLobbyRetrunBtn()
	{
		this.DisAppearPopup();
		CEnemyPatternManager.Instance.playerObj.SendMessage("EndGame", SendMessageOptions.DontRequireReceiver);
	}

	public IEnumerator FillGauge()
	{
		//fclearRate 넣기
		while(fillGauge.fillAmount < 1f)
		{
			var point = this.playerHead.localPosition;
			fillGauge.fillAmount += Time.unscaledDeltaTime * 0.3f;
			point.x = (fillGauge.gameObject.transform.localPosition.x) - (fillGauge.gameObject.transform.localPosition.x * 2f * fillGauge.fillAmount);
			this.playerHead.localPosition = point;
			yield return new WaitForEndOfFrame();
		}

		lobbyReturnBtn.SetActive(true);
		adBtn.SetActive(true);

		//this.DisAppearPopup();
		//Function.StartGame();
		//Initiate.Fade("PlazaScene", Color.black, 0.5f);

		Debug.Log("Game Over");
		//DO SOMETHING -> GO TO WAIT SCENE
	}
}
