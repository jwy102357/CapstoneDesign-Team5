using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ControllEvent();
public delegate void ControllEventFloat(float fValue);
public delegate void ControllEventVector3(Vector3 oVector);
public delegate void ControllEventInt(int nValue);

public interface IController
{
	event ControllEventInt AtkEvent;
	event ControllEvent MoveEvent;
	event ControllEvent TurnEvent;
	event ControllEventVector3 RandomMoveEvent;
}
