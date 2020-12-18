using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItemUi : MonoBehaviour
{
	[SerializeField] Text SceneName;
	[SerializeField] Animator Animator;
	[SerializeField] PlayerIcon PlayerIcon;

	Task Task;

	public void Setup(Task task)
	{
		Task = task;
		gameObject.SetActive(true);
		SceneName.text = Task.TaskId;

		if (Task.PlayerDoingTask == null)
		{
			PlayerIcon.gameObject.SetActive(false);
		}
		else
		{
			PlayerIcon.gameObject.SetActive(true);
			PlayerIcon.SetIcon(Task.PlayerDoingTask);
		}
	}

	void Update()
	{
		Animator.SetBool("Finished", Task.State == eTaskState.Completed);
	}
}
