using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMoneyText : MonoBehaviour
{
	Text moneyText;
	RectTransform rectTransform;

	void Start()
	{
		moneyText = this.GetComponent<Text>();
		CMerchandiseManager.Instance.moneyText = this;
		this.SetText(CMerchandiseManager.Instance.GetCoinAmount());
		rectTransform = this.moneyText.GetComponent<RectTransform>();
	}

	public void SetText(int nValue)
	{
		this.moneyText.text = nValue.ToString();
		//rectTransform.rect.width Image 위치에 따라 조정해야된다.
	}


	
}
