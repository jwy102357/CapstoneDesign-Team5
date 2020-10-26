using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BTNode
{
	public BTState NodeState
	{
		get { return currentState; }
	}
	protected BTState currentState;


	public BTNode () { }

	public abstract BTState Evaluate();
}
