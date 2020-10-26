using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSingleton<T> : CComponent where T : CComponent
{
	private static T _instance = null;

	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				var oGameObject = new GameObject(typeof(T).ToString());
				_instance = oGameObject.AddComponent<T>();

				DontDestroyOnLoad(oGameObject);
			}

			return _instance;
		}
	}

	public static T Create()
	{
		return CSingleton<T>.Instance;
	}
}
