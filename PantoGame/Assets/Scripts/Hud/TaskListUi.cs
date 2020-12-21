using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskListUi : MonoBehaviour
{
	[SerializeField] TaskItemUi Prefab;
	[SerializeField] Text SceneName;

	List<TaskItemUi> TaskItemPool = new List<TaskItemUi>();
	List<Task> LastTask;
	
	void Awake()
	{
		Prefab.gameObject.SetActive(false);
	}

	void Update()
	{
		if (Theatre.Instance?.CurrentScript.CurrentScene == null)
		{
			return;
		}

		var scene = Theatre.Instance.CurrentScript.CurrentScene;

		SceneName.text = $"Scene: {scene}";

		if (LastTask != scene.Tasks)
		{
			UpdateTasks(scene);
			LastTask = scene.Tasks;
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
		var instance = Instantiate<TaskItemUi>(Prefab, transform);

		TaskItemPool.Add(instance);
		
		return instance;
	}
}
