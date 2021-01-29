using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Script: MonoBehaviour
{
	public bool Finished {get 
		{	return SceneIndex >= Scenes.Count - 1 &&
			CurrentScene != null &&
			CurrentScene.State == eSceneState.Completed;}}
			
	public Scene CurrentScene {get {return Scenes[SceneIndex];}}
	public int SceneIndex;
	public List<Scene> Scenes;
	public Transform PropHolder;

	public float Rating;

	void Update()
	{
		int loop = 0;
		int newSceneIndex = 0;
		foreach (var scene in Scenes)
		{
			scene.Update();
			if (scene.State == eSceneState.Completed)
			{
				newSceneIndex += 1;
			}

			loop += 1;
		}

		if (newSceneIndex >= Scenes.Count)
		{
			newSceneIndex = Scenes.Count-1;
		}

		if (SceneIndex != newSceneIndex)
		{
			Logger.Log($"Script setting SceneIndex {SceneIndex} -> {newSceneIndex}");
			SceneIndex = newSceneIndex;
		}
		
		if (Scenes[SceneIndex].State == eSceneState.NotStarted)
		{
			Scenes[SceneIndex].SetState(eSceneState.InProgress);
		}

		UpdateRatings();
	}

	void UpdateRatings()
	{
		float rating = 0;
		int tasksCompleted = 0;
		foreach (var scene in Scenes)
		{
			if (scene.State == eSceneState.NotStarted)
			{
				continue;
			}

			foreach (var task in scene.Tasks)
			{
				var playerTask = task as PlayerTask;
				if (playerTask != null)
				{
					rating += playerTask.GetRating();
					tasksCompleted += 1;
				}
			}
		}

		if (tasksCompleted == 0)
		{
			tasksCompleted = 1;
		}
		Rating = rating / tasksCompleted;
	}
}