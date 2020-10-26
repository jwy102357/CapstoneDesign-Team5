using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class CMerchandiseManager : CSingleton<CMerchandiseManager>
{
	private Dictionary<string, KeyValuePair<string, CObjectPool>> merchandiseDict;
	private StringBuilder keyString;
	private string oFilePath;
	private int nCoinAmount;
	private int nDiaAmount;
	private List<GameObject> coinList;
	public CMoneyText moneyText;

	private void Awake()
	{
		merchandiseDict = new Dictionary<string, KeyValuePair<string, CObjectPool>>();
		keyString = new StringBuilder();
		oFilePath = "Prefabs/Money/";
		coinList = new List<GameObject>();
	}

	public void PlusCoin(int nAmount)
	{
		nCoinAmount += nAmount;
		moneyText.SetText(nCoinAmount);
	}

	public void MinusCoin(int nAmount)
	{
		nCoinAmount -= nAmount;
		if (nCoinAmount < 0) nCoinAmount = 0;
		moneyText.SetText(nCoinAmount);
	}

	public void PlusDia(int nAmount)
	{
		nDiaAmount += nAmount;
	}

	public void MinusDia(int nAmount)
	{
		nDiaAmount -= nAmount;
		if (nDiaAmount < 0) nDiaAmount = 0;
	}

	public int GetCoinAmount()
	{
		return this.nCoinAmount;
	}

	public int GetDiaAmount()
	{
		return this.nDiaAmount;
	}

	public GameObject GetCoinObject(string objectName)
	{
		if (!merchandiseDict.ContainsKey(objectName))
		{
			this.PushKeyValue(objectName);
		}

		var filepath = merchandiseDict[objectName].Key;
		var pool = merchandiseDict[objectName].Value;

		return pool.VisibleObject(filepath, this.transform);
	}

	public void PushCoinObject(string objectName, GameObject pushObject)
	{
		this.merchandiseDict[objectName].Value.InVisbileObject(pushObject);
	}

	public void ManyCoinCreate(int nMinValue, int nMaxValue, Vector3 position)
	{
		int nRandom = Random.Range(nMinValue, nMaxValue + 1);
        Debug.Log(nRandom);
		StartCoroutine(CreateCoin(nRandom, position));
	}

	public void ManyCoinCreate(int nValue, Vector3 position)
	{
		StartCoroutine(CreateCoin(nValue, position));
	}

	public void NextStage()
	{
		for (int i = 0; i < coinList.Count; i++)
		{
			if (coinList[i].activeSelf)
			{
				this.PushCoinObject(KDefine.MONEY_COIN, coinList[i]);
			}
		}
	}

	private void PushKeyValue(string objectName)
	{
		keyString.Clear();
		keyString.Append(this.oFilePath);
		keyString.Append(objectName);
		this.merchandiseDict.Add(objectName, new KeyValuePair<string, CObjectPool>(keyString.ToString(), new CObjectPool()));
	}

	private IEnumerator CreateCoin(int nCnt, Vector3 position)
	{
		for (int i = 0; i < nCnt; i++)
		{
			var coin = this.GetCoinObject(KDefine.MONEY_COIN);
			coin.transform.position = position + Vector3.up * 0.3f;
            if(nCnt - i > 99)
            {
                i += 99;
                coin.GetComponent<CCoin>().OnEnter(100);
            }
            else if(nCnt - i > 9)
            {
                i += 9;
                coin.GetComponent<CCoin>().OnEnter(10);
            }
            else
            {
                coin.GetComponent<CCoin>().OnEnter(1);
            }
			coinList.Add(coin);
            yield return new WaitForEndOfFrame();
		}
	}
}
