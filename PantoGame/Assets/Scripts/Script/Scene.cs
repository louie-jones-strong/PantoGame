using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class Scene
{
	public string SceneName;
	public List<Task> Tasks;
	
	public Scene()
	{
		Tasks = new List<Task>();
	}

	public Scene(Scene sceneToCopy)
	{
		Tasks = sceneToCopy.Tasks;
	}

#if UNITY_EDITOR
	Dictionary<int, bool> CurrentEditActions = new Dictionary<int, bool>();


	eActionType CurrentActionType;
	enum eActionType
	{
		None,
		Light,
		Actor,
	}

	public void OnDrawScene()
	{

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Separator();

		CurrentActionType = (eActionType)EditorGUILayout.EnumPopup(new GUIContent("Action Type"), CurrentActionType);
		if (GUILayout.Button("Add Task"))
		{
			var task = MakeTask(CurrentActionType);
			Tasks.Add(task);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Separator();

		int loop = 0;
		while (loop < Tasks.Count)
		{
			var action = Tasks[loop];
			if (action == null)
			{
				Tasks.RemoveAt(loop);
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
				Tasks.RemoveAt(loop);
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
#endif
}