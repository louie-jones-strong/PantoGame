using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskListUi : MonoBehaviour
{
	[SerializeField] TaskItemUi Prefab;
	[SerializeField] Text SceneName;
	
	void Update()
	{
		if (Theatre.Instance?.CurrentScript.CurrentScene == null)
		{
			return;
		}

		var scene = Theatre.Instance.CurrentScript.CurrentScene;

		SceneName.text = $"Scene: {scene.name}";

		foreach (var task in scene.Tasks)
		{
			
		}
	}
}
