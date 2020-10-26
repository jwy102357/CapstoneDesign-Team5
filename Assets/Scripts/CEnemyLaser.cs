using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyLaser : MonoBehaviour
{
	public Animator animator;
	public int patternIndex;
	private GameObject enemy;

	void Awake()
	{
		if (animator == null) animator = this.GetComponent<Animator>();
		if (enemy == null) enemy = GameObject.Find("FinalBoss");
	}

	void OnEnable()
	{
		if (enemy == null) enemy = GameObject.Find("FinalBoss");
	}

	void OnDisable()
	{
		//레이저 초기화
		this.animator.SetBool("AttackEnd", false);
		patternIndex = -1;
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(KDefine.TAG_PLAYER))
		{
			IDamageable target = collision.GetComponent<IDamageable>();
			if (target != null)
			{
				target.OnDamage(1f, Vector3.zero);
			}
		}
	}

	void LaserEnd()
	{
		this.enemy.SendMessage("SendEnd", patternIndex, SendMessageOptions.DontRequireReceiver);
		CRangedObjectManager.Instance.PushRangedObject("CenterLaser", this.gameObject);
	}
}
