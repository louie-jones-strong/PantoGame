using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskListUi : MonoBehaviour
{
	[SerializeField] Animator ListAnimator;
	[SerializeField] TaskItemUi Prefab;
	[SerializeField] Text SceneName;
	[SerializeField] Transform Root;

	List<TaskItemUi> TaskItemPool = new List<TaskItemUi>();
	List<Task> LastTask;
	int CurrentSceneIndex;
	
	void Awake()
	{
		Prefab.gameObject.SetActive(false);
	}

	void Update()
	{
		if (Theatre.Instance?.CurrentScript == null)
		{
			return;
		}
		if(Theatre.Instance.CurrentScript.Scenes.Count <= CurrentSceneIndex)
		{
			return;
		}
		var scene = Theatre.Instance.CurrentScript.Scenes[CurrentSceneIndex];

		SceneName.text = $"Scene: {scene.SceneName}";

		if (LastTask != scene.Tasks)
		{
			UpdateTasks(scene);
			LastTask = scene.Tasks;
		}

		ListAnimator.SetBool("NextScene", CurrentSceneIndex != Theatre.Instance.CurrentScript.SceneIndex);
	}

	//anim event
	public void SwapToNextSceneEvent()
	{
		if (CurrentSceneIndex >= Theatre.Instance.CurrentScript.SceneIndex)
		{
			CurrentSceneIndex = Theatre.Instance.CurrentScript.SceneIndex;
		}
		else
		{
			CurrentSceneIndex += 1;
		}
	}

	void UpdateTasks(Scene scene)
	{
		HideAllPoolItems();

		foreach (var task in scene.Tasks)
		{
			if (task.TaskPriority > 0)
			{
				var item = GetItemUi();
				item.Setup(task);
			}
		}
	}

	void HideAllPoolItems()
	{
		foreach (var item in TaskItemPool)
		{
			item.gameObject.SetActive(false);
		}
	}

	TaskItemUi GetItemUi()
	{
		foreach (var item in TaskItemPool)
		{
			if (!item.gameObject.activeSelf)
			{
				return item;
			}
		}
		var instance = Instantiate<TaskItemUi>(Prefab, Root);

		TaskItemPool.Add(instance);
		
		return instance;
	}
}
