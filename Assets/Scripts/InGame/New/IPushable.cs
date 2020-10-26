using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable
{
	bool IsContact { get; set; }
	void Push(float fDir);
}
