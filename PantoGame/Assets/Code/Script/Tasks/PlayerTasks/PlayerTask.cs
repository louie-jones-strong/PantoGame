using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class PlayerTask : Task
{
	public float TimeUntilDue {get {return TargetCompleteTime-TimeSinceStartAble;}}
	public float TargetCompleteTime = 15f;
	float TimeSinceStartAble;


	public override void Update()
	{

		if (State != eTaskState.CannotStart &&
			State != eTaskState.Completed)
		{
			TimeSinceStartAble += Time.deltaTime;
		}
		base.Update();
	}

#if UNITY_EDITOR
	public override void DrawTask()
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("TargetCompleteTime");
		TargetCompleteTime = EditorGUILayout.FloatField(TargetCompleteTime);
		EditorGUILayout.EndHorizontal();

		base.DrawTask();
	}
#endif
}