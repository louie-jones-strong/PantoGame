using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public enum eTaskState
{
	CannotStart,
	CanStart,
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
	public int TaskPriority = 1;
	public string TaskId;
	public List<TaskStateRequirement> StartRequiredActionStates = new List<TaskStateRequirement>();
	public List<TaskStateRequirement> EndRequiredActionStates = new List<TaskStateRequirement>();
	protected virtual string TaskIconPath {get { return ""; }}

	public Sprite TaskIcon {get; private set;}
	public eTaskState State {get; private set;}
	public PlayerAgent PlayerDoingTask {get; protected set;}
	public float Progress {get; protected set;}

	bool DoneSetup;

	void Setup()
	{
		if (DoneSetup)
		{
			return;
		}

		Logger.Log($"LoadTaskIcon path: {TaskIconPath}");

		var sprite = Resources.Load<Sprite>(TaskIconPath);
		TaskIcon = sprite;
		DoneSetup = true;
	}

	public virtual bool StartConditionsMet()
	{
		return CheckStateRequirement(StartRequiredActionStates);
	}

	public virtual bool EndConditionsMet()
	{
		if (!StartConditionsMet())
		{
			return false;
		}
		return CheckStateRequirement(EndRequiredActionStates);
	}

	bool CheckStateRequirement(List<TaskStateRequirement> requirements)
	{
		var tasks = Theatre.Instance.CurrentScript.CurrentScene.Tasks;

		foreach (var requirement in requirements)
		{
			bool requirementMet = false;
			foreach (var task in tasks)
			{
				if(requirement.TaskId == task.TaskId)
				{
					if (requirement.State == task.State)
					{
						requirementMet = true;
						break;
					}
					else
					{
						return false;
					}
				}
			}

			if (!requirementMet)
			{
				return false;
			}
		}
		return true;
	}

	public virtual float GetProgress()
	{
		return State == eTaskState.Completed? 1f : 0f;
	}

	public void SetState(eTaskState newState, bool force=false)
	{
		if (State == newState)
		{
			return;
		}

		if (State == eTaskState.Completed && !force)
		{
			return;
		}

		Logger.Log($"{this} set state {State} -> {newState}");
		State = newState;
	}

	public virtual void Update()
	{
		Setup();

		if (State == eTaskState.Completed)
		{
			PlayerDoingTask = null;
			return;
		}

		if (EndConditionsMet())
		{
			SetState(eTaskState.Completed);
			PlayerDoingTask = null;
		}
		else if (StartConditionsMet())
		{
			SetState(eTaskState.CanStart);
		}
		else
		{
			SetState(eTaskState.CannotStart);
		}
		
		if (StartConditionsMet())
		{
			Progress = GetProgress();
			Progress = Mathf.Clamp01(Progress);
		}
	}

#if UNITY_EDITOR
	public virtual void DrawTask()
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("TaskPriority");
		TaskPriority = EditorGUILayout.IntField(TaskPriority);
		EditorGUILayout.EndHorizontal();

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