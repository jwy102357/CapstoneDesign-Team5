using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEffectAnimation : MonoBehaviour
{
	public Animator animator;
	public bool bIsEffect;
	public string objectName;

	void Awake()
	{
		if (animator == null) animator = this.GetComponent<Animator>();
	}

	void AnimationEnd()
	{
		if (bIsEffect)
		{
			CEffectManager.Instance.PushEffect(this.gameObject, objectName);
		}

		else
		{
			CRangedObjectManager.Instance.PushRangedObject(objectName, this.gameObject, this.transform.localScale);
		}
	}
}
