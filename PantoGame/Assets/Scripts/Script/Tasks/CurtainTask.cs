using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class CurtainTask : Task
{
	public Curtain TargetCurtain;
	public float TargetOpenAmount;
	
	public override bool EndConditionsMet()
	{
		var correctState = Mathf.Abs(TargetCurtain.OpenAmount - TargetOpenAmount) <= Mathf.Epsilon;
		return correctState && base.EndConditionsMet();
	}

	public override void Update()
	{
		PlayerDoingTask = TargetCurtain.CurrentUser;
		base.Update();
	}

#if UNITY_EDITOR
	public override void DrawTask()
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Curtain");
		TargetCurtain = (Curtain)EditorGUILayout.ObjectField(TargetCurtain, typeof(Curtain));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("TargetOpenAmount");
		TargetOpenAmount = EditorGUILayout.Slider(TargetOpenAmount, 0f, 1f);
		EditorGUILayout.EndHorizontal();

		base.DrawTask();
	}
#endif
}