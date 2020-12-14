using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItemUi : MonoBehaviour
{
	[SerializeField] Text SceneName;

	public void Setup(Task task)
	{
		gameObject.SetActive(true);
		SceneName.text = task.TaskId;
	}
}
