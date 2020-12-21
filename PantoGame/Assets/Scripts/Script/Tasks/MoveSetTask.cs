using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class MoveSetTask : Task
{
	protected override string TaskIconPath {get { return "MoveSetTaskIcon"; }}

	public Transform Target;

	public SetPiece SetPiece;

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
		var delta = SetPiece.transform.position - Target.transform.position;

		var distance = (new Vector2(delta.x, delta.z)).magnitude;
		return distance;
	}

	public override void Update()
	{
		PlayerDoingTask = SetPiece.CurrentUser;
		base.Update();
	}

#if UNITY_EDITOR
	public override void DrawTask()
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target");
		Target = (Transform)EditorGUILayout.ObjectField(Target, typeof(Transform), true);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("SetPiece");
		SetPiece = (SetPiece)EditorGUILayout.ObjectField(SetPiece, typeof(SetPiece), true);
		EditorGUILayout.EndHorizontal();

		base.DrawTask();
	}
#endif
}