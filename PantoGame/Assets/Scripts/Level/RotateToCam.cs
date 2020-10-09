using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCam : MonoBehaviour
{
	void LateUpdate()
	{
		transform.localEulerAngles = CameraController.Instance.Camera.transform.eulerAngles * Settings.RotateToCamMultiplier;
	}
}
