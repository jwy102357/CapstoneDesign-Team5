using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CComponent : MonoBehaviour
{
	[HideInInspector] public Transform _transform = null;
	[HideInInspector] public Rigidbody2D _rigidbody = null;
	// Start is called before the first frame update
	public virtual void Awake()
	{
		_transform = this.GetComponent<Transform>();
		_rigidbody = this.GetComponent<Rigidbody2D>();
	}

}   
   
