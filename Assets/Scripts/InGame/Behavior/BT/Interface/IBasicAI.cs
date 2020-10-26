using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public interface IBasicAI
{
	GameObject Target { get; set; }
	CLivingEntity LivingEntity { get; set; }
	bool FacingRight { get; set; }
	bool IsFlight { get; set; }
	bool IsAttack { get; set; }
	bool IsMoving { get; set; }
	bool IsIdle { get; set; }
	bool IsAttackDelay { get; set; }
	float Speed { get; set; }
	CEnemySight EnemySight { get; set; }
    void AttackEnd();
	void SetTarget();
	void MoveTimeChk(float seconds);
	void IdleTimeChk(float seconds);
	float fIdleTime { get; set; }

	Vector3 GetTargetPosition();
	Animator GetAnimator();
	Transform GetTransform();
	float GetTargetDistance();

}