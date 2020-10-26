using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CStone : CEnemyBullet
{
	public Animator animator;
	public float fSpeed = 2f;
	private Quaternion originRotation;

	public override void Awake()
	{
		base.Awake();
		if (animator == null) animator = this.GetComponent<Animator>();
		this.originRotation = this.transform.rotation;
	}

	private void OnEnable()
	{
		if (animator == null) animator = this.GetComponent<Animator>();
	}

	private void OnDisable()
	{
		this.transform.rotation = originRotation;
	}

	public void Shot()
	{
		this.transform.rotation = Quaternion.AngleAxis(-90f, Vector3.forward);
		this.rigidbody.velocity = this.transform.right * this.fSpeed;
	}
}
