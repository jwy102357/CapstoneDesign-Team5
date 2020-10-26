using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageAI
{ 
	bool IsDamaged { get; set; }
	bool IsDead { get; set; }
}
