using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCam : MonoBehaviour
{
	protected virtual void LateUpdate()
	{
		PointAtCam(transform);
	}

	public static void PointAtCam(Transform targetTransform)
	{
		targetTransform.eulerAngles = CameraController.Instance.Camera.transform.eulerAngles * Settings.RotateToCamMultiplier;
	}
}
