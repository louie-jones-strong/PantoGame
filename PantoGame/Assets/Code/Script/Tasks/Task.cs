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
public class Task : ScriptableObject //this needs to be ScriptableObject so that it doesn't save it as the default type
{
	public int TaskUiPriority = 1;
	public string TaskId;
	public List<TaskStateRequirement> StartRequiredActionStates = new List<TaskStateRequirement>();
	public List<TaskStateRequirement> EndRequiredActionStates = new List<TaskStateRequirement>();
	protected virtual string TaskIconPath {get { return ""; }}

	public Sprite TaskIcon {get; private set;}
	public eTaskState State {get; private set;}
	public PlayerAgent PlayerDoingTask {get; protected set;}
	public float Progress {get; protected set;}

	bool DoneSetup;

	public Task()
	{
		TaskId = this.GetType().FullName;
	}

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

		if (newState == eTaskState.Completed &&
			State != eTaskState.Completed &&
			TaskUiPriority >= 0)
		{
			AudioManger.PlayEvent("TaskComplete");
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
	public virtual void DrawTask(Scene scene)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Task UI Priority");
		TaskUiPriority = EditorGUILayout.IntField(TaskUiPriority);
		EditorGUILayout.EndHorizontal();

		if (TaskUiPriority < 0)
		{
			EditorGUILayout.HelpBox($"This Task Will not be shown In UI", this is PlayerTask ? MessageType.Error : MessageType.Info);
		}

		EditorGUILayout.BeginHorizontal();

		GUILayout.Label("Start Required Action States");

		if (GUILayout.Button("Add"))
		{
			StartRequiredActionStates.Add(new TaskStateRequirement());
		}

		EditorGUILayout.EndHorizontal();

		DrawRequiredActionStates(StartRequiredActionStates, scene);

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("End Required Action States");

		if (GUILayout.Button("Add"))
		{
			EndRequiredActionStates.Add(new TaskStateRequirement());
		}

		EditorGUILayout.EndHorizontal();

		DrawRequiredActionStates(EndRequiredActionStates, scene);
	}

	void DrawRequiredActionStates(List<TaskStateRequirement> requiredActionStates, Scene scene)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical();


		int loop = 0;
		while (loop < requiredActionStates.Count)
		{
			var required = requiredActionStates[loop];

			EditorGUILayout.BeginHorizontal();
			
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

			if (required.TaskId == TaskId)
			{
				EditorGUILayout.HelpBox($"TaskID \"{required.TaskId}\" Cannot be set to the as this task", MessageType.Error);
			}
			else if (!scene.CheckTaskId(required.TaskId))
			{
				EditorGUILayout.HelpBox($"TaskID \"{required.TaskId}\" Not found in scene", MessageType.Error);
			}

			loop += 1;
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
	}

	public static int TaskSortOrder(Task a, Task b)
	{
		if (a.StartRequiredActionStates.Count == 0 &&
			b.StartRequiredActionStates.Count > 0)
		{
			//b is later in the show then a
			return -1;
		}

		if (b.StartRequiredActionStates.Count == 0 &&
			a.StartRequiredActionStates.Count > 0)
		{
			//a is later in the show then b
			return 1;
		}

		//check if b waits on a
		foreach (var item in b.StartRequiredActionStates)
		{
			if (item.TaskId == a.TaskId)
			{
				//b is later in the show then a
				return -1;
			}
		}
		foreach (var item in b.EndRequiredActionStates)
		{
			if (item.TaskId == a.TaskId)
			{
				//b is later in the show then a
				return -1;
			}
		}


		//check if a waits on b
		foreach (var item in a.StartRequiredActionStates)
		{
			if (item.TaskId == b.TaskId)
			{
				//a is later in the show then b
				return 1;
			}
		}
		foreach (var item in a.EndRequiredActionStates)
		{
			if (item.TaskId == b.TaskId)
			{
				//a is later in the show then b
				return 1;
			}
		}

		//they are both equal
		return 0;
	}

#endif
}