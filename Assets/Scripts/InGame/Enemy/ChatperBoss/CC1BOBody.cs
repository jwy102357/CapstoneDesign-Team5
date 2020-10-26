using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC1BOBody : MonoBehaviour
{
	public Animator faceAnimator;
	public BoxCollider2D boxCollider;
	public Transform effectPivot;
	public void Awake()
	{
		if (boxCollider == null)
		{
			boxCollider = this.GetComponent<BoxCollider2D>();
		}
	}

	public void Attack()
	{
		faceAnimator.SetBool("Attack", true);
	}

	public void AttackEnd()
	{
		faceAnimator.SetBool("Attack", false);
	}

	public void EightDirAttack(Vector3 point, float speed)
	{
		CEnemyPatternManager.Instance.EightDirAttack(point, speed);
	}

	public void SpiralBullet(Vector3 point)
	{
		CEnemyPatternManager.Instance.SpiralBullet(point);
	}

	public void Laser() 
	{
		this.Attack();	
	}

}
