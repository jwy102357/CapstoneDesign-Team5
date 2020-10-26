using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//이펙트에 붙히는 컴포넌트
public class CEffect : MonoBehaviour
{
	public string oName;
	private ParticleSystem particleSystem;
	private float fEffectEndTime;
	private float fAddTime = 0.05f;
    private CMovement playerMovement;

	void Awake()
	{
        particleSystem = this.GetComponent<ParticleSystem>();
		fEffectEndTime = particleSystem.main.duration + fAddTime;
        playerMovement = FindObjectOfType<CMovement>();
	}

	void OnEnable()
	{
		StartCoroutine(StartEffect());
	}

	void OnDisable()
	{
		StopCoroutine(StartEffect());	
	}

	IEnumerator StartEffect()
	{
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = this.transform.GetChild(i);
            child.localScale = new Vector2(child.localScale.x < 0 ? -child.localScale.x : child.localScale.x, child.localScale.y);
            child.localScale = new Vector2((playerMovement.bIsFacingRight? 1 : -1) * child.localScale.x, child.localScale.y);
        }

        this.gameObject.SetActive(true);
		this.particleSystem.Play();
		yield return new WaitForSeconds(fEffectEndTime);
		CEffectManager.Instance.PushEffect(this.gameObject, oName);
		yield break;
	}
}
