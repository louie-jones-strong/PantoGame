using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class MoveSetTask : PlayerTask
{
	protected override string TaskIconPath {get { return "MoveSetTaskIcon"; }}

	public Transform Target;

	public SetPiece SetPiece;

	float LargestDistance = 1;

	public override bool EndConditionsMet()
	{
		var distance = DistanceUtility.Get2d(SetPiece.transform, Target.transform);
		bool withinRange = distance <= 2;
		return withinRange && base.EndConditionsMet();
	}

	public override float GetProgress()
	{
		var distance = DistanceUtility.Get2d(SetPiece.transform, Target.transform);
		if (distance >= LargestDistance)
		{
			LargestDistance = distance;
		}
		return 1 - (distance / LargestDistance);
	}

	public override void Update()
	{
		PlayerDoingTask = SetPiece.CurrentUser;
		SetPiece.SetHighlight(State == eTaskState.CanStart ||
							State == eTaskState.InProgress);
		base.Update();
	}

#if UNITY_EDITOR
	public override void DrawTask(Scene scene)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target");
		Target = (Transform)EditorGUILayout.ObjectField(Target, typeof(Transform), true);
		EditorGUILayout.EndHorizontal();
		
		if (Target == null)
		{
			EditorGUILayout.HelpBox($"Target Cannot be empty", MessageType.Error);
		}

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("SetPiece");
		SetPiece = (SetPiece)EditorGUILayout.ObjectField(SetPiece, typeof(SetPiece), true);
		EditorGUILayout.EndHorizontal();

		if (SetPiece == null)
		{
			EditorGUILayout.HelpBox($"SetPiece Cannot be empty", MessageType.Error);
		}

		base.DrawTask(scene);
	}
#endif
}