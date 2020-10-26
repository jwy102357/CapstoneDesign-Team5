using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRandomMoveAI
{
	bool IsMoving { get; set; }
	int RandomCount { get; set; }

	void RandomMoveEnd(WaitForSeconds waitSec);
	void StartIdle();
}
