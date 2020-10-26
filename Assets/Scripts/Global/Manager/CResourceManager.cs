using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CResourceManager : CSingleton<CResourceManager>
{
	private Dictionary<string, Shader> _oShaderList = null;
	private Dictionary<string, Sprite> _oSpirteList = null;
	private Dictionary<string, Texture> _oTextureList = null;
	private Dictionary<string, Material> _oMaterialList = null;
	private Dictionary<string, AudioClip> _oAudioClipList = null;
	private Dictionary<string, GameObject> _oGameObjectList = null;
	private Dictionary<string, RuntimeAnimatorController> _oRuntimeAnimatorControllerList = null;

	public override void Awake()
	{
		base.Awake();

		_oShaderList = new Dictionary<string, Shader>();
		_oSpirteList = new Dictionary<string, Sprite>();
		_oTextureList = new Dictionary<string, Texture>();
		_oMaterialList = new Dictionary<string, Material>();
		_oAudioClipList = new Dictionary<string, AudioClip>();
		_oGameObjectList = new Dictionary<string, GameObject>();
		_oRuntimeAnimatorControllerList = new Dictionary<string, RuntimeAnimatorController>();
	}

	public Shader GetShaderForKey(string oFilepath, bool bIsAutoCreate = true)
	{
		if (bIsAutoCreate && !_oShaderList.ContainsKey(oFilepath))
		{
			Shader oShader = Shader.Find(oFilepath);
			_oShaderList.Add(oFilepath, oShader);
		}

		return _oShaderList[oFilepath];
	}

	public Sprite GetSpriteForKey(string oFilepath, bool bIsAutoCreate = true)
	{
		if (bIsAutoCreate && !_oSpirteList.ContainsKey(oFilepath))
		{
			Sprite oSprite = Resources.Load<Sprite>(oFilepath);
			_oSpirteList.Add(oFilepath, oSprite);
		}

		return _oSpirteList[oFilepath];
	}

	public Texture GetTextureForKey(string oFilepath, bool bIsAutoCreate = true)
	{
		if (bIsAutoCreate && !_oTextureList.ContainsKey(oFilepath))
		{
			Texture oTexture = Resources.Load<Texture>(oFilepath);
			_oTextureList.Add(oFilepath, oTexture);
		}
		return _oTextureList[oFilepath];
	}

	public Material GetMaterialForKey(string oFilepath, bool bIsAutoCreate = true)
	{
		if (bIsAutoCreate && !_oMaterialList.ContainsKey(oFilepath))
		{
			Material oMaterial = Resources.Load<Material>(oFilepath);
			_oMaterialList.Add(oFilepath, oMaterial);
		}

		return _oMaterialList[oFilepath];
	}

	public Material GetCopiedMaterialForKey(string oFilepath, bool bIsAutoCreate = true)
	{
		Material oMaterial = this.GetMaterialForKey(oFilepath, bIsAutoCreate);
		return new Material(oMaterial);
	}

	public AudioClip GetAudioClipForKey(string oFilepath, bool bIsAutoCreate = true)
	{
		if (bIsAutoCreate && !_oAudioClipList.ContainsKey(oFilepath))
		{
			AudioClip oAudioClip = Resources.Load<AudioClip>(oFilepath);
			_oAudioClipList.Add(oFilepath, oAudioClip);
		}

		return _oAudioClipList[oFilepath];
	}

	public GameObject GetObjectForKey(string oFilepath, bool bIsAutoCreate = true)
	{
		if (bIsAutoCreate && !_oGameObjectList.ContainsKey(oFilepath))
		{
			GameObject oGameObject = Resources.Load<GameObject>(oFilepath);
			_oGameObjectList.Add(oFilepath, oGameObject);
		}

		return _oGameObjectList[oFilepath];
	}

	public RuntimeAnimatorController GetAnimatorControllerForKey(string oFilepath, bool bIsAutoCreate = true)
	{
		if (bIsAutoCreate &&
			!_oRuntimeAnimatorControllerList.ContainsKey(oFilepath))
		{
			RuntimeAnimatorController oAnimatorController = Resources.Load<RuntimeAnimatorController>(oFilepath);
			_oRuntimeAnimatorControllerList.Add(oFilepath, oAnimatorController);
		}

		return _oRuntimeAnimatorControllerList[oFilepath];
	}
}
