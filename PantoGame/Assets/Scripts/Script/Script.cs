using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Script
{
	public bool Finished {get; private set;}
	public Scene CurrentScene {get {return Scenes[SceneIndex];}}
	public int SceneIndex;
	public List<Scene> Scenes;

	//todo add textures to add to theater walls
	//todo add costume sprites

	public void Update()
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

		if (SceneIndex != newSceneIndex)
		{
			Logger.Log($"Script setting SceneIndex {SceneIndex} -> {newSceneIndex}");
			SceneIndex += newSceneIndex;
		}
	}
}