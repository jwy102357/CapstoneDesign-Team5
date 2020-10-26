using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//중간보스마다 스테이지가 다르므로 중간보스 스테이지를 따로 만든 후에 넣어줘야 된다.
//마지막 스테이지 보스 ai예비로 넣어야될듯
//중간보스 미사일 난사하는 파라셸 미사일 난사패턴 안됨 확인바람.
public class ChapterScene : CSceneManager
{
	public List<CStage> stageList;
	public List<CStage> middleBossStageList;
	public CStage shopStage;

	public CPortal portal;
	public int nMaxStage = 0;
	public int chapterNumber = 1;
	public int nShopStageIndex;
	public int nMiddlebossStageIndex;
	private int middlebossIndex;
	private int[] pStageNumberArray;

	private CStage currentStage;
	private int nCurrentIndex = 0;
	private CMovement player;
	private ES3Settings settings;

	public override void Awake()
	{
		base.Awake();
		settings = new ES3Settings(ES3.EncryptionType.AES);
		
		if (ES3.KeyExists("IsContinue", "Stage.es3"))
		{
			if (ES3.Load<bool>("IsContinue", "Stage.es3"))
			{
				this.StageLoadData();
			}

			else
			{
				this.NotSaveChpaterEnter();
			}
		}

		else
		{
			this.NotSaveChpaterEnter();	
		}

		player = FindObjectOfType<CMovement>();
		portal = this.transform.Find("Portal").GetComponent<CPortal>();
		portal.SetChapterScene(this);
		this.MoveStage();
	}

	public override void Update()
	{
		base.Update();
		if (CStageManager.Instance.CurrentStage != null && CStageManager.Instance.CurrentStage.bIsStageEnd)
		{
			if (!portal.gameObject.activeSelf)
			{
				portal.transform.position = CStageManager.Instance.CurrentStage.nextDungeonPos;
				portal.gameObject.SetActive(true);
            }
        }
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
	}

	public void MoveStage()
	{
		portal.gameObject.SetActive(false);
		CMerchandiseManager.Instance.NextStage();
		if (nCurrentIndex == 0)
		{
			player.transform.position = CStageManager.Instance.CurrentStage.StartPos;
			nCurrentIndex++;
		}

		else
        {
			if (nCurrentIndex == nMaxStage)
			{
				//마지막 스테이지일때 챕터 이동 or 대기실 이동
				//대기실 이동이라면 IsContinue False;
				//클리어한 챕터 데이터 클리어.
				Debug.Log("챕터 끝");
			}

			else
			{

				CStageManager.Instance.PushStageObject(CStageManager.Instance.CurrentStage.name, CStageManager.Instance.CurrentStage.gameObject);
				CStageManager.Instance.ReUseStageSetting();
				currentStage = stageList[pStageNumberArray[nCurrentIndex]];

				var stageObj = CStageManager.Instance.GetStageObject(currentStage.name).GetComponent<CStage>();
				CStageManager.Instance.CurrentStage = stageObj;
				CStageManager.Instance.CurrentStage.name = currentStage.name;
				player.transform.position = CStageManager.Instance.CurrentStage.StartPos;

				nCurrentIndex++;

				if (nCurrentIndex == nMaxStage)
				{
					portal.bIsFinal = true;
				}

				CItemManager.Instance.MoveStageSetting();
			}
			this.StageSaveData();
			
		}
		Debug.Log(nCurrentIndex);
		CWeaponManager.Instance.SaveWeapon();
		ES3.Save<int>("CurrentIndex", nCurrentIndex, "Stage.es3");
	}

	public void CreateStageArray()
	{
		this.middlebossIndex = Random.Range(0, middleBossStageList.Count);
		stageList.Insert(this.nMiddlebossStageIndex, middleBossStageList[this.middlebossIndex]);
		stageList.Insert(this.nShopStageIndex, shopStage);

		for (int i = 0; i < stageList.Count; i++)
		{
			if (stageList[i] == this.middleBossStageList[this.middlebossIndex])
			{
				var temp = stageList[i];
				stageList[i] = stageList[this.nMiddlebossStageIndex];
				stageList[nMiddlebossStageIndex] = temp;
				break;
			}

			if (stageList[i] == this.shopStage)
			{
				var temp = stageList[i];
				stageList[i] = stageList[this.nShopStageIndex];
				stageList[this.nShopStageIndex] = temp;
			}
		}


		pStageNumberArray = new int[stageList.Count];
		
		for (int i = 0; i < stageList.Count; i++) //스테이지 리스트 카운트 만큼 돌고. 
		{
			pStageNumberArray[i] = i;
		}

		this.ShuffleArray();
		ES3.Save<int[]>("StageArray", pStageNumberArray, "Stage.es3");
		//stageList[this.pStageNumberArray[this.nMiddlebossStageIndex]] = middleBossStageList[this.middlebossIndex];
		//stageList[this.pStageNumberArray[this.nShopStageIndex]] = shopStage;

	}

