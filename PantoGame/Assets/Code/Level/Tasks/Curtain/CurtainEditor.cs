#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(Curtain), true)]
public class CurtainEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		var curtain = (Curtain)target;

		EditorGUILayout.BeginHorizontal();
		curtain.OpenAmount = EditorGUILayout.Slider(curtain.OpenAmount, 0f, 1f);
		curtain.SetOpenAmount();
		EditorGUILayout.EndHorizontal();
	}
}
#endif