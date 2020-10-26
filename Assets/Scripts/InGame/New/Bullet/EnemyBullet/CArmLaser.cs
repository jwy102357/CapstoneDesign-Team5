using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CArmLaser : MonoBehaviour
{
	public bool bIsCollision = false;
	private IDamageable player;
	void Update()
	{
		if (bIsCollision)
		{
			if (player == null)
			{
				player = CEnemyPatternManager.Instance.playerObj.GetComponent<IDamageable>();
			}

			player.OnDamage(1f, Vector3.zero);
		}

		else
		{
			if (player != null) player = null;
		}
	}

	private void OnDisable()
	{
		this.bIsCollision = false;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(KDefine.TAG_PLAYER))
		{
			IDamageable target = collision.GetComponent<IDamageable>();
			bIsCollision = true;

			if (target != null)
			{
				target.OnDamage(1f, Vector3.zero);
			}
		}

	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag(KDefine.TAG_PLAYER))
		{
			bIsCollision = false;
		}
	}

}
