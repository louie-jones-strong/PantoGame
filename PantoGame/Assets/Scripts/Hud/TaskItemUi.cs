using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItemUi : MonoBehaviour
{
	[SerializeField] Image TaskTypeIcon;
	[SerializeField] Text TaskName;
	[SerializeField] Image ProgressBar;
	[SerializeField] Animator Animator;
	[SerializeField] PlayerIcon PlayerIcon;

	Task Task;

	public void Setup(Task task)
	{
		Task = task;
		gameObject.SetActive(true);
		TaskName.text = Task.TaskId;
	}

	void Update()
	{
		Animator.SetBool("Finished", Task.State == eTaskState.Completed);
		Animator.SetBool("ShowPlayerIcon", Task.PlayerDoingTask != null);
		if (Task.PlayerDoingTask != null)
		{
			PlayerIcon.SetIcon(Task.PlayerDoingTask);
		}
		ProgressBar.fillAmount = Task.Progress;

		TaskTypeIcon.sprite = Task.TaskIcon;
		TaskTypeIcon.gameObject.SetActive(Task.TaskIcon != null);
	}
}
