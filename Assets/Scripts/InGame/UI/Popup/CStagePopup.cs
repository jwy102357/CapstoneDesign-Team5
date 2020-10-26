using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CStagePopup : MonoBehaviour
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
    public Text chapterName;
    public float fCheckTime;
    public Image currentChapterRenderer;

    EChapter currentChapter = EChapter.NONE;
    string chapterScene;
    WaitForSeconds checkTime;
    

    Dictionary<EChapter, Tuple<string, Sprite>> chapterDict;
    //Dict 설정하고 키로 챕터 밸류로 문자열과 이미지
    void Awake()
    {
        leftArrow.SetActive(false);
        checkTime = new WaitForSeconds(fCheckTime);

        currentChapter = EChapter.Chapter_1;
        currentChapterRenderer.sprite = chapterBackGroundImages[(int)currentChapter - 1];
        chapterScene = "Chapter01";

        chapterDict = new Dictionary<EChapter, Tuple<string, Sprite>>();

        chapterDict[EChapter.Chapter_1] = Tuple.Create("Chapter01", chapterBackGroundImages[(int)EChapter.Chapter_1 - 1]);
        chapterDict[EChapter.Chapter_2] = Tuple.Create("Chapter02", chapterBackGroundImages[(int)EChapter.Chapter_2 - 1]);
        chapterDict[EChapter.Chapter_3] = Tuple.Create("Chapter03", chapterBackGroundImages[(int)EChapter.Chapter_3 - 1]);

        StartCoroutine(SetPopup());
    }

    void OnEnable()
    {
        this.ResetSetting();
        StartCoroutine(SetPopup());
    }

    public void OnClickRightArrow()
    {
        currentChapter++;
        currentChapterRenderer.sprite = chapterDict[currentChapter].Item2;
        chapterName.text = chapterDict[currentChapter].Item1;
    }

    public void OnClickLeftArrow()
    {
        currentChapter--;
        currentChapterRenderer.sprite = chapterDict[currentChapter].Item2;
        chapterName.text = chapterDict[currentChapter].Item1;
    }

    public void OnClickEntrance()
    {
        chapterScene = chapterDict[currentChapter].Item1;
        Initiate.Fade(chapterScene.ToString(), Color.black, 0.3f);
        StopCoroutine(SetPopup());
    }

    public void OnClickExit()
    {
        this.gameObject.SetActive(false);
        StopCoroutine(SetPopup());
        Function.StartGame();
    }

    public IEnumerator SetPopup()
    {
        while(true)
        {
            this.leftArrow.SetActive(currentChapter > EChapter.Chapter_1);
            this.rightArrow.SetActive(currentChapter < EChapter.Chapter_3);
            yield return checkTime;
        }
    }

    public void ResetSetting()
    {
        leftArrow.SetActive(false);
        currentChapter = EChapter.Chapter_1;
        currentChapterRenderer.sprite = chapterBackGroundImages[(int)currentChapter - 1];
        chapterScene = "Chapter01";
        chapterName.text = chapterDict[currentChapter].Item1;
    }
}
