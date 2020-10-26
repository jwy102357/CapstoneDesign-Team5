using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : BTNode
{
	private List<BTNode> myNodeList = new List<BTNode>();

	public Selector(List<BTNode> nodeList)
	{
		myNodeList = nodeList;
	}

	public override BTState Evaluate()
	{
		for (int i = 0; i < myNodeList.Count; ++i)
		{
			switch(myNodeList[i].Evaluate())
			{
				case BTState.FAILURE:
					continue;

				case BTState.SUCCESS:
					currentState = BTState.SUCCESS;
					return currentState;

				case BTState.RUNNING:
					currentState = BTState.RUNNING;
					return currentState;

				default:
					continue;
			}
		}
		currentState = BTState.FAILURE;
		return currentState;
	}
}
