using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Function
{
	public static void ShowLog(string oFormat, params object[] oParams)
	{
		string oLog = string.Format(oFormat, oParams);
		Debug.Log(oLog);
	}

	public static T FindComponent<T>(string oName) where T : Component
	{
		var oGameObject = GameObject.Find(oName);

		if (oGameObject == null)
		{
			return null;
		}

		return oGameObject.GetComponentInChildren<T>();
	}

	public static T AddComponent<T>(GameObject oGameObject) where T : Component
	{
		var oComponent = oGameObject.GetComponent<T>();

		if (oComponent == null)
		{
			oComponent = oGameObject.AddComponent<T>();
		}

		return oComponent;
	}

	public static void LateCall(System.Action<object[]> oCallback, float fDelay, params object[] oParams)
	{
		var oSceneManager = CSceneManager.CurrentSceneManager;
		oSceneManager.StartCoroutine(Function.DoLateCall(oCallback, fDelay, oParams));
	}

	public static IEnumerator WaitAsyncOperation(AsyncOperation oAsyncOperation, System.Action<AsyncOperation, bool> oCallback) //몰라도 됨
	{
		oAsyncOperation.completed += oSender =>
		{
			if (oCallback != null)
			{
				oCallback(oSender, true);
			}
		};

		while (!oAsyncOperation.isDone)
		{
			yield return new WaitForEndOfFrame();

			if (oCallback != null)
			{
				oCallback(oAsyncOperation, false);
			}
		}
	}

	public static GameObject CreateGameObject(string oName, GameObject oParent, bool bIsWorldStay = false, bool bIsResetTransform = false)
	{
		var oGameObject = new GameObject(oName);

		if (oParent != null)
		{
			oGameObject.transform.SetParent(oParent.transform, bIsWorldStay);
		}

		if (bIsResetTransform)
		{
			oGameObject.transform.localPosition = Vector3.zero;
			oGameObject.transform.localEulerAngles = Vector3.zero;
			oGameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}

		return oGameObject;
	}

	public static GameObject CreateCopiedGameObject(string oName, GameObject oOrigin, GameObject oParent, bool bIsWorldStay = false, bool bIsResetTransform = false)
	{
		var oGameObject = Object.Instantiate(oOrigin, Vector3.zero, Quaternion.identity);

		if (oParent != null)
		{
			oGameObject.transform.SetParent(oParent.transform, bIsWorldStay);

			if (!bIsResetTransform)
			{
				oGameObject.transform.localPosition = oOrigin.transform.localPosition;
				oGameObject.transform.localEulerAngles = oOrigin.transform.localEulerAngles;
				oGameObject.transform.localScale = oOrigin.transform.localScale;
			}
		}

		if (bIsResetTransform)
		{
			oGameObject.transform.localPosition = Vector3.zero;
			oGameObject.transform.localEulerAngles = Vector3.zero;
			oGameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}

		oGameObject.name = oName;
		return oGameObject;
	}

	public static GameObject CreateCopiedGameObject(string oName, string oFilepath, GameObject oParent, bool bIsWorldStay = false, bool bIsResetTransform = false)
	{
		var oOriginObject = CResourceManager.Instance.GetObjectForKey(oFilepath);
		return Function.CreateCopiedGameObject(oName, oOriginObject, oParent, bIsWorldStay, bIsResetTransform);
	}

	public static T CreateGameObject<T>(string oName, GameObject oParent, bool bIsWorldStay = false, bool bIsResetTransform = false) where T : Component
	{
		var oGameObject = Function.CreateGameObject(oName, oParent, bIsWorldStay, bIsResetTransform);
		return Function.AddComponent<T>(oGameObject);
	}

	public static T CreateCopiedGameObject<T>(string oName, GameObject oOrigin, GameObject oParent, bool bIsWorldStay = false, bool bIsResetTransform = false) where T : Component
	{
		var oGameObject = Function.CreateCopiedGameObject(oName, oOrigin, oParent, bIsWorldStay, bIsResetTransform);
		return oGameObject.GetComponentInChildren<T>();
	}

	public static T CreateCopiedGameObject<T>(string oName, string oFilepath, GameObject oParent, bool bIsWorldStay = false, bool bIsResetTransform = false) where T : Component
	{
		var oGameObject = Function.CreateCopiedGameObject(oName, oFilepath, oParent, bIsWorldStay, bIsResetTransform);
		return oGameObject.GetComponentInChildren<T>();
	}
	
	public static void ShuffleArray<T>(List<T> shuffleList)
	{
		for (int i = 0; i < shuffleList.Count; i++)
		{
			int random1 = Random.Range(0, shuffleList.Count);
			int random2 = Random.Range(0, shuffleList.Count);

			T temp = shuffleList[random1];
			shuffleList[random1] = shuffleList[random2];
			shuffleList[random2] = temp;
		}
	}


	public static void StopGame()
	{
		Time.timeScale = 0;
	}

	public static void StartGame()
	{
		Time.timeScale = 1.0f;
	}

	private static IEnumerator DoLateCall(System.Action<object[]> oCallback, float fDelay, params object[] oParams)
	{
		yield return new WaitForSeconds(fDelay);
		oCallback(oParams);
	}
}
