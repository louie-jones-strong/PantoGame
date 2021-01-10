using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PhysicsRotation : MonoBehaviour
{
	public Vector3 CentreOfMassOffset;
	public float Mass = 1;
	
	Quaternion StartingRotation;

	void Awake()
	{
		StartingRotation = transform.localRotation;
	}

	public void Refresh(Vector3 acceleration)
	{

		var force = Mass * acceleration;
		var rotation = force * (float)(2 * Math.PI * Time.deltaTime);
		rotation *= -1;
		//transform.Rotate(-rotation.z, 0, 0);
		transform.Rotate(0, 0, rotation.x);

	}

	public void Reset()
	{
		transform.localRotation = StartingRotation;
	}
}
