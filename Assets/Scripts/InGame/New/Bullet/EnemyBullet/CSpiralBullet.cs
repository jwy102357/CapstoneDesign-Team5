using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSpiralBullet : CEnemyBullet
{
	public Transform _transform;
	public float fBulletSpeed;
	public float fAngle = 0f;
	private float fDivision180 = 0.0055555555555556f;
	public override void Awake()
	{
		base.Awake();
	}

	void OnEnable()
	{
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(KDefine.LAYER_ENEMYBULLET), LayerMask.NameToLayer(KDefine.LAYER_ENEMYBULLET), true);

		if (_transform == null)
		{
			_transform = this.transform;
		}
		StartCoroutine(SpiralShot());
	}

	void OnDisable()
	{
		fAngle = 0f;
		StopCoroutine(SpiralShot());
	}

	public void SetPosition(Vector3 pos)
	{
		if (_transform == null)
		{
			_transform = this.transform;
		}

		this._transform.position = pos;
		StartCoroutine(SpiralShot());
	}

	IEnumerator SpiralShot()
	{
		float fStartAnlge = fAngle;
		while(this.gameObject.activeSelf)
		{
			float dirX = this._transform.position.x + Mathf.Sin((fAngle * Mathf.PI) * fDivision180);
			float dirY = this._transform.position.y + Mathf.Cos((fAngle * Mathf.PI) * fDivision180);
			
			Vector3 bulletMoveDir = new Vector3(dirX, dirY, 0f);
			Vector2 bulletDir = (bulletMoveDir - this._transform.position).normalized;
			this.rigidbody.velocity = bulletDir * this.fBulletSpeed;
			fAngle += 26;

			if (fAngle >= fStartAnlge + 120f)
			{
				break;
			}

			if (fAngle >= 720f)
			{
				fAngle -= 720f;
			}
			yield return new WaitForSeconds(0.3f);

		}
	}
}
