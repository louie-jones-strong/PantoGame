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
	public float ActorSpeed = 5;


	float LargestDistance = 1;

	public override bool EndConditionsMet()
	{
		bool withinRange = (Actor.transform.position - Target.transform.position).magnitude <= 2;
		return withinRange && base.EndConditionsMet();
	}

	public override float GetProgress()
	{
		var distance = (Actor.transform.position - Target.transform.position).magnitude;
		if (distance >= LargestDistance)
		{
			LargestDistance = distance;
		}
		return 1 - (distance / LargestDistance);
	}

#if UNITY_EDITOR
	public override void DrawTask(Scene scene)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target");
		Target = (Transform)EditorGUILayout.ObjectField(Target, typeof(Transform), true);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Actor");
		Actor = (ActorAgent)EditorGUILayout.ObjectField(Actor, typeof(ActorAgent), true);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("ActorSpeed");
		ActorSpeed = EditorGUILayout.FloatField(ActorSpeed);
		EditorGUILayout.EndHorizontal();

		base.DrawTask(scene);
	}
#endif
}