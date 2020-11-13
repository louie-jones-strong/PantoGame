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

	public override bool EndConditionsMet()
	{
		bool withinRange = (Light.LightToControl.transform.position - Target.transform.position).magnitude <= 2;
		return withinRange && base.EndConditionsMet();
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