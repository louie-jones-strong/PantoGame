using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class WaitTask : Task
{
	public float TargetWaitTime;
	float TimeSinceStartAble;
	
	public override bool EndConditionsMet()
	{
		return GetProgress() >= 1f && base.EndConditionsMet();
	}

	public override float GetProgress()
	{
		return TimeSinceStartAble / TargetWaitTime;
	}

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
	public override void DrawTask(Scene scene)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("TargetWaitTime");
		TargetWaitTime = EditorGUILayout.FloatField(TargetWaitTime);
		EditorGUILayout.EndHorizontal();

		base.DrawTask(scene);
	}
#endif
}