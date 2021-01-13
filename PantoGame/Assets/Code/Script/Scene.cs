using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public enum eSceneState
{
	NotStarted,
	InProgress,
	Completed
}

[Serializable]
public class Scene
{
	public string SceneName;
	public List<Task> Tasks;
	public eSceneState State {get; private set;}
	
	public Scene()
	{
		Tasks = new List<Task>();
	}

	public void SetState(eSceneState state)
	{
		if (state == eSceneState.Completed &&
			State != eSceneState.Completed)
		{
			AudioManger.PlayEvent("SceneComplete");
		}

		State = state;
	}

	public void Update()
	{
		if (State == eSceneState.Completed ||
			State == eSceneState.NotStarted)
		{
			return;
		}

		bool allStatesFinished = true;
		foreach (var task in Tasks)
		{
			task.Update();
			if (task.State != eTaskState.Completed)
			{
				allStatesFinished = false;
			}
		}
		
		if (allStatesFinished)
		{
			Logger.Log($"set Scene ({SceneName}) State {State} -> {eSceneState.Completed}");
			SetState(eSceneState.Completed);
		}
	}

	public bool CheckTaskId(string taskId)
	{
		foreach (var task in Tasks)
		{
			if (task.TaskId == taskId)
			{
				return true;
			}
		}
		return false;
	}

#if UNITY_EDITOR
	Dictionary<int, bool> CurrentEditActions = new Dictionary<int, bool>();


	eTaskType CurrentActionType;
	enum eTaskType
	{
		None,
		WaitTask,
		AudioTrigger,
		PropTask,
		Light,
		Actor,
		ActorTalk,
		Curtain,
		AudienceIntent,
		Clapping,
		Toilet,
		MoveSet,
		StandInLobby
	}

	public void OnDrawScene()
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical();

		EditorGUILayout.BeginHorizontal();
		

		CurrentActionType = (eTaskType)EditorGUILayout.EnumPopup(new GUIContent("Action Type"), CurrentActionType);
		if (GUILayout.Button("Add Task"))
		{
			var task = MakeTask(CurrentActionType);
			Tasks.Add(task);
		}
		EditorGUILayout.EndHorizontal();

		int loop = 0;
		while (loop < Tasks.Count)
		{
			var task = Tasks[loop];
			if (task == null)
			{
				Tasks.RemoveAt(loop);
				break;
				
			}

			EditorGUILayout.Space(10);

			EditorGUILayout.BeginHorizontal();
			if (!CurrentEditActions.ContainsKey(loop))
			{
				CurrentEditActions[loop] = false;
			}
			CurrentEditActions[loop] = EditorGUILayout.BeginFoldoutHeaderGroup(CurrentEditActions[loop], $"Action ID:");

			task.TaskId = EditorGUILayout.TextField(task.TaskId);

			EditorGUILayout.LabelField($"Task Type: {task.GetType()}");

			GUILayout.Label($"State: {task.State}");


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

				task.DrawTask(this);

				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndFoldoutHeaderGroup();
			loop += 1;
		}

		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space(10);
	}

	public void SortTasks()
	{
		Tasks.Sort(Task.TaskSortOrder);
	}

	Task MakeTask(eTaskType actionType)
	{
		switch (actionType)
		{
			case eTaskType.WaitTask:
			{
				return new WaitTask();
			}
			case eTaskType.PropTask:
			{
				return new PropTask();
			}
			case eTaskType.Light:
			{
				return new LightTask();
			}
			case eTaskType.Actor:
			{
				return new ActorTask();
			}
			case eTaskType.ActorTalk:
			{
				return new ActorTalkTask();
			}
			case eTaskType.Curtain:
			{
				return new CurtainTask();
			}
			case eTaskType.MoveSet:
			{
				return new MoveSetTask();
			}
			case eTaskType.AudioTrigger:
			{
				return new AudioTriggerTask();
			}
			case eTaskType.AudienceIntent:
			{
				return new AudienceIntentTask();
			}
			default:
			{
				return new Task();
			}
		}
	}
#endif
}