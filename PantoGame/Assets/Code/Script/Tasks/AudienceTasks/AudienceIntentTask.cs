using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class AudienceIntentTask : Task
{
	public eAudienceIntent TargetIntent;
	public float TargetTimeForIntent = 5f;
	float TimeInIntent = 0;

	public override bool EndConditionsMet()
	{
		return TimeInIntent >= TargetTimeForIntent && base.EndConditionsMet();
	}

	public override void Update()
	{
		if (State == eTaskState.CanStart ||
			State == eTaskState.InProgress)
		{
			TimeInIntent += Time.deltaTime;
		}
		base.Update();
	}
	
	public override float GetProgress()
	{
		return TimeInIntent / TargetTimeForIntent;
	}

#if UNITY_EDITOR
	public override void DrawTask(Scene scene)
	{
		EditorGUILayout.BeginHorizontal();
		TargetIntent = (eAudienceIntent)EditorGUILayout.EnumPopup(new GUIContent("Target Intent"), TargetIntent);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target Time For Intent");
		TargetTimeForIntent = EditorGUILayout.FloatField(TargetTimeForIntent);
		EditorGUILayout.EndHorizontal();

		base.DrawTask(scene);
	}
#endif
}