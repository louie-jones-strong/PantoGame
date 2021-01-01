using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class CurtainTask : PlayerTask
{
	protected override string TaskIconPath {get { return "CurtainsTaskIcon"; }}

	public Curtain TargetCurtain;
	public float TargetOpenAmount;
	
	public override bool EndConditionsMet()
	{
		var correctState = Mathf.Abs(TargetCurtain.OpenAmount - TargetOpenAmount) <= Mathf.Epsilon;
		return correctState && base.EndConditionsMet();
	}

	public override float GetProgress()
	{
		return 1 - Mathf.Abs(TargetCurtain.OpenAmount - TargetOpenAmount);
	}

	public override void Update()
	{
		PlayerDoingTask = TargetCurtain.CurrentUser;
		TargetCurtain.SetHighlight(State == eTaskState.CanStart ||
							State == eTaskState.InProgress);
		base.Update();
	}

#if UNITY_EDITOR
	public override void DrawTask(Scene scene)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Curtain");
		TargetCurtain = (Curtain)EditorGUILayout.ObjectField(TargetCurtain, typeof(Curtain), true);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("TargetOpenAmount");
		TargetOpenAmount = EditorGUILayout.Slider(TargetOpenAmount, 0f, 1f);
		EditorGUILayout.EndHorizontal();

		base.DrawTask(scene);
	}
#endif
}