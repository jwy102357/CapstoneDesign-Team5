using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CSoundManager : CSingleton<CSoundManager>
{
	private bool _bEffectMute = false;
	private bool _bBackgroundMute = false;

	private float _fEffectVolume = 1.0f;
	private float _fBackgroundVolume = 1.0f;

	private CSound _oBackgroundSound = null;
	private Dictionary<string, List<CSound>> _oEffectSoundList = null;

	public float EffectVolume
	{
		get
		{
			return _fEffectVolume;
		}
		set
		{
			_fEffectVolume = value;
			this.EnumerateEffectSoundList((oSound) =>
			{
				oSound.Volume = value;
			});
		}
	}

	public float BackgroundVolume
	{
		get
		{
			return _oBackgroundSound.Volume;
		}
		set
		{
			_fBackgroundVolume = value;
			_oBackgroundSound.Volume = value;
		}
	}
	
	public bool EffectMute
	{
		get
		{
			return _bEffectMute;
		}
		set
		{
			_bEffectMute = value;

			this.EnumerateEffectSoundList((oSound) =>
			{
				oSound.Mute = value;
			});
		}
	}

	public bool BackgroundMute
	{
		get
		{
			return _bBackgroundMute;
		}
		set
		{
			_bBackgroundMute = value;
			_oBackgroundSound.Mute = value;
		}
	}

	public override void Awake()
	{
		base.Awake();
		_oEffectSoundList = new Dictionary<string, List<CSound>>();
		_oBackgroundSound = Function.CreateGameObject<CSound>("BackgroundSound", this.gameObject);
	}

	public void PlayOneShotSound(string oFilepath, Vector3 stPosition)
	{
		AudioClip oAuidoClip = CResourceManager.Instance.GetAudioClipForKey(oFilepath);
		AudioSource.PlayClipAtPoint(oAuidoClip, stPosition);
	}

	public void PlayEffectSound(string oFilepath, bool bIsLoop = false, bool bIs3DSound = false)
	{
		CSound oSound = this.FindPlayableEffectSound(oFilepath);

		if (oSound != null)
		{
			this.EffectVolume = _fEffectVolume;
			oSound.PlaySound(oFilepath, bIsLoop, bIs3DSound);
		}
	}

	public void PlayBackgroundSound(string oFilepath, bool bIsLoop = true)
	{
		this.BackgroundVolume = _fBackgroundVolume;
		_oBackgroundSound.PlaySound(oFilepath, bIsLoop, false);
	}

	public void PauseEffectSound()
	{
		this.EnumerateEffectSoundList((oSound) =>
		{
			oSound.PauseSound();
		});
	}

	public void PauseBackgroundSound()
	{
		_oBackgroundSound.PauseSound();
	}

	public void StopEffectSound()
	{
		this.EnumerateEffectSoundList((oSound) =>
		{
			oSound.StopSound();
		});
	}

	public void StopBackgroundSound()
	{
		_oBackgroundSound.StopSound();
	}



	private void EnumerateEffectSoundList(System.Action<CSound> oCallback)
	{
		var oKeyList = _oEffectSoundList.Keys.ToList();

		for (int i = 0; i < oKeyList.Count; ++i)
		{
			string oKey = oKeyList[i];
			var oSoundList = _oEffectSoundList[oKey];

			for (int j = 0; j < oSoundList.Count; ++j)
			{
				CSound oSound = oSoundList[j];
				oCallback(oSound);
			}
		}
	}

	private CSound FindPlayableEffectSound(string oFilepath)
	{
		if (!_oEffectSoundList.ContainsKey(oFilepath))
		{
			var oTempSoundList = new List<CSound>();
			_oEffectSoundList.Add(oFilepath, oTempSoundList);
		}

		var oSoundList = _oEffectSoundList[oFilepath];

		if (oSoundList.Count < KDefine.MAX_NUM_DUPLICATE_EFFECT_SOUND)
		{
			var oSound = Function.CreateGameObject<CSound>("EffectSound", this.gameObject);
			oSoundList.Add(oSound);

			return oSound;
		}

		else
		{
			for (int i = 0; i < oSoundList.Count; ++i)
			{
				var oSound = oSoundList[i];

				if (!oSound.IsPlaying)
				{
					return oSound;
				}
			}
		}

		return null;
	}
}
