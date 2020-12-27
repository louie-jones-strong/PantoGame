using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCam : MonoBehaviour
{
	protected virtual void LateUpdate()
	{
		PointAtCam();
	}

	public void PointAtCam()
	{
		transform.eulerAngles = CameraController.Instance.Camera.transform.eulerAngles * Settings.RotateToCamMultiplier;
	}
}
