using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public enum eTaskState
{
	NotStarted,
	InProgress,
	Completed
}

[Serializable]
public class TaskStateRequirement
{
	public string TaskId;
	public eTaskState State;
}

[Serializable]
public class Task : ScriptableObject
{
	public eTaskState State {get; private set;}
	public string TaskId;

	public List<TaskStateRequirement> StartRequiredActionStates = new List<TaskStateRequirement>();
	public List<TaskStateRequirement> EndRequiredActionStates = new List<TaskStateRequirement>();

	public virtual bool StartConditionsMet()
	{
		var tasks = Theatre.Instance.CurrentScript.CurrentScene.Tasks;

		foreach (var requirement in StartRequiredActionStates)
		{
			foreach (var item in tasks)
			{
				if(requirement.TaskId == item.TaskId)
				{
					if (requirement.State != item.State)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public virtual bool EndConditionsMet()
	{
		var tasks = Theatre.Instance.CurrentScript.CurrentScene.Tasks;

		foreach (var requirement in EndRequiredActionStates)
		{
			foreach (var item in tasks)
			{
				if(requirement.TaskId == item.TaskId)
				{
					if (requirement.State != item.State)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public void SetState(eTaskState newState)
	{
		State = newState;
	}

#if UNITY_EDITOR
	public virtual void DrawTask()
	{
		EditorGUILayout.BeginHorizontal();

		GUILayout.Label("Start Required Action States");

		if (GUILayout.Button("Add"))
		{
			StartRequiredActionStates.Add(new TaskStateRequirement());
		}

		EditorGUILayout.EndHorizontal();

		DrawRequiredActionStates(StartRequiredActionStates);

		EditorGUILayout.Separator();
		EditorGUILayout.Separator();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("End Required Action States");

		if (GUILayout.Button("Add"))
		{
			EndRequiredActionStates.Add(new TaskStateRequirement());
		}

		EditorGUILayout.EndHorizontal();

		DrawRequiredActionStates(EndRequiredActionStates);
	}

	void DrawRequiredActionStates(List<TaskStateRequirement> requiredActionStates)
	{
		EditorGUILayout.Separator();

		int loop = 0;
		while (loop < requiredActionStates.Count)
		{
			var required = requiredActionStates[loop];

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Separator();
			GUILayout.Label("TaskID:");
			required.TaskId = GUILayout.TextField(required.TaskId);
			required.State = (eTaskState)EditorGUILayout.EnumPopup(new GUIContent(), required.State);

			GUI.backgroundColor = Color.red;
			if (GUILayout.Button("Remove"))
			{
				requiredActionStates.RemoveAt(loop);
				break;
			}
			GUI.backgroundColor = Color.white;
			EditorGUILayout.EndHorizontal();

			loop += 1;
		}
	}
#endif
}