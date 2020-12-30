using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class ActorTalkTask : Task
{
	public ActorAgent Actor;
	public float TargetTalkTime = 5;
	float TimeTalking = 0;

	public override bool EndConditionsMet()
	{
		return TimeTalking >= TargetTalkTime && base.EndConditionsMet();
	}

	public override void Update()
	{
		if (State == eTaskState.CanStart ||
			State == eTaskState.InProgress)
		{
			TimeTalking += Time.deltaTime;
		}
		base.Update();
	}

	public override float GetProgress()
	{
		return TimeTalking / TargetTalkTime;
	}

#if UNITY_EDITOR
	public override void DrawTask()
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Actor");
		Actor = (ActorAgent)EditorGUILayout.ObjectField(Actor, typeof(ActorAgent), true);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("TargetTalkTime");
		TargetTalkTime = EditorGUILayout.FloatField(TargetTalkTime);
		EditorGUILayout.EndHorizontal();

		base.DrawTask();
	}
#endif
}