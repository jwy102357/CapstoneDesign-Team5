using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class CFadeInOut
{
	private static bool bIsFading = false; 
	private static GameObject fadeObj;
	private static Image fadeImage;
	public static void FadeIn(float fFadeTime)
	{
		if (fadeObj == null)
		{
			fadeObj = new GameObject("FadeImage");
			fadeObj.transform.SetParent(GameObject.Find("Canvas_UI").transform);
			fadeImage = fadeObj.AddComponent<Image>();
			fadeObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0f);
		}

		if (fadeImage != null)
		{
			if (!bIsFading)
			{
				fadeImage.color = Color.clear;
				CSceneManager.CurrentSceneManager.StartCoroutine(StartFadeIn(fFadeTime));
			}
			
		}
	}
	
	public static void FadeOut(float fFadeTime)
	{
		if (fadeObj == null)
		{
			fadeObj = new GameObject("FadeImage");
			fadeObj.transform.SetParent(GameObject.Find("Canvas_UI").transform);
			fadeImage = fadeObj.AddComponent<Image>();
            fadeObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0f);
		}

		if (fadeImage != null)
		{
			if (!bIsFading)
			{
				fadeImage.color = new Color(0, 0, 0, 255);
				CSceneManager.CurrentSceneManager.StartCoroutine(StartFadeOut(fFadeTime));
			}
		}
		//서서히 어두워짐
	}

    public static void FadeOut(float fFadeTime, string sSceneName)
    {
        if (fadeObj == null)
        {
            fadeObj = new GameObject("FadeImage");
            fadeObj.transform.SetParent(GameObject.Find("Canvas_UI").transform);
            fadeImage = fadeObj.AddComponent<Image>();
            fadeObj.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0f);
        }

        if (fadeImage != null)
        {
            if (!bIsFading)
            {
                fadeImage.color = new Color(0, 0, 0, 255);
                CSceneManager.CurrentSceneManager.StartCoroutine(StartFadeOut(fFadeTime));
            }
        }

        Function.LateCall((oParam) =>
        {
            SceneManager.LoadScene(sSceneName);
        }, (fFadeTime - 0.1f));
        //서서히 어두워짐
    }

    private static IEnumerator StartFadeIn(float fFadeTime)
	{
		bIsFading = true;
		fadeObj.SetActive(bIsFading);

		Color color = fadeImage.color;
		float fTime = 0f;
		color.a = Mathf.Lerp(1, 0, fTime);
		fadeImage.color = color;
		while (fadeImage.color.a > 0f)
		{
			fTime += Time.deltaTime / fFadeTime;
			color.a = Mathf.Lerp(1, 0, fTime);
			fadeImage.color = color;
			yield return new WaitForEndOfFrame();
		}
		bIsFading = false;
		fadeObj.SetActive(bIsFading);
		yield return null;
	}

	private static IEnumerator StartFadeOut(float fFadeTime)
	{
		bIsFading = true;
		fadeObj.SetActive(bIsFading);

		Color color = fadeImage.color;
		float fTime = 0f;
		color.a = Mathf.Lerp(0, 1, fTime);
		fadeImage.color = color;
		while (fadeImage.color.a < 1f)
		{
			fTime += Time.deltaTime / fFadeTime;
			color.a = Mathf.Lerp(0, 1, fTime);
			fadeImage.color = color;
			yield return new WaitForEndOfFrame();
		}

		bIsFading = false;
		fadeObj.SetActive(bIsFading);
	}


    public static void Fade(float outTime, float inTime)
    {
        FadeOut(outTime);
        Function.LateCall((oParam) =>
        {
            FadeIn(inTime);
        }, outTime);
    }

}
