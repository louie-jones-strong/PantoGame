using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceUtility
{
	public static float GetUi2d(Transform object1, Transform object2)
	{
		var delta = object1.position - object2.position;

		var distance = (new Vector2(delta.x, delta.y)).magnitude;
		return distance;
	}

	public static float Get2d(Transform object1, Transform object2)
	{
		var delta = object1.position - object2.position;

		var distance = (new Vector2(delta.x, delta.z)).magnitude;
		return distance;
	}

	public static float Get3d(Transform object1, Transform object2)
	{
		var distance = (object1.position - object2.position).magnitude;
		return distance;
	}
}