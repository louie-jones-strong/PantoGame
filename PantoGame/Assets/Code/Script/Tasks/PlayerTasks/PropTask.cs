using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class PropTask : PlayerTask
{
	protected override string TaskIconPath {get { return "PropTaskIcon"; }}

	public Prop TargetProp;
	public PropHolder TargetPropHolder;
	public Transform Target;

	float LargestDistance = 1;
	
	public override bool EndConditionsMet()
	{
		if (Target != null)
		{
			var distance = DistanceUtility.Get2d(TargetProp.transform, Target.transform);
			bool withinRange = distance <= 2;
			if (distance > 2)
			{
				return false;
			}
		}
		
		bool correctHolder = TargetPropHolder == TargetProp.PropHolder;
		return correctHolder && base.EndConditionsMet();
	}

	public override float GetProgress()
	{
		if (Target == null)
		{
			return EndConditionsMet() ? 1f : 0f;
		}

		var distance = DistanceUtility.Get2d(TargetProp.transform, Target.transform);
		if (distance >= LargestDistance)
		{
			LargestDistance = distance;
		}
		return 1 - (distance / LargestDistance);
	}

	public override void Update()
	{
		var player = TargetProp.PropHolder;
		PlayerDoingTask = player as PlayerAgent;

		base.Update();
	}

#if UNITY_EDITOR
	public override void DrawTask(Scene scene)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("TargetProp");
		TargetProp = (Prop)EditorGUILayout.ObjectField(TargetProp, typeof(Prop), true);
		EditorGUILayout.EndHorizontal();

		if (TargetProp == null)
		{
			EditorGUILayout.HelpBox($"TargetProp Cannot be empty", MessageType.Error);
		}

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target Prop Holder");
		TargetPropHolder = (PropHolder)EditorGUILayout.ObjectField(TargetPropHolder, typeof(PropHolder), true);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target");
		Target = (Transform)EditorGUILayout.ObjectField(Target, typeof(Transform), true);
		EditorGUILayout.EndHorizontal();
		
		if (TargetPropHolder == null && Target == null)
		{
			EditorGUILayout.HelpBox($"both TargetPropHolder and Target are empty", MessageType.Error);
		}

		base.DrawTask(scene);
	}
#endif
}