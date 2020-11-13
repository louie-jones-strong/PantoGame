using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class ActorTask : Task
{
	public Transform Target;

	public ActorAgent Actor;

	public override bool EndConditionsMet()
	{
		bool withinRange = (Actor.transform.position - Target.transform.position).magnitude <= 2;
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
		EditorGUILayout.LabelField("Actor");
		Actor = (ActorAgent)EditorGUILayout.ObjectField(Actor, typeof(ActorAgent));
		EditorGUILayout.EndHorizontal();

		base.DrawTask();
	}
#endif
}