	public void ShuffleArray()
	{
		for (int i = 0; i < pStageNumberArray.Length; i++)
		{
			int random1 = Random.Range(0, pStageNumberArray.Length);
			int random2 = Random.Range(0, pStageNumberArray.Length); 

			int temp = pStageNumberArray[random1];
			pStageNumberArray[random1] = pStageNumberArray[random2];
			pStageNumberArray[random2] = temp;
		}
		
		for (int i = 0; i < pStageNumberArray.Length; i++)
		{
			if (pStageNumberArray[i] == stageList.Count - 1)
			{
				int temp = pStageNumberArray[i];
				pStageNumberArray[i] = pStageNumberArray[nMaxStage- 1];
				pStageNumberArray[nMaxStage - 1] = temp;
				break;
			}
		}

		for (int i = 0; i < pStageNumberArray.Length; i++)
		{
			if (pStageNumberArray[i] == nMiddlebossStageIndex) // 지정 인덱스
			{
				int temp = pStageNumberArray[i];
				pStageNumberArray[i] = pStageNumberArray[nMiddlebossStageIndex];
				pStageNumberArray[nMiddlebossStageIndex] = temp;
				break;
			}
		}

		//상점 인덱스 넣기
		for (int i = 0; i < pStageNumberArray.Length; i++)
		{
			if (pStageNumberArray[i] == nShopStageIndex) // 지정 인덱스
			{
				int temp = pStageNumberArray[i];
				pStageNumberArray[i] = pStageNumberArray[nShopStageIndex];
				pStageNumberArray[nShopStageIndex] = temp;
				break;
			}
		}

		

	}

	public void NotSaveChpaterEnter()
	{
		ES3.Save<bool>("IsContinue", true, "Stage.es3");
		
		this.CreateStageArray();
		currentStage = stageList[pStageNumberArray[nCurrentIndex]];
		this.StageSaveData();

		var stageObj = CStageManager.Instance.GetStageObject(currentStage.name).GetComponent<CStage>();
		CStageManager.Instance.CurrentStage = stageObj;
		CStageManager.Instance.CurrentStage.name = currentStage.name;
	}

	public void StageSaveData()
	{
		ES3.Save<int[]>("StageArray", pStageNumberArray, "Stage.es3");
		ES3.Save<int>("MiddleBossIndex", middlebossIndex, "Stage.es3");
		ES3.Save<int>("CurrentIndex", nCurrentIndex, "Stage.es3");
		ES3.Save<int>("MiddleBossStageIndex", this.nMiddlebossStageIndex, "Stage.es3");
		ES3.Save<int>("ShopStageIndex", this.nShopStageIndex, "Stage.es3");
		ES3.Save<bool>("IsContinue", true, "Stage.es3");
	}

	public void StageLoadData()
	{
		pStageNumberArray = ES3.Load<int[]>("StageArray", "Stage.es3");
		nCurrentIndex = ES3.Load<int>("CurrentIndex", "Stage.es3");
		middlebossIndex = ES3.Load<int>("MiddleBossIndex", "Stage.es3");
		this.nMiddlebossStageIndex = ES3.Load<int>("MiddleBossStageIndex", "Stage.es3");
		this.nShopStageIndex = ES3.Load<int>("ShopStageIndex", "Stage.es3");

		if (this.nMiddlebossStageIndex == -1) this.nMiddlebossStageIndex = this.pStageNumberArray.Length / 2;
		if (this.nShopStageIndex == -1) this.nShopStageIndex = this.nMiddlebossStageIndex - 1;
		Debug.Log(nCurrentIndex);
		nCurrentIndex--;
		
		if (nCurrentIndex < 0)
		{
			nCurrentIndex = 0;
		}

		stageList.Insert(this.nMiddlebossStageIndex, middleBossStageList[this.middlebossIndex]);
		stageList.Insert(this.nShopStageIndex, shopStage);

		for (int i = 0; i < stageList.Count; i++)
		{
			if (stageList[i] == this.middleBossStageList[this.middlebossIndex])
			{
				var temp = stageList[i];
				stageList[i] = stageList[this.nMiddlebossStageIndex];
				stageList[nMiddlebossStageIndex] = temp;
				break;
			}

			if (stageList[i] == this.shopStage)
			{
				var temp = stageList[i];
				stageList[i] = stageList[this.nShopStageIndex];
				stageList[this.nShopStageIndex] = temp;
			}
		}


		currentStage = stageList[pStageNumberArray[nCurrentIndex]];

		var stageObj = CStageManager.Instance.GetStageObject(currentStage.name).GetComponent<CStage>();
		CStageManager.Instance.CurrentStage = stageObj;
		CStageManager.Instance.CurrentStage.name = currentStage.name;
	}

	public void StageDataClear()
	{
		ES3.Save<int[]>("StageArray", null, "Stage.es3");
		ES3.Save<int>("CurrentIndex", 0, "Stage.es3");
		ES3.Save<bool>("IsContinue", false, "Stage.es3");
		ES3.Save<int>("MiddeBossStageIndex", -1, "Stage.es3");
		ES3.Save<int>("ShopStageIndex", -1, "Stage.es3");
		ES3.Save<int>("MiddleBossIndex", -1, "Stage.es3");
	}

}
