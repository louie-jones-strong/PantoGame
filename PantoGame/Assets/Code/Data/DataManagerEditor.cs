#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(DataManager))]
public class DataManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var targetDataManager = (DataManager)target;

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Load"))
		{
			targetDataManager.Load();
		}
		if (GUILayout.Button("Save"))
		{
			targetDataManager.Save();
		}
		EditorGUILayout.EndHorizontal();

		DrawDefaultInspector();
	}
}
#endif