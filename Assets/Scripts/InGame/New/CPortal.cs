using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPortal : MonoBehaviour
{
    public bool bIsFinal = false;
	private ChapterScene chapterManager;
    private CMovement player;
    private CRewardSelectPopup rewardSelectPopup;
    int currentStageIndex;
    int shopStageIndex;

    private void Awake()
    {
        rewardSelectPopup = GameObject.Find("Canvas_UI").transform.Find("RewardSelectPopup").GetComponent<CRewardSelectPopup>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(KDefine.TAG_PLAYER))
		{
			Debug.Log("포탈");
            player = collision.GetComponent<CMovement>();
            player.transform.position = transform.position;
            player.EnterPortal();

            if (this.chapterManager != null && !bIsFinal)
            {
                Invoke("MoveStage", 2f);
                CFadeInOut.Fade(2f, 2f);
            }
            else if (this.chapterManager != null && bIsFinal)
            {
                Invoke("MoveChapter", 2f);
                CFadeInOut.Fade(2f, 2f);
            }
        }
	}

    void MoveStage()
    {
        currentStageIndex = ES3.Load<int>("CurrentIndex", "Stage.es3");
        shopStageIndex = ES3.Load<int>("ShopStageIndex", "Stage.es3");
        if(currentStageIndex-1 != shopStageIndex)
        {
            rewardSelectPopup.gameObject.SetActive(true);
        }
        chapterManager.MoveStage();
    }

    void MoveChapter()
    {
        switch (this.chapterManager.chapterNumber)
        {
            case 1:
                //Initiate.Fade("Chapter2Scene", Color.black ,2f);
                Initiate.Fade("LobbyScene", Color.black, 2f);
                break;
            case 2:
                Initiate.Fade("Chapter3Scene", Color.black, 2f);
                break;
            case 3:
                Initiate.Fade("LobbyScene", Color.black, 2f);
                break;
            default:
                Initiate.Fade("LobbyScene", Color.black, 2f);
                break;
        }
        bIsFinal = false;
    }

    public void SetChapterScene(ChapterScene chapterScene)
	{
		this.chapterManager = chapterScene;
	}
}
