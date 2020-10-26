using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSceneManager : CComponent
{
	// Start is called before the first frame update
	public static Camera UICamera
	{
		get
		{
			return Function.FindComponent<Camera>(KDefine.NAME_UI_CAMERA);
		}
	}

	public static Camera MainCamera
	{
		get
		{
			return Function.FindComponent<Camera>(KDefine.NAME_MAIN_CAMERA);
		}
	}

	public static CSceneManager CurrentSceneManager
	{
		get
		{
			return Function.FindComponent<CSceneManager>(KDefine.NAME_SCENE_MANAGER);
		}
	}

	public static GameObject UIRoot
	{
		get
		{
			return GameObject.Find(KDefine.NAME_UI_ROOT);
		}
	}

	public static GameObject ObjectRoot
	{
		get
		{
			return GameObject.Find(KDefine.NAME_OBJECT_ROOT);
		}
	}

	public bool bIsPause = false;

	public override void Awake()
	{
		base.Awake();

		//this.SetupUICamera();
		//this.SetupMainCamera();
		CResourceManager.Create();
		CSoundManager.Create();
		CPopupManager.Create();
		CEffectManager.Create();
		CWeaponManager.Create();
		CItemManager.Create();
		CEnemyManager.Create();
		CStageManager.Create();
		CMerchandiseManager.Create();
		//에너미패턴 매니저의 경우 로비씬에서 크리에이트 해야 된다. 지금은 테스트용이라 여기에 해놈
		CEnemyPatternManager.Create();
		//CLocalizeManager.Instance.ResetDict();
		//CLocalizeManager.Instance.LoadStringListFromFile();

		Application.targetFrameRate = 60;

		Screen.SetResolution(KDefine.SCREEN_WIDTH, KDefine.SCREEN_HEIGHT, true);
	}

	public virtual void Update()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				//POPUP CREATE
				Function.StopGame();
			}

			
		}
	}

	public virtual void LateUpdate()
	{
		if ((Input.deviceOrientation == DeviceOrientation.LandscapeLeft) && (Screen.orientation != ScreenOrientation.LandscapeLeft))
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}

		else if ((Input.deviceOrientation == DeviceOrientation.LandscapeRight) && (Screen.orientation != ScreenOrientation.LandscapeRight))
		{
			Screen.orientation = ScreenOrientation.LandscapeRight;
		}
	}

	protected virtual void SetupUICamera()
	{
		if (CSceneManager.UICamera != null)
		{
			var oUICamera = CSceneManager.UICamera;
			oUICamera.orthographic = true;
			oUICamera.orthographicSize = (KDefine.SCREEN_HEIGHT / 2.0f) * 0.01f;
		}
	}

	protected virtual void SetupMainCamera()
	{
		if (CSceneManager.MainCamera != null)
		{
			var oMainCamera = CSceneManager.MainCamera;
			oMainCamera.orthographic = false;
			oMainCamera.fieldOfView = 0.0f;

			float fDepth = Mathf.Abs(oMainCamera.transform.position.z);
			float fFieldOfView = Mathf.Atan((KDefine.SCREEN_HEIGHT / 2.0f) / fDepth);

			oMainCamera.fieldOfView = (fFieldOfView * 2.0f) * Mathf.Rad2Deg;
		}
	}

	public void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			bIsPause = true;
			Function.StopGame();
		}

		else
		{
			if (bIsPause)
			{
				bIsPause = false;
				Function.StartGame();
			}
		}
	}

	public void OnApplicationQuit()
	{
		
	}
}
