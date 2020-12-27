#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(Theatre))]
public class TheatreEditor : Editor
{
	Dictionary<int, bool> CurrentEditActions = new Dictionary<int, bool>();

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		var targetTheatre = (Theatre)target;
		EditorUtility.SetDirty(target);

		var generator = targetTheatre.Generator;
		
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Add Scene"))
		{
			var scene = new Scene();
			generator.Scenes.Add(scene);
		}
		EditorGUILayout.EndHorizontal();

		int loop = 0;
		while (loop < generator.Scenes.Count)
		{
			var scene = generator.Scenes[loop];
			
			EditorGUILayout.BeginHorizontal();
			
			if (!CurrentEditActions.ContainsKey(loop))
			{
				CurrentEditActions[loop] = false;
			}
			CurrentEditActions[loop] = EditorGUILayout.BeginFoldoutHeaderGroup(CurrentEditActions[loop], $"SceneName:");

			scene.SceneName = GUILayout.TextField(scene.SceneName);

			GUI.backgroundColor = Color.red;
			if (GUILayout.Button("Remove Scene"))
			{
				generator.Scenes.RemoveAt(loop);
				break;
			}
			GUI.backgroundColor = Color.white;

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndFoldoutHeaderGroup();

			if(CurrentEditActions[loop])
			{
				EditorGUILayout.BeginVertical();

				scene.OnDrawScene();

				EditorGUILayout.Separator();
				EditorGUILayout.Separator();

				EditorGUILayout.EndVertical();
			}
			
			loop += 1;
		}
	}
}
#endif