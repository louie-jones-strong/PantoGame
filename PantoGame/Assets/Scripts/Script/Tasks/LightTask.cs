using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class LightTask : Task
{
	public Transform Target;

	public LightingDesk Light;

	float LargestDistance = 1;

	public override bool EndConditionsMet()
	{
		var distance = GetDistance();
		bool withinRange = distance <= 2;
		return withinRange && base.EndConditionsMet();
	}

	public override float GetProgress()
	{
		var distance = GetDistance();
		if (distance >= LargestDistance)
		{
			LargestDistance = distance;
		}
		return 1 - (distance / LargestDistance);
	}

	float GetDistance()
	{
		var delta = Light.LightToControl.transform.position - Target.transform.position;

		var distance = (new Vector2(delta.x, delta.z)).magnitude;
		return distance;
	}

	public override void Update()
	{
		PlayerDoingTask = Light.CurrentUser;
		base.Update();
	}

#if UNITY_EDITOR
	public override void DrawTask()
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target");
		Target = (Transform)EditorGUILayout.ObjectField(Target, typeof(Transform));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Light");
		Light = (LightingDesk)EditorGUILayout.ObjectField(Light, typeof(LightingDesk));
		EditorGUILayout.EndHorizontal();

		base.DrawTask();
	}
#endif
}