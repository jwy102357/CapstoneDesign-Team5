using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;
//스테이지 클리어 여부에 따라 락 풀기 지금은 그냥 2 3스테이지 잠궈놈 DO SOMETHING
public class CChapterSelectPopup : MonoBehaviour
{
    enum EChapter
    {
        NONE = 0,
        Chapter_1,
        Chapter_2,
        Chapter_3
    }

    public GameObject rightArrow;
    public GameObject leftArrow;
    public List<Sprite> chapterBackGroundImages;
    public List<Sprite> chpaterNameImage;
    public GameObject startButton;
    public GameObject LockObject;

    public Image chapterName;
    public Image currentChapterRenderer;

    EChapter currentChapter = EChapter.NONE;
    string chapterScene;
    public bool selected;

    Dictionary<EChapter, Tuple<string, Sprite>> chapterDict;

    void Awake()
    {
        leftArrow = transform.Find("LeftArrow").GetComponent<Button>().gameObject;
        rightArrow = transform.Find("RightArrow").GetComponent<Button>().gameObject;
        chapterDict = new Dictionary<EChapter, Tuple<string, Sprite>>();

        chapterDict[EChapter.Chapter_1] = Tuple.Create("Chapter01", 
            chapterBackGroundImages[(int)EChapter.Chapter_1 - 1]);

        chapterDict[EChapter.Chapter_2] = Tuple.Create("Chapter02",
            chapterBackGroundImages[(int)EChapter.Chapter_2 - 1]);

        chapterDict[EChapter.Chapter_3] = Tuple.Create("Chapter03",
            chapterBackGroundImages[(int)EChapter.Chapter_3 - 1]);
        
        currentChapter = EChapter.Chapter_1;
        currentChapterRenderer.sprite = chapterBackGroundImages[(int)currentChapter - 1];
        chapterName.sprite = chpaterNameImage[(int)currentChapter - 1];
        chapterScene = "Chapter01";
        startButton.SetActive(true);
        selected = false;
    }

    void Start()
    {
        leftArrow.SetActive(false);
    }

    void OnEnable()
    {
        Function.StopGame();
        this.ResetSetting();
    }

    public void OnClickRightArrow()
    {
        currentChapter++;
        //클리어 여부 확인후 sprite 셀렉트
        if (chapterDict[currentChapter].Item1 == "Chapter02" || chapterDict[currentChapter].Item1 == "Chapter03")
        {
            this.startButton.SetActive(false);
            this.LockObject.SetActive(true);
        }

        else
        {
            this.startButton.SetActive(true);
            this.LockObject.SetActive(false);
        }
        currentChapterRenderer.sprite = chapterDict[currentChapter].Item2;
        chapterName.sprite = chpaterNameImage[(int)currentChapter - 1];
        HideArrow();
    }

    public void OnClickLeftArrow()
    {
        currentChapter--;
        if (chapterDict[currentChapter].Item1 == "Chapter02" || chapterDict[currentChapter].Item1 == "Chapter03")
        {
            this.startButton.SetActive(false);
            this.LockObject.SetActive(true);
        }

        else
        {
            this.startButton.SetActive(true);
            this.LockObject.SetActive(false);
        }
        currentChapterRenderer.sprite = chapterDict[currentChapter].Item2;
        chapterName.sprite = chpaterNameImage[(int)currentChapter - 1];
        HideArrow();
    }

    public void OnClickEntrance()
    {
        if (chapterDict[currentChapter].Item1 == "Chapter01")
        {
            chapterScene = chapterDict[currentChapter].Item1;
            Function.StartGame();
            Initiate.Fade(chapterScene.ToString(), Color.black, 1f);
            //CFadeInOut.FadeOut(1f, chapterScene.ToString());
            selected = true;
            gameObject.SetActive(false);
        }

        CWeaponManager.Instance.SetMaxBullet();
    }

    private void HideArrow()
    {
        leftArrow.SetActive(currentChapter > EChapter.Chapter_1);
        rightArrow.SetActive(currentChapter < EChapter.Chapter_3);
    }

    public void OnClickExit()
    {
        this.gameObject.SetActive(false);
        Function.StartGame();
    }

    public void ResetSetting()
    {
        currentChapter = EChapter.Chapter_1;
        HideArrow();
        currentChapterRenderer.sprite = chapterBackGroundImages[(int)currentChapter - 1];
        chapterScene = "Chapter01";
        chapterName.sprite = chpaterNameImage[(int)currentChapter - 1];
        this.LockObject.SetActive(false);
        this.startButton.SetActive(true);
    }

}
