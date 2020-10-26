using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPopupManager : CSingleton<CPopupManager>
{
	public Dictionary<string, CBasePopup> popupDictionary;

	public override void Awake()
	{
		popupDictionary = new Dictionary<string, CBasePopup>();
		var canvasObject = new GameObject("PopupCanvas");

		this.CanvasSetting(canvasObject);
        /*
		popupDictionary.Add("Resurrection", 
			Function.CreateCopiedGameObject("ResurrectionPopup" , 
			CResourceManager.Instance.GetObjectForKey("Prefabs/ResurrectionPopup"), 
			canvasObject).GetComponent<CBasePopup>());
            */

		popupDictionary.Add("Code",
			Function.CreateCopiedGameObject("CodePopup",
			CResourceManager.Instance.GetObjectForKey("Prefabs/CodePopup"),
			canvasObject).GetComponent<CBasePopup>());

		popupDictionary.Add("Setting",
			Function.CreateCopiedGameObject("SettingPopup",
			CResourceManager.Instance.GetObjectForKey("Prefabs/SettingPopup"),
			canvasObject.gameObject).GetComponent<CBasePopup>());

		popupDictionary.Add("Developer",
			Function.CreateCopiedGameObject("DeveloperScrollView",
			CResourceManager.Instance.GetObjectForKey("Prefabs/DeveloperScrollView"),
			canvasObject.gameObject).GetComponent<CBasePopup>());

		popupDictionary.Add("Pause",
			Function.CreateCopiedGameObject("PausePopup",
			CResourceManager.Instance.GetObjectForKey("Prefabs/UI/PausePopup"),
			canvasObject.gameObject).GetComponent<CBasePopup>());

		popupDictionary.Add("LobbyReturn",
			Function.CreateCopiedGameObject("LobbyReturnPopup",
			CResourceManager.Instance.GetObjectForKey("Prefabs/UI/LobbyReturnPopup"),
			canvasObject).GetComponent<CBasePopup>());
	}

	public void ShowPopup(string popupName)
	{
		popupDictionary[popupName].AppearPopup();
	}

	public void DisAppearPopup(string popupName)
	{
		popupDictionary[popupName].DisAppearPopup();
	}

	public GameObject GetObjectForKey(string key)
	{
		if (popupDictionary.ContainsKey(key))
		{
			return popupDictionary[key].gameObject;
		}

		return null;
	}

	private void CanvasSetting(GameObject canvasObject)
	{
		canvasObject.transform.parent = this.transform;
		var canvas = canvasObject.AddComponent<Canvas>();
		var canvasScaler = canvasObject.AddComponent<CanvasScaler>();
		var graphicRaycaster = canvasObject.AddComponent<GraphicRaycaster>();

		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		
		canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
		canvasScaler.scaleFactor = 1;
		canvasScaler.referencePixelsPerUnit = 100;

		graphicRaycaster.ignoreReversedGraphics = true;
		graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
	}
}
