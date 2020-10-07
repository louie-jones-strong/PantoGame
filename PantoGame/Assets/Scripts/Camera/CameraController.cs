using UnityEngine;
using System;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance;
	
	[SerializeField] Camera Camera;
	List<Transform> TargetList;
	float MoveSmoothTime = 0.5f;
	float MinZoom = 70;
	float MaxZoom = 20;
	float MaxTargetDistance = 50f;


	Vector3 Velocity;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			TargetList = new List<Transform>();
		}
		else
		{
			Logger.LogError("CameraController.Instance already set");
		}
	}

	void OnDestroy()
	{
		Instance = null;
	}

	public static void AddTarget(Transform toAdd)
	{
		if (Instance == null)
		{
			return;
		}

		Instance.TargetList.Add(toAdd);
	}

	public static void RemoveTarget(Transform toRemove)
	{
		if (Instance == null)
		{
			return;
		}

		Instance.TargetList.Remove(toRemove);
	}


	void LateUpdate()
	{
		if (TargetList.Count == 0)
		{
			return;
		}

		SetPos();
		SetZoom();
	}

	void SetPos()
	{
		var targetPos = GetCenterOfTargets();
		transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref Velocity, MoveSmoothTime);
	}

	void SetZoom()
	{
		var maxDistance = GetMaxDistanceBetweenTargets();
		var normalizedMaxDistance = maxDistance / MaxTargetDistance;
		normalizedMaxDistance = Mathf.Clamp(normalizedMaxDistance, 0, 1);

		var zoomValue = Mathf.Lerp(MaxZoom, MinZoom, normalizedMaxDistance);
		Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, zoomValue, Time.deltaTime);
	}

	Vector3 GetCenterOfTargets()
	{
		return GetTargetsBounds().center;
	}

	float GetMaxDistanceBetweenTargets()
	{
		var boundsSize = GetTargetsBounds().size;
		return Mathf.Max(boundsSize.x, boundsSize.y, boundsSize.z);
	}

	Bounds GetTargetsBounds()
	{
		var bounds = new Bounds(TargetList[0].position, Vector3.zero);
		foreach (var target in TargetList)
		{
			bounds.Encapsulate(target.position);
		}
		return bounds;
	}
}