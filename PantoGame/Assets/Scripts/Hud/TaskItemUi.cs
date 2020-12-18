using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItemUi : MonoBehaviour
{
	[SerializeField] Text SceneName;
	[SerializeField] Animator Animator;

	Task Task;

	public void Setup(Task task)
	{
		Task = task;
		gameObject.SetActive(true);
		SceneName.text = Task.TaskId;
	}

	void Update()
	{
		Animator.SetBool("Finished", Task.State == eTaskState.Completed);
	}
}
