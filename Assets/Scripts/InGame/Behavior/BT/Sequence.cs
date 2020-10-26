using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : BTNode
{
	private List<BTNode> myNodeList = new List<BTNode>();

	public Sequence(List<BTNode> nodeList)
	{
		myNodeList = nodeList;
	}

	public override BTState Evaluate()
	{
		bool bIsRunning = false;
		
		for (int i = 0; i < myNodeList.Count; ++i)
		{
			switch(myNodeList[i].Evaluate())
			{
				case BTState.FAILURE :
					currentState = BTState.FAILURE;
					return currentState;

				case BTState.SUCCESS :
					continue;

				case BTState.RUNNING :
					bIsRunning = true;
					continue;

				default:
					currentState = BTState.SUCCESS;
					return currentState;
			}
		}
		currentState = bIsRunning ? BTState.RUNNING : BTState.SUCCESS;
		return currentState;
	}
}
