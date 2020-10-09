using UnityEngine;
using System;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance;
	
	public Camera Camera;
	List<TrackerTarget> TargetList;
	class TrackerTarget
	{
		public Transform Transform;
		public float Weighting;

		public TrackerTarget(Transform target, float weighting)
		{
			Transform = target;
			Weighting = weighting;
		}
	}

	float MoveSmoothTime = 0.5f;
	[SerializeField] float MinZoom = 70;
	[SerializeField] float MaxZoom = 20;
	[SerializeField] float MaxTargetDistance = 50f;

	[SerializeField] float MinXMove = -7.5f;
	[SerializeField] float MaxXMove = 7.5f;

	[SerializeField] float MinYMove = -7.5f;
	[SerializeField] float MaxYMove = 7.5f;


	Vector3 Velocity;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			TargetList = new List<TrackerTarget>();
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

	public static void AddTarget(Transform toAdd, float weighting=1)
	{
		if (Instance == null)
		{
			return;
		}
		var trackerTarget = new TrackerTarget(toAdd, weighting);

		Instance.TargetList.Add(trackerTarget);
	}

	public static void RemoveTarget(Transform toRemove)
	{
		if (Instance == null)
		{
			return;
		}

		int index = 0;
		while (Instance.TargetList.Count <= index)
		{
			
			if (Instance.TargetList[index].Transform == toRemove)
			{
				Instance.TargetList.RemoveAt(index);
			}
			else
			{
				index += 1;
			}
		}
	}


	void LateUpdate()
	{
		if (TargetList.Count == 0)
		{
			return;
		}

		//set the pos of the cam
		var centerPos = GetCenterOfTargets();
		centerPos = LimitMoveAmount(centerPos);
	

		transform.position = Vector3.SmoothDamp(transform.position, centerPos, ref Velocity, MoveSmoothTime);

		//set the zoom of the cam
		var maxDistance = GetMaxDistanceFromCenter(centerPos);
		maxDistance *= 2;
		var normalizedMaxDistance = maxDistance / MaxTargetDistance;
		normalizedMaxDistance = Mathf.Clamp(normalizedMaxDistance, 0, 1);

		var zoomValue = Mathf.Lerp(MaxZoom, MinZoom, normalizedMaxDistance);
		
		var newZoom = Mathf.Lerp(Camera.fieldOfView, zoomValue, Time.deltaTime);
		Camera.fieldOfView = newZoom;
		Camera.orthographicSize = newZoom/2;
	}
	
	Vector3 LimitMoveAmount(Vector3 centerPos)
	{
		float x = Mathf.Clamp(centerPos.x, MinXMove, MaxXMove);
		float y = centerPos.y;
		float z = Mathf.Clamp(centerPos.z, MinYMove, MaxYMove);
		return new Vector3(x, y, z);
	}

	Vector3 GetCenterOfTargets()
	{
		var center = Vector3.zero;
		float totalWeight = 0;
		foreach (var target in TargetList)
		{
			center += target.Transform.position * target.Weighting;
			totalWeight += target.Weighting;
		}
		return center / totalWeight;
	}

	float GetMaxDistanceFromCenter(Vector3 center)
	{
		float largestDistance = 0;
		foreach (var target in TargetList)
		{
			var distance = (target.Transform.position - center).magnitude;
			if (distance >= largestDistance)
			{
				largestDistance = distance;
			}
		}

		return largestDistance;
	}

}