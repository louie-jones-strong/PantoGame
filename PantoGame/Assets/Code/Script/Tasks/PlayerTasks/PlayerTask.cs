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
	float TimingAccuracyMultiplayer = 1f;
	float RatingPriorityMultiplayer = 1f;

	float TimeSinceStartAble;
	
	public float GetRating()
	{
		if (State != eTaskState.Completed && TimeUntilDue >= 0)
		{
			return 0;
		}
	
		return TimeToRating(TimeUntilDue);
	}

	float TimeToRating(float time)
	{
		float xValue = time * TimingAccuracyMultiplayer;

		xValue = Mathf.Clamp(xValue, -Mathf.PI, Mathf.PI);
		float yValue = Mathf.Cos(xValue);
		yValue *= RatingPriorityMultiplayer;

		return yValue;
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
	public override void DrawTask()
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("TargetCompleteTime");
		TargetCompleteTime = EditorGUILayout.FloatField(TargetCompleteTime);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("TimingAccuracyMultiplayer");
		TimingAccuracyMultiplayer = EditorGUILayout.FloatField(TimingAccuracyMultiplayer);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("RatingPriorityMultiplayer");
		RatingPriorityMultiplayer = EditorGUILayout.FloatField(RatingPriorityMultiplayer);
		EditorGUILayout.EndHorizontal();

		base.DrawTask();
	}
#endif
}