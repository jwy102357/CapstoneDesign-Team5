using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSound : CComponent
{
	private AudioSource _oAudioSource = null;

	public float Volume
	{
		get
		{
			return _oAudioSource.volume;
		}
		set
		{
			_oAudioSource.volume = value;
		}
	}

	public bool Mute
	{
		get
		{
			return _oAudioSource.mute;
		}
		set
		{
			_oAudioSource.mute = value;
		}
	}

	public bool IsPlaying
	{
		get
		{
			return _oAudioSource.isPlaying;
		}
	}

	public override void Awake()
	{
		base.Awake();

		_oAudioSource = Function.AddComponent<AudioSource>(this.gameObject);
		_oAudioSource.playOnAwake = false;
	}

	public void PlaySound(string oFilepath, bool bIsLoop, bool bIs3DSound)
	{
		_oAudioSource.clip = CResourceManager.Instance.GetAudioClipForKey(oFilepath);
		_oAudioSource.loop = bIsLoop;
		_oAudioSource.spatialBlend = bIs3DSound ? 1.0f : 0.0f;

		_oAudioSource.Play();
	}

	public void PauseSound()
	{
		_oAudioSource.Pause();
	}

	public void StopSound()
	{
		_oAudioSource.Stop();
	}
}
