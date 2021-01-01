using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class AudienceClapTask : Task
{
	public float TargetTimeToClap = 5f;
	float TimeClapping = 0;

	public override bool EndConditionsMet()
	{
		return TimeClapping >= TargetTimeToClap && base.EndConditionsMet();
	}

	public override void Update()
	{
		if (State == eTaskState.CanStart ||
			State == eTaskState.InProgress)
		{
			TimeClapping += Time.deltaTime;
		}
		base.Update();
	}
	
	public override float GetProgress()
	{
		return TimeClapping / TargetTimeToClap;
	}

#if UNITY_EDITOR
	public override void DrawTask(Scene scene)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Time To Clap");
		TargetTimeToClap = EditorGUILayout.FloatField(TargetTimeToClap);
		EditorGUILayout.EndHorizontal();

		base.DrawTask(scene);
	}
#endif
}