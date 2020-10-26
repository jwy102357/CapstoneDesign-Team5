using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CLobbyScene : CSceneManager
{
    CMovement playerMovement;
    CinemachineVirtualCamera cineCamera;
    private float endOrthographicSize = 5f;
    private List<int> indexList = new List<int>();

    public GameObject canvas;
    public GameObject skipButton;

    public float startDelay = 1f;
    public float startOrthographicSize = 2f;
    public float delayTime = 0.1f;
    public float cuts = 10f;

    public override void Awake()
    {
        base.Awake();
        CWeaponManager.Instance.ReturnLobby();
        playerMovement = GameObject.Find("Player").GetComponent<CMovement>();
        cineCamera = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        canvas = GameObject.Find("Canvas_UI");
        skipButton = canvas.transform.Find("SkipButton").gameObject;
        ES3.Save<int[]>("StageArray", null, "Stage.es3");
        ES3.Save<int>("CurrentIndex", -1, "Stage.es3");
        ES3.Save<bool>("IsContinue", false, "Stage.es3");
    }

    private void Start()
    {
        if (!ES3.KeyExists("CurrentHealth", "Player.es3") || ES3.Load<float>("CurrentHealth", "Player.es3") == 0)
        {
            StartCoroutine(SceneOpening());
        }
        InitPlayerData();
    }

    private IEnumerator SceneOpening()
    {
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            GameObject child;
            child = canvas.transform.GetChild(i).gameObject;
            if (child.activeSelf)
            {
                indexList.Add(i);
                child.SetActive(false);
            }
        }
        CFadeInOut.FadeIn(3f);
        skipButton.SetActive(true);

        float distance = endOrthographicSize - startOrthographicSize;
        playerMovement.WakeUp();
        cineCamera.m_Lens.OrthographicSize = startOrthographicSize;

        yield return new WaitForSeconds(startDelay);

        while(cineCamera.m_Lens.OrthographicSize < endOrthographicSize - (distance / cuts))
        {
            cineCamera.m_Lens.OrthographicSize += distance/cuts;
            yield return new WaitForSeconds(delayTime);
        }
        CloseOpening();
        skipButton.SetActive(false);
    }

    public void Skip()
    {
        CloseOpening();
        skipButton.SetActive(false);
        playerMovement.WakeUpEnd();
        StopCoroutine(SceneOpening());
    }

    private void CloseOpening()
    {
        cineCamera.m_Lens.OrthographicSize = endOrthographicSize;
        for (int i = 0; i < indexList.Count; i++)
        {
            canvas.transform.GetChild(indexList[i]).gameObject.SetActive(true);
        }
    }

    private void InitPlayerData()
    {
        ES3.Save<float>("CurrentHealth", 6, "Player.es3");
        ES3.Save<float>("MaxHealth", 6, "Player.es3");
        ES3.Save<int>("Shield", 0, "Player.es3");
        playerMovement.GetComponent<CPlayerHealth>().SynchronizeUI();
    }
}
