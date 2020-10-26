using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALGPS01EnemyController : CRangedEnemyController
{
	LineRenderer lineRenderer;
	int wallLayerMask;
	RaycastHit2D rayCastHit2D;
	bool bIsShotReady = false;
	Vector2 LineDir;
	float fShotTime = 1f;
	Transform linePosition;

	public override void Awake()
	{
		base.Awake();
		lineRenderer = this.GetComponentInChildren<LineRenderer>();
		lineRenderer.startColor = new Color(1, 1, 1, 0.5f);
		lineRenderer.endColor = new Color(1, 1, 1, 0.5f);
		lineRenderer.startWidth = 1.5f;
		lineRenderer.endWidth = 1.5f;
		//linePosition = lineRenderer.transform;
		wallLayerMask = (1 << LayerMask.NameToLayer(KDefine.LAYER_WALL)) + (1 << LayerMask.NameToLayer(KDefine.LAYER_GROUND));
	}

	public override void MoveForwardObject()
	{
		base.MoveForwardObject();
	}

	public override void TurnObject()
	{
		base.TurnObject();
	}

	public override void AttackObject(int nDiceValue)
	{
		base.AttackObject(nDiceValue);
		this.MakeDangerDir();
	}

	public override void OnDisable()
	{
		base.OnDisable();
		this.bIsShotReady = false;
		lineRenderer.positionCount = 0;
	}

	public override void OnEnable()
	{
		base.OnEnable();
	}


	void Shot()
	{
		bIsShotReady = false;
		lineRenderer.positionCount = 0;
		CEnemyPatternManager.Instance.NormalBulletAttack(this.GetDirection().normalized, 
			lineRenderer.transform.position, fBulletSpeed);
	}
	
	public void MakeDangerLine()
	{
		bIsShotReady = true;
		lineRenderer.positionCount = 1;
		lineRenderer.SetPosition(0, lineRenderer.transform.position);
		rayCastHit2D = Physics2D.Raycast(lineRenderer.transform.position, LineDir, 100f, wallLayerMask);
		if(rayCastHit2D.collider)
		{
			lineRenderer.positionCount++;
			lineRenderer.SetPosition(1, rayCastHit2D.point);
		}
	}

	void MakeDangerDir()
	{
		LineDir = this.GetDirection();
	}
}
