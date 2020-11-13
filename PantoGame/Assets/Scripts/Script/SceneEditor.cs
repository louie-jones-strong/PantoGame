#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(Scene))]
public class SceneEditor : Editor
{
	Dictionary<int, bool> CurrentEditActions = new Dictionary<int, bool>();


	eActionType CurrentActionType;
	enum eActionType
	{
		None,
		Light,
		Actor,
	}

	public override void OnInspectorGUI()
	{
		var targetScene = (Scene)target;
		

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Save"))
		{
			Save(targetScene);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		CurrentActionType = (eActionType)EditorGUILayout.EnumPopup(new GUIContent("Action Type"), CurrentActionType);
		if (GUILayout.Button("Add Task"))
		{
			var task = MakeTask(CurrentActionType);
			AssetDatabase.AddObjectToAsset(task, "Assets/Scripts/Script/ShowScripts/Interval.asset");
			targetScene.Tasks.Add(task);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Separator();

		int loop = 0;
		while (loop < targetScene.Tasks.Count)
		{
			var action = targetScene.Tasks[loop];
			if (action == null)
			{
				targetScene.Tasks.RemoveAt(loop);
				break;
				
			}

			EditorGUILayout.BeginHorizontal();
			if (!CurrentEditActions.ContainsKey(loop))
			{
				CurrentEditActions[loop] = false;
			}
			CurrentEditActions[loop] = EditorGUILayout.BeginFoldoutHeaderGroup(CurrentEditActions[loop], $"Action ID:");

			action.TaskId = EditorGUILayout.TextField(action.TaskId);

			EditorGUILayout.LabelField($"Task Type: {action.GetType()}");


			GUI.backgroundColor = Color.red;
			if (GUILayout.Button("Remove"))
			{
				AssetDatabase.RemoveObjectFromAsset(action);
				AssetDatabase.RemoveUnusedAssetBundleNames();
				targetScene.Tasks.RemoveAt(loop);
				break;
			}
			GUI.backgroundColor = Color.white;

			EditorGUILayout.EndHorizontal();

			if(CurrentEditActions[loop])
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Separator();

				EditorGUILayout.BeginVertical();

				action.DrawTask();

				EditorGUILayout.Separator();
				EditorGUILayout.Separator();

				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndFoldoutHeaderGroup();
			loop += 1;
		}
	}

	Task MakeTask(eActionType actionType)
	{
		switch (actionType)
		{
			case eActionType.Light:
			{
				return new LightTask();
			}
			case eActionType.Actor:
			{
				return new ActorTask();
			}
			default:
			{
				return new Task();
			}
		}
	}

	void Save(Scene targetScene)
	{
		EditorUtility.SetDirty(targetScene);
		AssetDatabase.SaveAssets();
	}
}
#endif